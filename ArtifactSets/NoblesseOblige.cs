using System;

namespace ShogunOptimizer.ArtifactSets
{
    public class NoblesseOblige : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.BurstDmgBonus:
                    return .2;
                case 4 when statType == StatType.AtkPercent:
                    return .2;
            }
            return 0;
        }
    }
}
