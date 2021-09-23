using ShogunOptimizer.ArtifactSets;
using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShogunOptimizer.BuildTargets
{
    public class XingqiuTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => "Xingqiu";
        public override bool AllowUnequipped => true;
        public override bool UseGeneratedArtifacts => false;

        public override Type[] UsefulSets => new Type[]
        {
            typeof(GladiatorsFinale),
            typeof(ShimenawasReminiscence),
            typeof(EmblemOfSeveredFate),
            typeof(HeartOfDepth),
            typeof(MaidenBeloved),
            typeof(NoblesseOblige),
            typeof(TenacityOfTheMillelith),
            typeof(WanderersTroupe),
        };
        public override StatType[] UsefulMainStats { get; } = { StatType.HpPercent, StatType.AtkPercent, StatType.HydroDmgBonus, StatType.CritRate, StatType.CritDamage, StatType.ElementalMastery, StatType.EnergyRecharge, };
        public override StatType[] UsefulSubStats { get; } = { StatType.CritDamage, StatType.CritRate, StatType.ElementalMastery, StatType.HpPercent, StatType.HpFlat, StatType.AtkPercent, StatType.AtkFlat, };

        public override void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons)
        {
            character = new Xingqiu
            {
                Level = 80,
                AttackLevel = 1,
                SkillLevel = 11,
                BurstLevel = 13,
                Constellation = 6,

                BaseHp = 9514,
                BaseAtk = 188,
                BaseDef = 705,
            };

            //character.Stats[(int)StatType.BurstDmgBonus] += .003 * 80; // Raiden's E

            character.Stats[(int)StatType.AtkPercent] += .25; // Pyro resonance
            //character.Stats[(int)StatType.AtkFlat] += 1046; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO
            
            enemy = new Enemy { Level = 84, /* AffectedBy = Element.Pyro, */ };

            weapons = new List<Weapon>
            {
                //new SacrificialSword(3),
                new PrimordialJadeCutter(1),
            };
        }

        public override ReadOnlyDictionary<Type, object> GetConfigs() => new Dictionary<Type, object>
        {
        }.AsReadOnly();

        public override bool FilterBuild(Build build, Character character, Enemy enemy)
            => character.GetStat(StatType.EnergyRecharge, build) >= 1.45 && character.GetStat(StatType.ElementalMastery, build) >= 100;

        public override double Evaluate(Build build, Character character, Enemy enemy)
        {
            return character.Calculate(Xingqiu.PropertyBurst, build, HitType.Averaged, enemy);
        }

        public override void DisplayResults(Build build, Character character, Enemy enemy)
        {
            Console.WriteLine($"Hydro DMG Bonus: {character.GetStat(StatType.HydroDmgBonus, build):P}");
            Console.WriteLine();
            Console.WriteLine($"Skill 1: {character.Calculate(Xingqiu.PropertySkillA, build, HitType.Normal, enemy):#} - {character.Calculate(Xingqiu.PropertySkillA, build, HitType.Critical, enemy):#} ({character.Calculate(Xingqiu.PropertySkillA, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Skill 2: {character.Calculate(Xingqiu.PropertySkillB, build, HitType.Normal, enemy):#} - {character.Calculate(Xingqiu.PropertySkillB, build, HitType.Critical, enemy):#} ({character.Calculate(Xingqiu.PropertySkillB, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Skill 1 (burst): {character.Calculate(Xingqiu.PropertySkillADuringBurst, build, HitType.Normal, enemy):#} - {character.Calculate(Xingqiu.PropertySkillADuringBurst, build, HitType.Critical, enemy):#} ({character.Calculate(Xingqiu.PropertySkillADuringBurst, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Skill 2 (burst): {character.Calculate(Xingqiu.PropertySkillBDuringBurst, build, HitType.Normal, enemy):#} - {character.Calculate(Xingqiu.PropertySkillBDuringBurst, build, HitType.Critical, enemy):#} ({character.Calculate(Xingqiu.PropertySkillBDuringBurst, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Burst: {character.Calculate(Xingqiu.PropertyBurst, build, HitType.Normal, enemy):#} - {character.Calculate(Xingqiu.PropertyBurst, build, HitType.Critical, enemy):#} ({character.Calculate(Xingqiu.PropertyBurst, build, HitType.Averaged, enemy):#})");
            Console.WriteLine();
            Console.WriteLine($"Skill Heal: {character.Calculate(Xingqiu.PropertySkillHeal, build, HitType.Averaged, enemy) * 4:#}");
            Console.WriteLine();
        }
    }
}
