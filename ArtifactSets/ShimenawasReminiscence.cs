namespace ShogunOptimizer.ArtifactSets
{
    public class ShimenawasReminiscence : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.AtkPercent:
                    return .18;
                case 4 when statType == StatType.AttackDmgBonus || statType == StatType.ChargedDmgBonus || statType == StatType.PlungeDmgBonus:
                    return .50;
            }
            return 0;
        }
    }
}
