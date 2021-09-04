using System;

namespace ShogunOptimizer.ArtifactSets
{
    public class GladiatorsFinale : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.AtkPercent:
                    return .18;
                case 4 when statType == StatType.AttackDmgBonus:
                    return .35;
            }
            return 0;
        }
    }
}
