using System;

namespace ShogunOptimizer.ArtifactSets
{
    public class CrimsonWitchOfFlames : ArtifactSet
    {
        public class Config
        {
            public int Stacks { get; init; } = 3;
        }

        public override double GetStat(StatType statType, Build build, Character character, int count)
        {
            switch (count)
            {
                case 2 when statType == StatType.PyroDmgBonus: 
                    return 0.15;

                case 4 when statType == StatType.PyroDmgBonus:
                    return 0.15 * 0.5 * Math.Min(3, build.GetConfig<Config>().Stacks);

                case 4 when statType == StatType.VaporizeDmgBonus || statType == StatType.MeltDmgBonus:
                    return 0.15;

                case 4 when statType == StatType.OverloadedDmgBonus || statType == StatType.BurningDmgBonus:
                    return .40;
            }

            return 0;
        }
    }
}
