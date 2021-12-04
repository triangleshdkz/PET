using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Terraria;
using Terraria.ID;

namespace PET.DataStructures
{
    internal class PetIO
    {
        public static readonly string FilePath = Main.SavePath + Path.DirectorySeparatorChar;

        internal static void createVanillaPetData()
        {
            string filePath = FilePath + "VanillaPetData.json";
            using (var stream = File.Create(filePath))
            {
                List<VanillaPetData> vanilla = new List<VanillaPetData>
                {
                    new VanillaPetData("Bunny", ItemID.Carrot, BuffID.PetBunny),
                    new VanillaPetData("BabyDinosaur", ItemID.AmberMosquito, BuffID.BabyDinosaur),
                    new VanillaPetData("BabyEater", ItemID.EatersBone, BuffID.BabyEater),
                    new VanillaPetData("BabyFaceMonster", ItemID.BoneRattle, BuffID.BabyFaceMonster),
                    new VanillaPetData("BabyGrinch", ItemID.BabyGrinchMischiefWhistle, BuffID.BabyGrinch),
                    new VanillaPetData("BabyHornet", ItemID.Nectar, BuffID.BabyHornet),
                    new VanillaPetData("BabyPenguin", ItemID.Fish, BuffID.BabyPenguin),
                    new VanillaPetData("BabySkeletronHead", ItemID.BoneKey, BuffID.BabySkeletronHead),
                    new VanillaPetData("BabySnowman", ItemID.ToySled, BuffID.BabySnowman),
                    new VanillaPetData("BabyTruffle", ItemID.StrangeGlowingMushroom, BuffID.BabyTruffle),
                    new VanillaPetData("BlackCat", ItemID.UnluckyYarn, BuffID.BlackCat),
                    new VanillaPetData("CompanionCube", ItemID.CompanionCube, BuffID.CompanionCube),
                    new VanillaPetData("CursedSapling", ItemID.CursedSapling, BuffID.CursedSapling),
                    new VanillaPetData("EyeballSpring", ItemID.EyeSpring, BuffID.EyeballSpring),
                    new VanillaPetData("Hoardagron", ItemID.DD2PetDragon, BuffID.PetDD2Dragon),
                    new VanillaPetData("Lizard", ItemID.LizardEgg, BuffID.PetLizard),
                    new VanillaPetData("MiniMinotaur", ItemID.TartarSauce, BuffID.MiniMinotaur),
                    new VanillaPetData("Parrot", ItemID.ParrotCracker, BuffID.PetParrot),
                    new VanillaPetData("Gato", ItemID.DD2PetGato, BuffID.PetDD2Gato),
                    new VanillaPetData("Puppy", ItemID.DogWhistle, BuffID.Puppy),
                    new VanillaPetData("Sapling", ItemID.Seedling, BuffID.PetSapling),
                    new VanillaPetData("Spider", ItemID.SpiderEgg, BuffID.PetSpider),
                    new VanillaPetData("Squashling", ItemID.MagicalPumpkinSeed, BuffID.Squashling),
                    new VanillaPetData("TikiSpirit", ItemID.TikiTotem, BuffID.TikiSpirit),
                    new VanillaPetData("Turtle", ItemID.Seaweed, BuffID.PetTurtle),
                    new VanillaPetData("Zephyr", ItemID.ZephyrFish, BuffID.ZephyrFish),
                };
                string value = JsonConvert.SerializeObject(vanilla, Formatting.Indented);
                byte[] arr = Encoding.ASCII.GetBytes(value);
                stream.Write(arr, 0, arr.Length);
            }
        }

        internal static void createModPetData()
        {
            string filePath = FilePath + "ModPetData.json";
            using (var stream = File.Create(filePath))
            {
                List<ModPetData> vanilla = new List<ModPetData>
                {
                    new ModPetData("BloodEel", "BloodEel_Item", "BloodEel_Buff"),
                    new ModPetData("Dreadnautilus", "Dreadnautilus_Item", "Dreadnautilus_Buff"),
                    new ModPetData("StarplateVoyager", "StarplateVoyager_Item", "StarplateVoyager_Buff"),
                    new ModPetData("VlitchGigapede", "VlitchGigapede_Item", "VlitchGigapede_Buff"),
                };
                string value = JsonConvert.SerializeObject(vanilla, Formatting.Indented);
                byte[] arr = Encoding.ASCII.GetBytes(value);
                stream.Write(arr, 0, arr.Length);
            }
        }

        public static List<T> ReadPetData<T>() where T : PetData
        {
            string name = "DataStructures/PremadeData/" + typeof(T).Name + ".json";
            using (var stream = PET.Instance.GetFileStream(name, false))
            {
                using (var reader = new StreamReader(stream))
                {
                    var value = JsonConvert.DeserializeObject<List<T>>(reader.ReadToEnd());
                    return value;
                }
            }
        }
    }
}