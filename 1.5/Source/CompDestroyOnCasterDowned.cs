using Verse;

namespace AADMod
{
    public class CompProperties_DestroyOnCasterDowned : CompProperties
    {
        public CompProperties_DestroyOnCasterDowned()
        {
            this.compClass = typeof(CompDestroyOnCasterDowned);
        }
    }

    public class CompDestroyOnCasterDowned : ThingComp
    {
        public Pawn caster;

        public override void CompTick()
        {
            base.CompTick();
            if (caster != null && caster.Downed) 
            {
                this.parent.Destroy();
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref caster, "caster");
        }
    }
}
