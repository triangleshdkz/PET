using Microsoft.Xna.Framework;
using PET.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PET.Pets.SpiritMod
{
    public class StarplateVoyager_Item : PetModItem<StarplateVoyager_Projectile, StarplateVoyager_Buff>
    {
    }

    public class StarplateVoyager_Projectile : WormPet
    {
        public override int GetSpacing() => 20;

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

        public override int GetLength() => Main.netMode == NetmodeID.SinglePlayer ? ClientConfig.Instance.starplateVoyagerLength : 3;

        protected override void CheckPlayer(Player plr, PetPlayer petPlr)
        {
            if (plr.dead)
                petPlr.starplateVoyager = false;
            if (petPlr.starplateVoyager)
                projectile.timeLeft = 2;
        }
    }

    public class StarplateVoyager_Buff : PetModBuff
    {
        public override void SetBuffValues(PetPlayer plr) => plr.starplateVoyager = true;

        public override bool ShouldSpawnPet(Player plr) => plr.ownedProjectileCounts[ModContent.ProjectileType<StarplateVoyager_Projectile>()] < 1;

        public override void SpawnPet(Player plr)
        {
            Projectile.NewProjectile(plr.Center, new Vector2(0f, 0f), ModContent.ProjectileType<StarplateVoyager_Projectile>(), 0, 0f, plr.whoAmI);
        }
    }
}