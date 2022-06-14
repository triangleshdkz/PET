using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TVanities.Pets.BloodEel;

namespace TVanities
{
    public class Drops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodEelHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodBait>(), 4));
            }
        }
    }
}