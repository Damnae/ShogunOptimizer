using ShogunOptimizer.ArtifactSets;
using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;

namespace ShogunOptimizer.BuildTargets
{
    public class KokomiTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => null;
        public override bool AllowUnequipped => true;
        public override bool UseGeneratedArtifacts => true;

        public override Type[] UsefulSets => new Type[]
        {
            typeof(EmblemOfSeveredFate),
            typeof(GladiatorsFinale),
            typeof(HeartOfDepth),
            typeof(MaidenBeloved),
            typeof(NoblesseOblige),
            typeof(ShimenawasReminiscence),
            typeof(TenacityOfTheMillelith),
            typeof(WanderersTroupe),
        };
        public override StatType[] UsefulMainStats { get; } = { StatType.HpPercent, StatType.AtkPercent, StatType.HydroDmgBonus, StatType.HealBonus, };
        public override StatType[] UsefulSubStats { get; } = { StatType.HpPercent, StatType.HpFlat, StatType.AtkPercent, StatType.AtkFlat, };

        public bool EvaluateHealing = false;

        public override void Initialize(out Character character, out Enemy enemy, out ICollection<Weapon> weapons)
        {
            character = new Kokomi
            {
                Level = 90,
                AttackLevel = 10,
                SkillLevel = 10,
                BurstLevel = 10,
                Constellation = 0,
            };

            //character.Stats[(int)StatType.BurstDmgBonus] += .003 * 70; // Raiden's E
            
            //character.Stats[(int)StatType.AtkPercent] += .25; // Pyro resonance
            //character.Stats[(int)StatType.AtkFlat] += 815; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO

            enemy = new Enemy { Level = 90, };

            weapons = new List<Weapon>
            {
                new EverlastingMoonglow(1),
            };
        }

        public override bool FilterBuild(Build build, Character character, Enemy enemy)
            => true;

        public override double Evaluate(Build build, Character character, Enemy enemy)
        {
            if (EvaluateHealing)
                return character.Calculate(Kokomi.PropertyBurstHealing, build, HitType.Averaged, enemy) * 10
                    + character.Calculate(Kokomi.PropertySkillHealing, build, HitType.Averaged, enemy) * 10;
            else
                return character.Calculate(Kokomi.PropertyBurstInitial, build, HitType.Averaged, enemy)
                    + character.Calculate(Kokomi.PropertySkillDamage, build, HitType.Averaged, enemy) * 5
                    + character.Calculate(Kokomi.PropertyBurstSkillDamage, build, HitType.Averaged, enemy) * 5
                    + character.Calculate(Kokomi.PropertyBurstAttack1, build, HitType.Averaged, enemy) * 3
                    + character.Calculate(Kokomi.PropertyBurstAttack2, build, HitType.Averaged, enemy) * 3
                    + character.Calculate(Kokomi.PropertyBurstAttack3, build, HitType.Averaged, enemy) * 3
                    + character.Calculate(Kokomi.PropertyBurstAttack3C1, build, HitType.Averaged, enemy) * 3
                    + character.Calculate(Kokomi.PropertyBurstCharged, build, HitType.Averaged, enemy) * 3;
        }

        public override void DisplayResults(Build build, Character character, Enemy enemy)
        {
            Console.WriteLine($"Hydro DMG Bonus: {character.GetStat(StatType.HydroDmgBonus, build):P}");
            Console.WriteLine($"Healing Bonus: {character.GetStat(StatType.HealBonus, build):P}");
            Console.WriteLine();
            Console.WriteLine($"E Healing: {character.Calculate(Kokomi.PropertySkillHealing, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Healing: {character.Calculate(Kokomi.PropertyBurstHealing, build, HitType.Averaged, enemy):#} (per Normal/Charged Attacks hit)");
            Console.WriteLine();
            Console.WriteLine($"Attack1: {character.Calculate(Kokomi.PropertyAttack1, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Attack2: {character.Calculate(Kokomi.PropertyAttack2, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Attack3: {character.Calculate(Kokomi.PropertyAttack3, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Charged Attack: {character.Calculate(Kokomi.PropertyCharged, build, HitType.Averaged, enemy):#}");
            Console.WriteLine();
            Console.WriteLine($"E Damage: {character.Calculate(Kokomi.PropertySkillDamage, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"E Damage (Burst): {character.Calculate(Kokomi.PropertyBurstSkillDamage, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Initial Damage: {character.Calculate(Kokomi.PropertyBurstInitial, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Attack1: {character.Calculate(Kokomi.PropertyBurstAttack1, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Attack2: {character.Calculate(Kokomi.PropertyBurstAttack2, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Attack3: {character.Calculate(Kokomi.PropertyBurstAttack3, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Attack3 C1: {character.Calculate(Kokomi.PropertyBurstAttack3C1, build, HitType.Averaged, enemy):#}");
            Console.WriteLine($"Q Charged Attack: {character.Calculate(Kokomi.PropertyBurstCharged, build, HitType.Averaged, enemy):#}");
        }
    }
}
