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
            if (parent.Map is null)
            {
                yield break;
            }

            int cellCount = GenRadial.NumCellsInRadius(Props.radius);
            for (int i = 0; i < cellCount; i++)
            {
                IntVec3 cell = parent.Position + GenRadial.RadialPattern[i];
                List<Thing> thingList = parent.Map.thingGrid.ThingsListAt(cell);
                foreach (Thing thing in thingList)
                {
                    if (thing.Faction != parent.Faction)
                    {
                        continue;
                    }

                    CompShield shield = thing.TryGetComp<CompShield>();
                    if (shield is not null)
                    {
                        yield return shield;
                    }
                    else if (thing is Pawn { apparel.WornApparel: not null } pawn)
                    {
                        foreach (Apparel apparel in pawn.apparel.WornApparel)
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

    public override void CompTick()
    {
        foreach (CompShield shield in Shields)
        {
            shield.energy = Mathf.Clamp(shield.energy + Props.energyChargeRate, 0, shield.EnergyMax);
        }
    }

    public override void PostDrawExtraSelectionOverlays()
    {
        base.PostDrawExtraSelectionOverlays();
        GenDraw.DrawFieldEdges(
            GenRadial.RadialCellsAround(
                parent.Position,
                Props.radius,
                true
            ).ToList()
        );
    }
}
