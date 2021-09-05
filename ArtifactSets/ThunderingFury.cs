namespace ShogunOptimizer.ArtifactSets
{
    public class ThunderingFury : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.ElectroDmgBonus:
                    return .15;
            }
            return 0;
        }
    }
}
