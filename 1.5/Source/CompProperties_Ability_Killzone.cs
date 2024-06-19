using RimWorld;
using Verse;

namespace AADMod
{
    public class CompProperties_Ability_Killzone : CompProperties_AbilityEffect
    {
        public float InitialRadius = 0f;

        public float ZoneRadius = 6f;

        public ThingDef SpawnThingDef;

        public ThingDef MoteDef;

        public FleckDef beamGroundFleckDef;

        public EffecterDef beamEndEffecterDef;

        public int LaserNum = 4;

        public float Speed;

        public int StunDuration;

        public CompProperties_Ability_Killzone()
        {
            compClass = typeof(Comp_AbilityEffect_Killzone);
        }
    }
}
