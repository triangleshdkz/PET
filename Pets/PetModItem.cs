using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PET
{
    public abstract class PetModItem<TPetProj, TPetBuff> : ModItem where TPetProj : ModProjectile where TPetBuff : ModBuff
    {
        protected virtual void PostSetDefaults()
        {
        }

        public sealed override void SetDefaults()
        {
            item.damage = 0;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 24;
            item.height = 24;
            item.UseSound = SoundID.Item2;
            item.useAnimation = 20;
            item.useTime = 20;
            item.noMelee = true;
            item.value = Item.sellPrice(gold: 1);
            item.rare = ItemRarityID.Orange;
            item.buffType = ModContent.BuffType<TPetBuff>();
            item.shoot = ModContent.ProjectileType<TPetProj>();
            PostSetDefaults();
        }

        public virtual void ApplyBuff(Player player)
        {
            player.AddBuff(item.buffType, 3600, true);
        }

        public sealed override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                ApplyBuff(player);
            }
        }
    }
}