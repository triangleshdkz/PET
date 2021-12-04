using Terraria.ModLoader.Config;

namespace PET.Config
{
    public class DebugConfig : ModConfig
    {
        public static DebugConfig Instance { get; internal set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public bool itemDebug;

        public bool multipetDebug;

        public bool petBagVanillaDrops;

        public override void OnLoaded()
        {
            Instance = this;
        }
    }
}