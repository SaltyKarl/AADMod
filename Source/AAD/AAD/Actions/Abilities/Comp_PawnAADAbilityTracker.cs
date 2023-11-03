using System.Collections.Generic;
using Verse;

namespace AAD;

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

public class CompProperties_PawnAADAbilityTracker : CompProperties
{
    public List<AADAbilityDef> defaultAbilities;
    
    public CompProperties_PawnAADAbilityTracker()
    {
        compClass = typeof(Comp_PawnAADAbilityTracker);
    }
}