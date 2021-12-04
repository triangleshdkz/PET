using Terraria;
using Terraria.ModLoader;

namespace PET
{
    public abstract class PetModBuff : ModBuff
    {
        protected virtual void PostSetDefaults()
        {
        }

        public sealed override void SetDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
            PostSetDefaults();
        }

        public abstract void SetBuffValues(PetPlayer plr);

        public abstract bool ShouldSpawnPet(Player plr);

        public abstract void SpawnPet(Player plr);

        public sealed override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            SetBuffValues(player.GetModPlayer<PetPlayer>());
            if (ShouldSpawnPet(player) && player.whoAmI == Main.myPlayer)
            {
                SpawnPet(player);
            }
        }
    }
}