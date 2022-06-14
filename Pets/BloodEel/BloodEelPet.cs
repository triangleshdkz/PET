using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TVanities.Pets.BloodEel
{
    public class BloodEelPet : WormPetBase
    {
        public override int FirstSegmentSpacing => Projectile.width;
        public override int SegmentSpacing => Projectile.width - 4;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override ref bool Active(Player player)
        {
            return ref player.Pet().petBloodEel;
        }

        public override int Length => 10;

        public override void PostAI()
        {
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
        }
        protected override void OnSnapSegment(Vector2 to, int i)
        {
            segments[i].Direction = Math.Sign(segments[i].SnapVector.X);
        }
    }
}