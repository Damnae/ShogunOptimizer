namespace ShogunOptimizer.ArtifactSets
{
    public class MaidenBeloved : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.HealBonus:
                    return .15;
                case 4 when statType == StatType.HealReceived:
                    return .20;
            }
            return 0;
        }
    }
}
