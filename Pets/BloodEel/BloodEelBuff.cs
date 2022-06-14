using Terraria;
using Terraria.ModLoader;

namespace TVanities.Pets.BloodEel
{
    public class BloodEelBuff : PetBuffBase
    {
        protected override int PetProj => ModContent.ProjectileType<BloodEelPet>();

        protected override ref bool ActiveFlag(Player player)
        {
            return ref player.Pet().petBloodEel;
        }
    }
}