using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TVanities
{
    public sealed class PetsPlayer : ModPlayer
    {
        public bool petBloodEel;

        public override void ResetEffects()
        {
            petBloodEel = false;
        }
    }
}