using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PET
{
    public abstract class WormPet : ModProjectile
    {
        public virtual int GetSpacing() => (int)Math.Sqrt(projectile.width * projectile.height);

        public virtual bool IsHead => projectile.ai[0] <= 1f;

        public virtual float RotationOffset => -MathHelper.PiOver2;

        protected virtual void GetSpeedValues(float distance, out float speed, out float turnSpeed)
        {
            speed = 6f + distance / 400f;
            turnSpeed = 0.035f;
        }

        public virtual void HeadAI(Vector2 center, Vector2 plrCenter, Vector2 difference, float lengthFromPlr)
        {
            GetSpeedValues(lengthFromPlr, out float speed, out float turnSpeed);
            if (lengthFromPlr > 120f)
            {
                float currentAngle = projectile.velocity.ToRotation();
                float gotoAngle = Utils.AngleTowards(currentAngle, difference.ToRotation(), MathHelper.PiOver2 * 3);
                projectile.velocity = new Vector2(speed, 0f).RotatedBy(Utils.AngleLerp(currentAngle, gotoAngle, turnSpeed));
                projectile.rotation = projectile.velocity.ToRotation() - RotationOffset;
            }
            if (projectile.velocity == new Vector2(0f, 0f))
            {
                projectile.velocity = new Vector2(speed, 0f).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                projectile.rotation = projectile.velocity.ToRotation() - RotationOffset;
            }
            if (lengthFromPlr > 2000f)
            {
                projectile.Center = plrCenter;
                projectile.netUpdate = true;
            }
            ConnectWorm((int)projectile.ai[1]);
        }

        /// <summary>
        /// The length of the worm's body. This includes the tail. Setting this to zero or below means the worm will only be a head<para>
        /// Defaults to 3. Meaning 2 body segments and a tail.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public virtual int GetLength() => 3;

        public virtual void InitHead()
        {
            projectile.netUpdate = true;
        }

        public virtual void InitSegment(int totalSegments, int currentSegment, int headIndex)
        {
            projectile.ai[0] = 2f;
            if (currentSegment == totalSegments)
            {
                projectile.frame = 2;
            }
            else
            {
                projectile.frame = 1;
            }
            projectile.netUpdate = true;
        }

        protected virtual void PostConnect(Vector2 difference)
        {
        }

        protected void ConnectWorm(int index)
        {
            Projectile proj = Main.projectile[index];
            Vector2 projCenter = projectile.Center;
            Vector2 difference = proj.Center - projCenter;
            int spacing = GetSpacing();
            if (difference.Length() > spacing)
            {
                proj.Center = projCenter + Vector2.Normalize(difference) * spacing;
                ((WormPet)proj.modProjectile).PostConnect(difference);
                proj.rotation = difference.ToRotation() + RotationOffset;
                proj.netUpdate = true;
            }
        }

        private void SpawnSegments(int amount, Vector2 center)
        {
            Vector2 zero = new Vector2(0f, 0f);
            int lastProj = projectile.whoAmI;
            for (int i = 0; i < amount; i++)
            {
                int p = Projectile.NewProjectile(center, zero, projectile.type, 0, 0f, projectile.owner);
                Main.projectile[lastProj].ai[1] = p;
                Main.projectile[p].ai[1] = -1;
                Main.projectile[p].localAI[0] = projectile.whoAmI;
                ((WormPet)Main.projectile[p].modProjectile).InitSegment(amount, i + 1, projectile.whoAmI);
                lastProj = p;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            projectile.position += projectile.velocity;
            if (IsHead)
            {
                int lastProj = projectile.whoAmI;
                for (int i = 0; i < 20; i++)
                {
                    Projectile proj = Main.projectile[lastProj];
                    if (proj.ai[1] >= 0f)
                    {
                        lastProj = (int)proj.ai[1];
                        ((WormPet)proj.modProjectile).ConnectWorm((int)proj.ai[1]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return false;
        }

        protected abstract void CheckPlayer(Player plr, PetPlayer petPlr);

        public sealed override void AI()
        {
            CheckPlayer(Main.player[projectile.owner], Main.player[projectile.owner].GetModPlayer<PetPlayer>());
            Vector2 center = projectile.Center;
            switch (projectile.ai[0])
            {
                case 0f:
                projectile.ai[0] = 1f;
                InitHead();
                if (Main.myPlayer == projectile.owner)
                {
                    SpawnSegments(GetLength(), center);
                }
                break;

                case 1f:
                Vector2 plrCenter = Main.player[projectile.owner].Center;
                Vector2 difference = plrCenter - center;
                HeadAI(center, plrCenter, difference, (float)Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y));
                break;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] >= 0f)
            {
                Projectile next = Main.projectile[(int)projectile.ai[1]];
                if (next.ai[1] == -1f || Main.projectile[(int)next.ai[1]].ai[1] != next.ai[1])
                {
                    next.Kill();
                }
            }
        }

        public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

        public virtual void Draw()
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
            Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, Lighting.GetColor((int)projectile.position.X / 16, (int)projectile.position.Y / 16), projectile.rotation, frame.Size() / 2f, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}