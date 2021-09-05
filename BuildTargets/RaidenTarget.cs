using ShogunOptimizer.Characters;
using ShogunOptimizer.Weapons;
using System;
using System.Collections.Generic;

namespace ShogunOptimizer.BuildTargets
{
    public class RaidenTarget : BuildTarget
    {
        public override bool UpgradeArtifactsToLvl20 => true;
        public override string EquippedTo => "raidenshogun";
        public override bool AllowUnequipped => true;

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
            //character.Stats[(int)StatType.AtkFlat] += 815; // Bennett
            //character.Stats[(int)StatType.AtkPercent] += .2; // Bennett's 4 pieces NO

            enemy = new Enemy { Level = 90, };

            weapons = new List<Weapon>
            {
                new TheCatch(5),
                //new StaffOfHoma(1) { Under50PercentHp = true, },
                //new EngulfingLightning(1),
            };
        }

        public override void FilterArtifacts(ArtifactSource artifactSource)
        {
            artifactSource.Sands.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.EnergyRecharge));
            artifactSource.Goblets.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.ElectroDmgBonus));
            artifactSource.Circlets.RemoveAll(p => !(p.Stats[0].Item1 == StatType.AtkPercent || p.Stats[0].Item1 == StatType.CritRate || p.Stats[0].Item1 == StatType.CritDamage));
        }

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
