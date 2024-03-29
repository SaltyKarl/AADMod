using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;

namespace AADMod
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        static Utils()
        {
            foreach (var def in DefDatabase<ThingDef>.AllDefs)
            {
                var ext = def.GetModExtension<RemoveComps>();
                if (def.comps != null && ext != null)
                {
                    def.comps = def.comps.Where((CompProperties c) => !ext.compProps.Contains(c.GetType())).ToList();
                }
            }
        }

        public static bool IsMechanoidHacked(this Pawn pawn)
        {
            return pawn.Faction != null && pawn.Faction == Faction.OfPlayerSilentFail
                            && pawn.health.hediffSet.GetFirstHediffOfDef(AAD_DefOf.AAD_MechControllable) != null;
        }
    }
}
