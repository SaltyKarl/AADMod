using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace AADMod
{
    [HarmonyPatch(typeof(MechanitorUtility), "GetMechGizmos")]
    public static class MechanitorUtility_GetMechGizmos_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn mech)
        {
            if (mech.IsMechanoidHacked())
            {
                yield break;
            }
            else
            {
                foreach (var g in  __result)
                {
                    yield return g;
                }
            }
        }
    }
}
