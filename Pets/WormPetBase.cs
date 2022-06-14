using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TVanities.Pets
{
    public abstract class WormPetBase : ModProjectile
    {
        /// <summary>
        /// Spacing between the head and first segment
        /// </summary>
        public virtual int FirstSegmentSpacing => (int)Math.Sqrt(Projectile.width * Projectile.height);
        /// <summary>
        /// Spacing between each segment
        /// </summary>
        public virtual int SegmentSpacing => (int)Math.Sqrt(Projectile.width * Projectile.height);
        /// <summary>
        /// An adjuster for the worm's rotation
        /// </summary>
        public virtual float RotationOffset => -MathHelper.PiOver2;
        /// <summary>
        /// All segments. The last index will be a tail, please keep this over 0
        /// </summary>
        public virtual int Length => 3;

        public IWormSegment[] segments;

        public override void OnSpawn(IEntitySource source)
        {
            segments = GetSegmentList(Length);
        }

        public abstract ref bool Active(Player player);

        protected virtual IWormSegment[] GetSegmentList(int length)
        {
            var list = new IWormSegment[length];
            var center = Projectile.Center;
            for (int i = 0; i < length; i++)
            {
                list[i] = new WormSegmentBase(center, DetermineSegmentFrame(length, i));
            }
            return list;
        }
        public virtual int DetermineSegmentFrame(int length, int i)
        {
            return i + 1 == length ? 2 : 1;
        }

        public sealed override void AI()
        {
            if (segments == null)
            {
                segments = GetSegmentList(Length);
            }
            Helpers.UpdateProjActive(Projectile, ref Active(Main.player[Projectile.owner]));
            var plrCenter = Main.player[Projectile.owner].Center;
            var difference = plrCenter - Projectile.Center;
            HeadAI(Projectile.Center, plrCenter, difference, (float)Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y));

            ConnectSegment(Projectile.Center + Projectile.velocity, FirstSegmentSpacing, 0);
            for (int i = 1; i < segments.Length; i++)
            {
                ConnectSegment(segments[i - 1].Center, SegmentSpacing, i);
            }
        }

        public virtual void HeadAI(Vector2 center, Vector2 plrCenter, Vector2 difference, float lengthFromPlr)
        {
            if (lengthFromPlr > 2000f)
            {
                Projectile.Center = plrCenter;
                Projectile.netUpdate = true;
                return;
            }
            HeadAI_GetSpeedValues(lengthFromPlr, out float speed, out float turnSpeed);
            if (lengthFromPlr > 120f)
            {
                float currentAngle = Projectile.velocity.ToRotation();
                float gotoAngle = currentAngle.AngleTowards(difference.ToRotation(), MathHelper.PiOver2 * 3);
                Projectile.velocity = currentAngle.AngleLerp(gotoAngle, turnSpeed).ToRotationVector2() * speed;
                Projectile.rotation = Projectile.velocity.ToRotation() - RotationOffset;
            }
            if (Projectile.velocity == new Vector2(0f, 0f))
            {
                Projectile.velocity = new Vector2(speed, 0f).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                Projectile.rotation = Projectile.velocity.ToRotation() - RotationOffset;
            }
        }
        protected virtual void HeadAI_GetSpeedValues(float distance, out float speed, out float turnSpeed)
        {
            speed = 6f;
            if (distance > 200f)
            {
                speed += distance / 100f;
            }
            turnSpeed = 0.035f;
        }
        public virtual void ConnectSegment(Vector2 to, float spacing, int i)
        {
            var segment = segments[i];
            Vector2 difference = to - segment.Center;
            if (difference.Length() > spacing)
            {
                segment.SnapVector = -Vector2.Normalize(difference) * spacing;
                segment.Center = to + segment.SnapVector;
                segment.Rotation = segment.SnapVector.ToRotation() + RotationOffset;
                OnSnapSegment(to, i);
            }
        }
        protected virtual void OnSnapSegment(Vector2 to, int i)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (segments == null)
            {
                return false;
            }

            var texture = TextureAssets.Projectile[Type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            var frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            var origin = frame.Size() / 2f;

            for (int i = segments.Length - 1; i >= 0; i--)
            {
                DrawSegment(texture, frame, origin, i);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, GetLight(Projectile.Center), Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }
        public void DrawSegment(Texture2D texture, Rectangle frame, Vector2 origin, int i)
        {
            frame.Y += frame.Height * segments[i].Frame;
            Main.spriteBatch.Draw(texture, segments[i].Center - Main.screenPosition, frame, GetLight(segments[i].Center), segments[i].Rotation, origin, Projectile.scale, segments[i].Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public Color GetLight(Vector2 location)
        {
            return Lighting.GetColor((int)location.X / 16, (int)location.Y / 16);
        }
    }
    public interface IWormSegment
    {
        public Vector2 Center { get; set; }
        public Vector2 SnapVector { get; set; }
        public float Rotation { get; set; }
        public int Direction { get; set; }
        public int Frame { get; set; }
    }
    public class WormSegmentBase : IWormSegment
    {
        public Vector2 Center { get; set; }
        public Vector2 SnapVector { get; set; }
        public float Rotation { get; set; }
        public int Direction { get; set; }
        public int Frame { get; set; }
        public WormSegmentBase(Vector2 position, int frame = 1)
        {
            Center = position;
            Frame = frame;
        }
    }
}