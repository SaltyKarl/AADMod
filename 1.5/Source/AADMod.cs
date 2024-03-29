using HarmonyLib;
using Verse;

namespace AADMod
{
    public class AADMod : Mod
    {
        public AADMod(ModContentPack pack) : base(pack)
        {
			new Harmony("AADMod").PatchAll();
        }
    }
}
