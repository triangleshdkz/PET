using Microsoft.Xna.Framework;
using PET.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PET.Pets.Terraria
{
    public class BloodEel_Item : PetModItem<BloodEel_Projectile, BloodEel_Buff>
    {
    }

    public class BloodEel_Projectile : WormPet
    {
        public override int GetSpacing() => projectile.width;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        protected override void CheckPlayer(Player plr, PetPlayer petPlr)
        {
            if (plr.dead)
                petPlr.bloodEel = false;
            if (petPlr.bloodEel)
                projectile.timeLeft = 2;
        }

        public override int GetLength() => Main.netMode == NetmodeID.SinglePlayer ? ClientConfig.Instance.bloodEelLength : 3;

        public override void InitSegment(int totalSegments, int currentSegment, int headIndex)
        {
            base.InitSegment(totalSegments, currentSegment, headIndex);
            if (currentSegment != totalSegments)
                projectile.width = 14;
        }

        protected override void PostConnect(Vector2 difference)
        {
            projectile.spriteDirection = difference.X > 0f ? 1 : -1;
        }

        public override void PostAI()
        {
            if (IsHead)
                projectile.spriteDirection = projectile.velocity.X > 0f ? -1 : 1;
        }
    }

    public class BloodEel_Buff : PetModBuff
    {
        public override void SetBuffValues(PetPlayer plr) => plr.bloodEel = true;

        public override bool ShouldSpawnPet(Player plr) => plr.ownedProjectileCounts[ModContent.ProjectileType<BloodEel_Projectile>()] < 1;

        public override void SpawnPet(Player plr)
        {
            Projectile.NewProjectile(plr.Center, new Vector2(0f, 0f), ModContent.ProjectileType<BloodEel_Projectile>(), 0, 0f, plr.whoAmI);
        }
    }
}