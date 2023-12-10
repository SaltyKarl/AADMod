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
        public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
        {
            base.Cast(targets, ability);
            var target = targets[0];
            var cellToTeleport = GetCellToTeleport(ability, target);
            var portalEffecter = AAD_DefOf.AAD_TeleportEffect.Spawn(cellToTeleport, ability.pawn.Map, new Vector3(0, 3, 0));
            ability.AddEffecterToMaintain(portalEffecter, cellToTeleport, 180, ability.pawn.Map);
            if (teleportCaster)
            {
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
                foreach (var thing in GetThingsToTeleport())
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
                    GenPlace.TryPlaceThing(thing, cellToTeleport, ability.pawn.Map, ThingPlaceMode.Direct);
                }
            }
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
    }
}
