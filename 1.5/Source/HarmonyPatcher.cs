using HarmonyLib;
using Verse;

namespace AADMod;

[StaticConstructorOnStartup]
public static class HarmonyPatcher
{
    static HarmonyPatcher()
    {
        new Harmony("AADMod").PatchAll();
    }
}
