using HarmonyLib;
using Verse;

namespace AAD;

public class AADMod : Mod
{
    private Harmony _harmony;
    
    public AADMod(ModContentPack content) : base(content)
    {
        _harmony = new Harmony("heraldarms.aad");
        _harmony.PatchAll();
    }
}