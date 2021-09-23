using ShogunOptimizer.ArtifactSets;
using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShogunOptimizer.BuildTargets
{
    public class RaidenTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => "RaidenShogun";
        public override bool AllowUnequipped => true;
        public override bool UseGeneratedArtifacts => false;

        public override Type[] UsefulSets => new Type[]
{
            typeof(EmblemOfSeveredFate),
            /*
            typeof(GladiatorsFinale),
            typeof(NoblesseOblige),
            typeof(ShimenawasReminiscence),
            typeof(ThunderingFury),
            typeof(Thundersoother),
            */
        };
        public override StatType[] UsefulMainStats { get; } = { StatType.AtkPercent, StatType.CritRate, StatType.CritDamage, StatType.ElectroDmgBonus, StatType.EnergyRecharge, };
        public override StatType[] UsefulSubStats { get; } = { StatType.AtkPercent, StatType.AtkFlat, StatType.CritRate, StatType.CritDamage, StatType.EnergyRecharge, /* StatType.ElementalMastery, */ };

        public override void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons)
        {
            character = new Raiden
            {
                Level = 90,
                AttackLevel = 10,
                SkillLevel = 10,
                BurstLevel = 10,
                Constellation = 0,

                Resolve = 60,
            };

            character.Stats[(int)StatType.BurstDmgBonus] += .003 * 90; // Raiden's E

            //character.Stats[(int)StatType.AtkPercent] += .25; // Pyro resonance

            //character.Stats[(int)StatType.AtkFlat] += 1046; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO

            //character.Stats[(int)StatType.AtkPercent] += .48; // Thrilling
            //character.Stats[(int)StatType.ElectroDmgBonus] += .2; // Sucrose

            character.Stats[(int)StatType.ElectroResShred] += .4; // VV

            enemy = new Enemy { Level = 90, };

            weapons = new List<Weapon>
            {
                new TheCatch(5),
                //new StaffOfHoma(1) { Under50PercentHp = true, },
                //new EngulfingLightning(1),
            };
        }

        public override ReadOnlyDictionary<Type, object> GetConfigs() => new Dictionary<Type, object>
        {
        }.AsReadOnly();

        public override bool FilterBuild(Build build, Character character, Enemy enemy)
            => true;

        public override double Evaluate(Build build, Character character, Enemy enemy) =>
            character.Calculate(Raiden.PropertySkillInitial, build, HitType.Averaged, enemy) * 2
            + character.Calculate(Raiden.PropertySkillTick, build, HitType.Averaged, enemy) * 20
            + character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Averaged, enemy)
            + character.Calculate(Raiden.PropertyBurst2N4C1N2C, build, HitType.Averaged, enemy);

        public override void DisplayResults(Build build, Character character, Enemy enemy)
        {
            Console.WriteLine($"Electro DMG Bonus: {character.GetStat(StatType.ElectroDmgBonus, build):P}");
            Console.WriteLine($"Avg Burst Crit Multiplier: {character.GetCritMultiplier(build, DamageType.Burst, HitType.Averaged):P}");
            Console.WriteLine();
            Console.WriteLine($"Q Initial Damage: {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertyBurstInitial, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Q Avg Attack Damage: {character.Calculate(Raiden.PropertyBurst2N4C1N2C, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"E Initial Damage: {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"E Tick Damage: {character.Calculate(Raiden.PropertySkillTick, build, HitType.Normal, enemy):#} - {character.Calculate(Raiden.PropertySkillTick, build, HitType.Critical, enemy):#} (Avg {character.Calculate(Raiden.PropertySkillTick, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"E 20sec Avg Damage: {character.Calculate(Raiden.PropertySkillInitial, build, HitType.Averaged, enemy) * 2 + character.Calculate(Raiden.PropertySkillTick, build, HitType.Averaged, enemy) * 20:#}");
            Console.WriteLine();
            Console.WriteLine($"Q Energy Restored: {character.Calculate(Raiden.PropertyBurstEnergyRestored, build, HitType.Normal, enemy):#.##}");
        }
    }
}
