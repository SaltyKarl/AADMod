using System.Diagnostics.CodeAnalysis;
using RimWorld;
using Verse;

namespace AADMod;

[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class CompProperties_Ability_Killzone : CompProperties_AbilityEffect
{
    public EffecterDef beamEndEffecterDef;
    public FleckDef beamGroundFleckDef;
    public float InitialRadius = 0f;
    public int LaserNum = 4;
    public ThingDef MoteDef;
    public ThingDef SpawnThingDef;
    public float Speed;
    public int StunDuration;
    public float ZoneRadius = 6f;

    public CompProperties_Ability_Killzone()
    {
        compClass = typeof(Comp_AbilityEffect_Killzone);
    }
}
