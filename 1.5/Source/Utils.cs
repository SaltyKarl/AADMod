using System.Linq;
using RimWorld;
using Verse;

namespace AADMod;

[StaticConstructorOnStartup]
public static class Utils
{
    static Utils()
    {
        foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
        {
            RemoveComps ext = def.GetModExtension<RemoveComps>();
            if (def.comps != null && ext != null)
            {
                def.comps = def.comps.Where(c => !ext.compProps.Contains(c.GetType())).ToList();
            }
        }
    }

    public static bool IsMechanoidHacked(this Pawn pawn)
    {
        return pawn.Faction != null && pawn.Faction == Faction.OfPlayerSilentFail
                                    && pawn.health.hediffSet.GetFirstHediffOfDef(AAD_DefOf.AAD_MechControllable) != null;
    }
}
