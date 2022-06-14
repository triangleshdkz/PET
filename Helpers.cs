using Terraria;

namespace TVanities
{
    public static class Helpers
    {
        public static PetsPlayer Pet(this Player player)
        {
            return player.GetModPlayer<PetsPlayer>();
        }
        public static bool UpdateProjActive(Projectile projectile, ref bool active)
        {
            if (Main.player[projectile.owner].dead)
                active = false;
            if (active)
                projectile.timeLeft = 2;
            return active;
        }
    }
}