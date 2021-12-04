using Microsoft.Xna.Framework;
using PET.Config;
using PET.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace PET
{
    public class PetItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (DebugConfig.Instance.itemDebug)
            {
                if (item.type < Main.maxItemTypes)
                {
                    var v = VanillaPetData.Get(item.type);
                    if (v != null)
                    {
                        tooltips.Add(new TooltipLine(mod, "PetItem", string.Format("{0}, {1}", v.name, v.itemID)) { overrideColor = new Color(250, 250, 122, 255)});
                    }
                }
                else
                {
                    var m = ModPetData.Get(item.type);
                    if (m != null)
                    {
                        tooltips.Add(new TooltipLine(mod, "PetItem", string.Format("{0}, {1}, {2}, {3}, {4}", m.name, m.mod, m.itemName, m.buffName, m.itemID)) { overrideColor = new Color(250, 250, 122, 255)});
                    }
                }
            }
        }
    }
}