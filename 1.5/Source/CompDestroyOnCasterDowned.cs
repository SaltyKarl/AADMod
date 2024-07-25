using Verse;

namespace AADMod;

public class CompDestroyOnCasterDowned : ThingComp
{
    public Pawn caster;

    public override void CompTick()
    {
        base.CompTick();
        if (caster is { Downed: true })
        {
            parent.Destroy();
        }
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_References.Look(ref caster, "caster");
    }
}
