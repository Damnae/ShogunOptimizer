using ShogunOptimizer.ArtifactSets;
using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShogunOptimizer.BuildTargets
{
    public class IttoTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => "Itto";
        public override bool AllowUnequipped => true;
        public override bool UseGeneratedArtifacts => true;

        public override Type[] UsefulSets => new Type[]
{
            typeof(GladiatorsFinale),
            typeof(ShimenawasReminiscence),
            typeof(HuskOfOpulentDreams),
        };
        public override StatType[] UsefulMainStats { get; } = { StatType.DefPercent, StatType.AtkPercent, StatType.CritRate, StatType.CritDamage, StatType.GeoDmgBonus, };
        public override StatType[] UsefulSubStats { get; } = { StatType.DefPercent, StatType.AtkPercent, StatType.CritRate, StatType.CritDamage, StatType.DefFlat, StatType.AtkFlat, };

        public override void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons)
        {
            character = new Itto
            {
                Level = 90,
                AttackLevel = 10,
                SkillLevel = 10,
                BurstLevel = 10,
                Constellation = 0,

                BurstActive = true,
            };

            character.Stats[(int)StatType.ShieldStrength] += .15; // geo resonance
            character.Stats[(int)StatType.DmgBonus] += .15; // geo resonance
            character.Stats[(int)StatType.GeoResShred] += .2; // geo resonance

            character.Stats[(int)StatType.DefPercent] += .25; // gorou's passive
            character.Stats[(int)StatType.DefFlat] += 371; // gorou's E
            character.Stats[(int)StatType.GeoDmgBonus] += .15; // gorou's E

            enemy = new Enemy { Level = 90, };

            weapons = new List<Weapon>
            {
                new SerpentSpine(1),
                //new Whiteblind(5),
                //new RedhornStonethresher(1),
            };
        }

        public override ReadOnlyDictionary<Type, object> GetConfigs() => new Dictionary<Type, object>
        {
        }.AsReadOnly();

        public override bool FilterBuild(Build build, Character character, Enemy enemy)
            => true;

        public override double Evaluate(Build build, Character character, Enemy enemy) =>
            character.Calculate(Itto.PropertyAttack1, build, HitType.Averaged, enemy)
            + character.Calculate(Itto.PropertyAttack2, build, HitType.Averaged, enemy)
            + character.Calculate(Itto.PropertyAttack3, build, HitType.Averaged, enemy)
            + character.Calculate(Itto.PropertyAttack4, build, HitType.Averaged, enemy)
            + character.Calculate(Itto.PropertyChargedCombo, build, HitType.Averaged, enemy) * 4
             + character.Calculate(Itto.PropertyChargedFinal, build, HitType.Averaged, enemy)
             + character.Calculate(Itto.PropertySkill, build, HitType.Averaged, enemy);

        public override void DisplayResults(Build build, Character character, Enemy enemy)
        {
            Console.WriteLine($"Geo DMG Bonus: {character.GetStat(StatType.GeoDmgBonus, build):P}");
            Console.WriteLine();
            Console.WriteLine($"Attack 1: {character.Calculate(Itto.PropertyAttack1, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyAttack1, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyAttack1, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Attack 2: {character.Calculate(Itto.PropertyAttack2, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyAttack2, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyAttack2, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Attack 3: {character.Calculate(Itto.PropertyAttack3, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyAttack3, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyAttack3, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Attack 4: {character.Calculate(Itto.PropertyAttack4, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyAttack4, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyAttack4, build, HitType.Averaged, enemy):#})");
            Console.WriteLine();
            Console.WriteLine($"Charged Combo: {character.Calculate(Itto.PropertyChargedCombo, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyChargedCombo, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyChargedCombo, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Charged Final: {character.Calculate(Itto.PropertyChargedFinal, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyChargedFinal, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyChargedFinal, build, HitType.Averaged, enemy):#})");
            Console.WriteLine($"Charged Single: {character.Calculate(Itto.PropertyChargedSingle, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertyChargedSingle, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertyChargedSingle, build, HitType.Averaged, enemy):#})");
            Console.WriteLine();
            Console.WriteLine($"Skill: {character.Calculate(Itto.PropertySkill, build, HitType.Normal, enemy):#} - {character.Calculate(Itto.PropertySkill, build, HitType.Critical, enemy):#} ({character.Calculate(Itto.PropertySkill, build, HitType.Averaged, enemy):#})");
        }
    }
}
