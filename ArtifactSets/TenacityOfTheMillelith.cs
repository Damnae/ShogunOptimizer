namespace ShogunOptimizer.ArtifactSets
{
    public class TenacityOfTheMillelith : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.HpPercent:
                    return .2;
                case 4 when statType == StatType.AtkPercent:
                    return .2;
                case 4 when statType == StatType.ShieldStrength:
                    return .3;
            }
            return 0;
        }
    }
}
