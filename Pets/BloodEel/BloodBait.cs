using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TVanities.Pets.BloodEel
{
    public sealed class BloodBait : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<BloodEelPet>(), ModContent.BuffType<BloodEelBuff>());
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
        }
    }
}