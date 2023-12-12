using HarmonyLib;
using RimWorld;
using Verse;

namespace AADMod
{
    [HarmonyPatch(typeof(PawnComponentsUtility), "AddAndRemoveDynamicComponents")]
    public static class PawnComponentsUtility_AddAndRemoveDynamicComponents
    {
        public static void Postfix(Pawn pawn)
        {
            if (pawn.IsMechanoidHacked())
            {
                AssignMechComponents(pawn);
            }
        }

        public static void AssignMechComponents(Pawn pawn)
        {
            if (pawn.drafter is null)
            {
                pawn.drafter = new Pawn_DraftController(pawn);
            }
        }
    }
}
