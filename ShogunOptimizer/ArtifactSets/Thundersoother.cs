using System;

namespace ShogunOptimizer.ArtifactSets
{
    public class Thundersoother : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 4 when statType == StatType.DmgBonus:
                    return .35;
            }
            return 0;
        }
    }
}
