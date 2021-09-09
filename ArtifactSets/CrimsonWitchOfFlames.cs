using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogunOptimizer.ArtifactSets
{
    public class CrimsonWitchOfFlames : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            var pyroDmgBonus = 0.15;

            switch (count)
            {
                case 2 when statType == StatType.PyroDmgBonus: 
                    return pyroDmgBonus;

                case 4 when statType == StatType.PyroDmgBonus:
                    return pyroDmgBonus * 0.5; // assumming elemental skill was used just once

                case 4 when statType == StatType.VaporizeDmgBonus || statType == StatType.MeltDmgBonus:
                    return 0.15;

                case 4 when statType == StatType.OverloadedDmgBonus || statType == StatType.BurningDmgBonus:
                    return .40;
            }

            return 0;
        }
    }
}
