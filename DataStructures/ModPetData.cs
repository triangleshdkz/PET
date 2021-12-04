using Newtonsoft.Json;
using Terraria.ModLoader;

namespace PET.DataStructures
{
    public class ModPetData : PetData
    {
        public string mod;

        public string itemName;

        public string buffName;

        [JsonIgnore]
        public readonly int itemID;

        [JsonIgnore]
        public readonly int buffID;

        internal ModPetData(string name, string itemName, string buffName) : base(name)
        {
            mod = PET.ModName;
            this.itemName = itemName;
            this.buffName = buffName;
            itemID = PET.Instance.ItemType(itemName);
            buffID = PET.Instance.BuffType(buffName);
        }

        public ModPetData(string name, Mod mod, string itemName, string buffName) : base(name)
        {
            this.mod = mod.Name;
            this.itemName = itemName;
            this.buffName = buffName;
            itemID = mod.ItemType(itemName);
            buffID = mod.BuffType(buffName);
        }

        [JsonConstructor]
        /// <summary>
        /// tbh this is only here so that json can deserialize properly
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mod"></param>
        /// <param name="itemName"></param>
        /// <param name="buffName"></param>
        public ModPetData(string name, string mod, string itemName, string buffName) : base(name)
        {
            var _mod = ModLoader.GetMod(mod);
            this.mod = mod;
            this.itemName = itemName;
            this.buffName = buffName;
            itemID = _mod.ItemType(itemName);
            buffID = _mod.BuffType(buffName);
        }

        public static ModPetData Get(int id)
        {
            return PET.supported.Find((m) => m.itemID == id);
        }
        public override int GetItem() => itemID;

        public override int GetBuff() => buffID;
    }
}