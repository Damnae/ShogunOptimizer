namespace ShogunOptimizer.ArtifactSets
{
    public class WanderersTroupe : ArtifactSet
    {
        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.ElementalMastery:
                    return 80;
                case 4 when statType == StatType.ChargedDmgBonus && (build.Weapon.Type == WeaponType.Bow || build.Weapon.Type == WeaponType.Catalyst):
                    return .35;
            }
            return 0;
        }
    }
}
