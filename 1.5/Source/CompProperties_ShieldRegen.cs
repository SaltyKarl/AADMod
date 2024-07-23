using Verse;

namespace AADMod;

public class CompProperties_ShieldRegen : CompProperties
{
    public float energyChargeRate = 0.1f;
    public float radius = 9.9f;

    public CompProperties_ShieldRegen()
    {
        compClass = typeof(Comp_ShieldRegen);
    }
}
