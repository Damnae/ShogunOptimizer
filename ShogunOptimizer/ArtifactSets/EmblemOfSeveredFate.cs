using System;

namespace ShogunOptimizer.ArtifactSets
{
    public class EmblemOfSeveredFate : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.EnergyRecharge:
                    return .2;
                case 4 when statType == StatType.BurstDmgBonus:
                    return Math.Min(.75, .25 * character.GetStat(StatType.EnergyRecharge, build));
            }
            return 0;
        }
    }
}
