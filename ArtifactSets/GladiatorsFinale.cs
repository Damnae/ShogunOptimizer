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
                case 4 when statType == StatType.AttackDmgBonus && 
                    (build.Weapon.Type == WeaponType.Sword || build.Weapon.Type == WeaponType.Claymore || build.Weapon.Type == WeaponType.Spear):
                    return .35;
            }
            return 0;
        }
    }
}
