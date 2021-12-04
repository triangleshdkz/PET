using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PET.Config
{
    public class ClientConfig : ModConfig
    {
        public static ClientConfig Instance { get; internal set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Worm Pet Lengths")]

        [Label("Blood Eel")]
        [Slider()]
        [Range(3, 20)]
        [DefaultValue(3)]
        public int bloodEelLength;

        [Label("Vlitch Gigapede")]
        [Slider()]
        [Range(3, 20)]
        [DefaultValue(3)]
        public int vlitchGigapedeLength;

        [Label("Starplate Voyager")]
        [Slider()]
        [Range(3, 20)]
        [DefaultValue(3)]
        public int starplateVoyagerLength;

        public override void OnLoaded()
        {
            Instance = this;
        }
    }
}