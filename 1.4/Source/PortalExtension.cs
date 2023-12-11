using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using VFECore.Abilities;

namespace AADMod
{
    [HotSwappable]
    public class PortalExtension : AbilityExtension_AbilityMod
    {
        public ThingDef thingDef;
        public PawnKindDef pawnKind;
        public int count;
        public bool teleportCaster;
        public List<IntVec2> pattern;
        public int durationTicks;
        public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
        {
            base.Cast(targets, ability);
            var target = targets[0];
            var cellToTeleport = GetCellToTeleport(ability, target);
            if (teleportCaster)
            {
                var portalEffecter = AAD_DefOf.AAD_TeleportEffect.Spawn(cellToTeleport, ability.pawn.Map, new Vector3(0, 3, 0));
                ability.AddEffecterToMaintain(portalEffecter, cellToTeleport, 180, ability.pawn.Map);
                var portalEffecter2 = AAD_DefOf.AAD_TeleportEffect.Spawn(ability.pawn.Position, ability.pawn.Map, new Vector3(0, 3, 0));
                ability.AddEffecterToMaintain(portalEffecter2, ability.pawn.Position, 180, ability.pawn.Map);
                foreach (var sub in portalEffecter2.children.OfType<SubEffecter_Sprayer>())
                {
                    if (sub?.mote != null)
                    {
                        sub.mote.yOffset -= 6.2f;
                    }
                }
                ability.pawn.Position = cellToTeleport;
                ability.pawn.Notify_Teleported(true, true);
                if (target.Thing is Pawn pawnTarget && pawnTarget.HostileTo(ability.pawn))
                {
                    pawnTarget.stances?.stunner.StunFor(180, ability.pawn);
                }
            }
            else
            {
                if (pattern != null)
                {
                    var things = GetThingsToTeleport();
                    var cells = AffectedCells(cellToTeleport, ability.pawn.Map).ToList();
                    for (var i = 0; i < cells.Count; i++)
                    {
                        DoTeleport(ability, things[i], cells[i]);
                    }
                }
                else
                {
                    foreach (var thing in GetThingsToTeleport())
                    {
                        DoTeleport(ability, thing, cellToTeleport);
                    }
                }
            }
        }

        private void DoTeleport(VFECore.Abilities.Ability ability, Thing thing, IntVec3 cell)
        {
            cell = AdjustCell(ability, cell, thing);
            TeleportThing(ability, cell, thing);
            var portalEffecter = AAD_DefOf.AAD_TeleportEffect.Spawn(cell, ability.pawn.Map, new Vector3(0, 3, 0));
            ability.AddEffecterToMaintain(portalEffecter, cell, 180, ability.pawn.Map);
        }

        private void TeleportThing(VFECore.Abilities.Ability ability, IntVec3 cellToTeleport, Thing thing)
        {
            if (thing.def.CanHaveFaction)
            {
                thing.SetFaction(ability.pawn.Faction);
            }
            var comp = thing.TryGetComp<CompDestroyOnCasterDowned>();
            if (comp != null)
            {
                comp.caster = ability.pawn;
            }
            if (thing is Pawn pawn && durationTicks > 0)
            {
                var hediff = pawn.health.AddHediff(AAD_DefOf.AAD_MechControllable);
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = durationTicks;
                PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn);
            }

            GenPlace.TryPlaceThing(thing, cellToTeleport, ability.pawn.Map, ThingPlaceMode.Direct);
            var compExplosive = thing.TryGetComp<CompExplosive>();
            if (compExplosive != null)
            {
                compExplosive.StartWick(ability.pawn);
                compExplosive.wickTicksLeft = 60 * 90;
            }
        }

        private static IntVec3 AdjustCell(VFECore.Abilities.Ability ability, IntVec3 cellToTeleport, Thing thing)
        {
            if (thing is Pawn pawn)
            {
                if (cellToTeleport.WalkableBy(ability.pawn.Map, ability.pawn) is false || cellToTeleport.GetFirstPawn(ability.pawn.Map) is not null)
                {
                    RCellFinder.TryFindRandomCellNearWith(cellToTeleport, (IntVec3 x) => x.WalkableBy(ability.pawn.Map, ability.pawn)
                        && x.GetFirstPawn(ability.pawn.Map) is null, ability.pawn.Map, out cellToTeleport, startingSearchRadius: 1);
                }
            }
            else
            {
                if (cellToTeleport.GetFirstBuilding(ability.pawn.Map) is not null || cellToTeleport.GetFirstItem(ability.pawn.Map) is not null)
                {
                    RCellFinder.TryFindRandomCellNearWith(cellToTeleport, (IntVec3 x) => x.GetFirstBuilding(ability.pawn.Map) is null
                       && x.GetFirstItem(ability.pawn.Map) is null, ability.pawn.Map, out cellToTeleport, startingSearchRadius: 1);
                }
            }

            return cellToTeleport;
        }

        private List<Thing> GetThingsToTeleport()
        {
            List<Thing> things = new List<Thing>();
            if (pawnKind != null)
            {
                for (var i = 0; i < count; i++)
                {
                    var pawn = PawnGenerator.GeneratePawn(pawnKind);
                    things.Add(pawn);
                }
            }
            else
            {
                var countToMake = count;
                while (countToMake > 0)
                {
                    var curStack = Mathf.Min(thingDef.stackLimit, countToMake);
                    countToMake -= curStack;
                    var thing = ThingMaker.MakeThing(thingDef);
                    thing.stackCount = curStack;
                    things.Add(thing);
                }
            }
            return things;
        }

        private IntVec3 GetCellToTeleport(VFECore.Abilities.Ability ability, GlobalTargetInfo target)
        {
            if (target.HasThing)
            {
                var pos = target.Cell + target.Thing.Rotation.Opposite.FacingCell;
                if (pos.WalkableBy(ability.pawn.Map, ability.pawn))
                {
                    return pos;
                }
            }
            return target.Cell;
        }

        private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
        {
            foreach (IntVec2 item in pattern)
            {
                IntVec3 intVec = target.Cell + new IntVec3(item.x, 0, item.z);
                if (intVec.InBounds(map))
                {
                    yield return intVec;
                }
            }
        }
    }
}
