using Terraria;

namespace PET.DataStructures
{
    public class VanillaPetData : PetData
    {
        public int itemID;
        public int buffID;

        internal static bool[] isVanillaPet { get; set; }

        public static bool IsVanillaPet(int id)
        {
            return id < Main.maxItemTypes && isVanillaPet[id];
        }

        public static VanillaPetData Get(int id)
        {
            return PET.vanilla.Find((v) => v.itemID == id);
        }

        public VanillaPetData(string name, int itemID, int buffID) : base(name)
        {
            this.itemID = itemID;
            this.buffID = buffID;
        }

        public override int GetItem() => itemID;

        public override int GetBuff() => buffID;
    }
}