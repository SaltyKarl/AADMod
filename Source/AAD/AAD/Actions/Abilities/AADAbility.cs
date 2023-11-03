using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AAD;

public class AADAbility
{
    private AADAbilityDef _def;

    public TargetingParameters TargetParams;

    public virtual void Execute(Pawn pawn, TargetInfo? target)
    {
        
    }
}
