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
        DefShred,
    }
}