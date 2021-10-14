namespace ShogunOptimizer
{
    public enum StatType
    {
        // Base stats
        AtkFlat,
        AtkPercent,
        DefFlat,
        DefPercent,
        HpFlat,
        HpPercent,
        CritRate,
        CritDamage,
        EnergyRecharge,
        ElementalMastery,
        HealBonus,

        ShieldStrength,
        HealReceived,

        // Dmg Bonuses
        DmgBonus,
        AttackDmgBonus,
        ChargedDmgBonus,
        PlungeDmgBonus,
        SkillDmgBonus,
        BurstDmgBonus,

        PhysicalDmgBonus,
        PyroDmgBonus,
        HydroDmgBonus,
        CryoDmgBonus,
        ElectroDmgBonus,
        AnemoDmgBonus,
        GeoDmgBonus,
        DendroDmgBonus,

        // Extra Damage
        ExtraDmg,
        AttackExtraDmg,
        ChargedExtraDmg,
        PlungeExtraDmg,
        SkillExtraDmg,
        BurstExtraDmg,

        PhysicalExtraDmg,
        PyroExtraDmg,
        HydroExtraDmg,
        CryoExtraDmg,
        ElectroExtraDmg,
        AnemoExtraDmg,
        GeoExtraDmg,
        DendroExtraDmg,

        // Reaction Bonuses
        OverloadedDmgBonus,
        BurningDmgBonus,
        VaporizeDmgBonus,
        MeltDmgBonus,
        ElectroChargedDmgBonus,
        SuperconductDmgBonus,
        SwirlDmgBonus,
        ShatterDmgBonus,

        // Crit Bonuses
        AttackCritRateBonus,
        AttackCritDamageBonus,
        ChargedCritRateBonus,
        ChargedCritDamageBonus,
        PlungeCritRateBonus,
        PlungeCritDamageBonus,
        SkillCritRateBonus,
        SkillCritDamageBonus,
        BurstCritRateBonus,
        BurstCritDamageBonus,

        // Enemy Def / Res Reductions
        PhysicalResShred,
        PyroResShred,
        HydroResShred,
        CryoResShred,
        ElectroResShred,
        AnemoResShred,
        GeoResShred,
        DendroResShred,
        ResShred,
        DefShred,
    }
}