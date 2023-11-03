using System;
using RimWorld;
using Verse;

namespace AAD;

public class AADAbilityDef : Def
{
    private AADAbility _abilityInt;
    
    public Type? workerClass;
    public int cooldownTicks;

    public bool targetGround;
    public bool targetPawns;

    public AADAbility Ability
    {
        get
        {
            if (workerClass == null) return null;
            return _abilityInt ??= (AADAbility)Activator.CreateInstance(workerClass);
        }
    }
}
