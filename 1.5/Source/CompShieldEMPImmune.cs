using RimWorld;
using Verse;

namespace AADMod;

public class CompShieldEMPImmune : CompShield
{
    public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
    {
        absorbed = false;
        if (ShieldState != ShieldState.Active || PawnOwner == null)
        {
            return;
        }

        if (dinfo.Def == DamageDefOf.EMP)
        {
            return;
        }

        if (!dinfo.Def.isRanged && !dinfo.Def.isExplosive)
        {
            return;
        }

        energy -= dinfo.Amount * Props.energyLossPerDamage;
        if (energy < 0f)
        {
            Break();
        }
        else
        {
            AbsorbedDamage(dinfo);
        }

        absorbed = true;
    }
}
