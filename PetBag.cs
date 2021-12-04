using PET.Config;
using PET.Pets.ModOfRedemption;
using PET.Pets.SpiritMod;
using PET.Pets.Terraria;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PET
{
    public class PetBag : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Blue;
            item.consumable = true;
            item.maxStack = 999;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player) // TODO : create a better system for the pet bag
        {
            if (DebugConfig.Instance.petBagVanillaDrops)
            {
                List<int> choices = new List<int>();
                foreach (var v in PET.vanilla)
                {
                    choices.Add(v.itemID);
                }
                choices.Add(ModContent.ItemType<BloodEel_Item>());
                choices.Add(ModContent.ItemType<StarplateVoyager_Item>());
                choices.Add(ModContent.ItemType<VlitchGigapede_Item>());
                choices.Add(ModContent.ItemType<Dreadnautilus_Item>());
                player.QuickSpawnItem(choices[Main.rand.Next(choices.Count)]);
                return;
            }
            int choice = Main.rand.Next(3);
            int item;
            switch (choice)
            {
                default:
                item = ModContent.ItemType<BloodEel_Item>();
                break;

                case 1:
                item = ModContent.ItemType<StarplateVoyager_Item>();
                break;

                case 2:
                item = ModContent.ItemType<VlitchGigapede_Item>();
                break;

                case 3:
                item = ModContent.ItemType<Dreadnautilus_Item>();
                break;
            }
            player.QuickSpawnItem(item);
        }
    }
}