using Microsoft.Xna.Framework.Graphics;
using PET.Config;
using PET.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace PET
{
    public class PET : Mod
    {
        public const string ModName = nameof(PET);

        public static PET Instance { get; private set; }

        public static List<VanillaPetData> vanilla;

        public static List<ModPetData> supported;

        public override void Load()
        {
            Instance = this;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            vanilla = PetIO.ReadPetData<VanillaPetData>();
            supported = PetIO.ReadPetData<ModPetData>();
            VanillaPetData.isVanillaPet = new bool[Main.maxItemTypes];
            foreach (var v in vanilla)
            {
                VanillaPetData.isVanillaPet[v.itemID] = true;
            }
        }

        public override void Unload()
        {
            vanilla = null;
            supported = null;
            DebugConfig.Instance = null;
            ClientConfig.Instance = null;
            Instance = null;
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, self.Rasterizer, null, Main.Transform);
            for (int i = Main.maxProjectiles - 1; i >= 0; i--)
            {
                if (Main.projectile[i].active && Main.projectile[i].type > Main.maxProjectileTypes && Main.projectile[i].modProjectile is WormPet pet)
                {
                    pet.Draw();
                }
            }
            Main.spriteBatch.End();
            orig(self);
        }
    }
}