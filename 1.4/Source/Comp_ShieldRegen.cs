using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AADMod;

public class Comp_ShieldRegen : ThingComp
{
    public CompProperties_ShieldRegen Props => (CompProperties_ShieldRegen) props;

    public IEnumerable<CompShield> Shields
    {
        get
        {
            var cellCount = GenRadial.NumCellsInRadius(Props.radius);
            for (var i = 0; i < cellCount; i++)
            {
                var cell = parent.Position + GenRadial.RadialPattern[i];
                var thingList = parent.Map.thingGrid.ThingsListAt(cell);
                for (var t = 0; t < thingList.Count; t++)
                {
                    var thing = thingList[t];
                    var shield = thing.TryGetComp<CompShield>();
                    if (shield is not null)
                    {
                        yield return shield;
                    }
                    else if (thing is Pawn pawn)
                    {
                        if (pawn.apparel?.WornApparel != null)
                        {
                            foreach (var apparel in pawn.apparel.WornApparel)
                            {
                                shield = apparel.TryGetComp<CompShield>();
                                if (shield is not null)
                                {
                                    yield return shield;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public override void CompTick()
    {
        foreach (var shield in Shields)
        {
            shield.energy = Mathf.Clamp(shield.energy + Props.energyChargeRate, 0, shield.EnergyMax);
        }
    }

    public override void PostDrawExtraSelectionOverlays()
    {
        base.PostDrawExtraSelectionOverlays();
        GenDraw.DrawFieldEdges(GenRadial.RadialCellsAround(parent.Position, Props.radius, useCenter: true).ToList());
    }
}

public class CompProperties_ShieldRegen : CompProperties
{
    public float radius = 9.9f;
    public float energyChargeRate = 0.1f;
    
    public CompProperties_ShieldRegen()
    {
        compClass = typeof(Comp_ShieldRegen);
    }
}