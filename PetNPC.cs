using Terraria;
using Terraria.ModLoader;

namespace PET
{
    public class PetNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.boss && Main.rand.NextBool(4))
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<PetBag>());
            }
        }
    }
}