using Terraria.ModLoader;

namespace PET
{
    public class PetPlayer : ModPlayer
    {
        public bool tim;
        public bool bloodEel;
        public bool dreadnautilus;

        public bool starplateVoyager;

        public bool vlitchGigapede;

        public override void ResetEffects()
        {
            bloodEel = false;
            dreadnautilus = false;
            starplateVoyager = false;
            vlitchGigapede = false;
            tim = false;
        }
    }
}