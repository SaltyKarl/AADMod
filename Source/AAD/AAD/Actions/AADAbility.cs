using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AAD.Actions;

public class AADAbility
{
    private AADAbilityDef _def;

    public TargetingParameters TargetParams;

    public virtual void Execute(Pawn pawn, TargetInfo? target)
    {
        
    }
}

public class AADAbilityDef : Def
{
    public Type workerClass;
    public bool usesTarget;
}

public class AADAbility_Teleport : AADAbility
{
    
}

public class HediffComp_AADAbilityGiver : HediffComp
{
    private Comp_PawnAADAbilityTracker _abilityTracker;

    public HediffCompProperties_AADAbilityGiver Props => (HediffCompProperties_AADAbilityGiver) props;

    public override void CompPostMake()
    {
        base.CompPostMake();
        _abilityTracker = Pawn.GetComp<Comp_PawnAADAbilityTracker>();
        if (_abilityTracker != null)
        {
            foreach (var abilityDef in Props.abilities)
            {
                _abilityTracker.LearnAbility(abilityDef);
            }
        }
    }

    public override IEnumerable<Gizmo> CompGetGizmos()
    {
        return base.CompGetGizmos();
    }
}

public class HediffCompProperties_AADAbilityGiver : HediffCompProperties
{
    public List<AADAbilityDef> abilities;
    
    public HediffCompProperties_AADAbilityGiver()
    {
        compClass = typeof(HediffComp_AADAbilityGiver);
    }
    
}

public class CompProperties_PawnAADAbilityTracker : CompProperties
{
    public List<AADAbilityDef> defaultAbilities;
    
    public CompProperties_PawnAADAbilityTracker()
    {
        compClass = typeof(Comp_PawnAADAbilityTracker);
    }
}

public class Comp_PawnAADAbilityTracker : ThingComp
{
    private List<AADAbilityDef> _knownAbilities;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
    }

    public void LearnAbility(AADAbilityDef ability)
    {
        _knownAbilities ??= new List<AADAbilityDef>();
        if (_knownAbilities.Contains(ability))
        {
            return;
        }
        _knownAbilities.Add(ability);
    }
    
    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield break;
    }
}