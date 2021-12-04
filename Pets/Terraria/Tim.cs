using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PET.Pets.Terraria
{
    public class Tim_Item : PetModItem<Tim_Projectile, Tim_Buff>
    {
    }

    public class Tim_Projectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer petPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();
            if (player.dead)
                petPlayer.tim = false;
            if (petPlayer.tim)
                projectile.timeLeft = 2;
            projectile.velocity.Y += 0.2f;
        }
    }

    public class Tim_Buff : PetModBuff
    {
        public override void SetBuffValues(PetPlayer plr) => plr.tim = true;

        public override bool ShouldSpawnPet(Player plr) => plr.ownedProjectileCounts[ModContent.ProjectileType<Tim_Projectile>()] < 1;

        public override void SpawnPet(Player plr)
        {
            Projectile.NewProjectile(plr.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Tim_Projectile>(), 0, 0f, plr.whoAmI);
        }
    }
}
