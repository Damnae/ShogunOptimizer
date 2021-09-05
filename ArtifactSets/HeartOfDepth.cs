namespace ShogunOptimizer.ArtifactSets
{
    public class HeartOfDepth : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.HydroDmgBonus:
                    return .15;
                case 4 when statType == StatType.AttackDmgBonus || statType == StatType.ChargedDmgBonus:
                    return .3;
            }
            return 0;
        }
    }
}
