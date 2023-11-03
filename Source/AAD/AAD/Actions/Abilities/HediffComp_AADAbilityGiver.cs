using System.Collections.Generic;
using Verse;

namespace AAD;

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
