using RimWorld;
using Verse;

namespace AADMod
{
    public class CompShieldEMPImmune : CompShield
    {
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (ShieldState != 0 || PawnOwner == null)
            {
                return;
            }
            if (dinfo.Def == DamageDefOf.EMP)
            {

            }
            else if (dinfo.Def.isRanged || dinfo.Def.isExplosive)
            {
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
    }
}
