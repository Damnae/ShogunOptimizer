using ShogunOptimizer.ArtifactSets;
using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShogunOptimizer.BuildTargets
{
    public class HuTaoTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => null;
        public override bool AllowUnequipped => true;
        public override bool UseGeneratedArtifacts => false;

        public override Type[] UsefulSets => new Type[]
        {
            typeof(GladiatorsFinale),
            typeof(CrimsonWitchOfFlames),
        };
        public override StatType[] UsefulMainStats { get; } = { StatType.HpPercent, StatType.AtkPercent, StatType.PyroDmgBonus, StatType.CritRate, StatType.CritDamage, };
        public override StatType[] UsefulSubStats { get; } = { StatType.CritDamage, StatType.CritRate, StatType.ElementalMastery, StatType.HpPercent, StatType.AtkPercent, };

        public override void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons)
        {
            var under50Percent = true;

            character = new HuTao
            {
                Level = 90,
                AttackLevel = 10,
                SkillLevel = 10,
                BurstLevel = 10,
                Constellation = 0,

                ElementalSkillActive = true,
                Under50PercentHp = under50Percent,
            };

            //character.Stats[(int)StatType.BurstDmgBonus] += .003 * 70; // Raiden's E

            //character.Stats[(int)StatType.AtkPercent] += .25; // Pyro resonance
            //character.Stats[(int)StatType.AtkFlat] += 815; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO

            enemy = new Enemy { Level = 90, AffectedBy = Element.Hydro, };

            weapons = new List<Weapon>
            {
                new StaffOfHoma(1) { Under50PercentHp = under50Percent, },
            };
        }

        public override ReadOnlyDictionary<Type, object> GetConfigs() => new Dictionary<Type, object>
        {
            { typeof(CrimsonWitchOfFlames.Config), new CrimsonWitchOfFlames.Config() { Stacks = 1, } },
        }.AsReadOnly();

        public override bool FilterBuild(Build build, Character character, Enemy enemy)
            => true;

        public override double Evaluate(Build build, Character character, Enemy enemy)
        {
            return character.Calculate(HuTao.PropertyCharged, build, HitType.Averaged, enemy);
        }

        public override void DisplayResults(Build build, Character character, Enemy enemy)
        {
            Console.WriteLine($"Pyro DMG Bonus: {character.GetStat(StatType.PyroDmgBonus, build):P}");
            Console.WriteLine();
            Console.WriteLine($"Charged Attack: {character.Calculate(HuTao.PropertyCharged, build, HitType.Critical, enemy):#}");
            Console.WriteLine();
        }
    }
}
