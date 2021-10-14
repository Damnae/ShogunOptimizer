namespace ShogunOptimizer.ArtifactSets
{
    public class HuskOfOpulentDreams : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.DefPercent:
                    return .3;
                case 4 when statType == StatType.DefPercent || statType == StatType.GeoDmgBonus:
                    return .06 * 4;
            }
            return 0;
        }
    }
}
