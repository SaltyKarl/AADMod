using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AADMod
{
    public class Comp_AbilityEffect_Killzone : CompAbilityEffect
    {
        public bool beamsActive;

        public IntVec3 center;

        public List<Vector3> beamTargets = [];

        public List<Vector3> beamDestinations = [];

        public List<Effecter> effecters = [];

        public List<MoteDualAttached> motes = [];

        private new CompProperties_Ability_Killzone Props => (CompProperties_Ability_Killzone)props;

        private Pawn Pawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            beamsActive = false;
            center = target.Cell;
            beamTargets.Clear();
            effecters.Clear();
            motes.Clear();

            var laserNum = Props.LaserNum;
            var zoneRadius = Props.ZoneRadius;
            var centerVector = target.CenterVector3;
            var map = Pawn.Map;

            for (int i = 0; i < laserNum; i++)
            {
                float angle = Rand.Range(0f, 360f);

                var beamTarget = VectorHelper.GetVector3_By_AngleFlat(centerVector, zoneRadius, angle);
                var beamDest = VectorHelper.GetVector3_By_AngleFlat(centerVector, zoneRadius, angle - 180f);
                var beamTargetInt = beamTarget.ToIntVec3();

                beamTargets.Add(beamTarget);
                beamDestinations.Add(beamDest);

                MoteDualAttached item = MoteMaker.MakeInteractionOverlay(Props.MoteDef, new TargetInfo(beamTargetInt, map), new TargetInfo(center, map));
                motes.Add(item);

                if (Props.beamEndEffecterDef != null)
                {
                    Effecter item2 = Props.beamEndEffecterDef.Spawn(beamTargetInt, map, new Vector3(0.5f, 0f, 0.5f));
                    effecters.Add(item2);
                }
            }

            beamsActive = true;
            Pawn.stances.stunner.StunFor(Props.StunDuration, Pawn);
            base.Apply(target, dest);
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target.Thing is Pawn pawn && pawn?.TargetCurrentlyAimingAt == Pawn;
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            base.DrawEffectPreview(target);
            GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
            GenDraw.DrawRadiusRing(target.Cell, Props.ZoneRadius, Verb_CastAbility.RadiusHighlightColor);
        }

        public override void CompTick()
        {
            if (!beamsActive) return;

            var range = AAD_DefOf.AAD_APMine.GetCompProperties<CompProperties_Explosive>().explosiveRadius;
            var hostilePawns = Pawn.Map.mapPawns.AllPawnsSpawned.Where(p => p.HostileTo(Pawn));
            var beamEndEffecterDef = Props.beamEndEffecterDef;
            var beamGroundFleckDef = Props.beamGroundFleckDef;
            var spawnThingDef = Props.SpawnThingDef;
            var spawnChance = Props.SpawnChance;
            var faction = Pawn.Faction;
            var speed = Props.Speed;
            var map = Pawn.Map;

            for (int i = 0; i < motes.Count; i++)
            {
                var target = beamTargets[i];
                var targetInt = target.ToIntVec3();
                var offset = target - targetInt.ToVector3Shifted();

                motes[i].UpdateTargets(new TargetInfo(targetInt, map), new TargetInfo(center, map), offset, Vector3.zero);
                motes[i].Maintain();

                if (beamEndEffecterDef != null)
                {
                    effecters[i].offset = offset;
                    effecters[i].EffectTick(new TargetInfo(targetInt, map), TargetInfo.Invalid);
                    effecters[i].ticksLeft--;
                }

                if (beamGroundFleckDef != null && Rand.Chance(0.32f))
                {
                    FleckMaker.Static(target + offset, map, beamGroundFleckDef);
                }

                if (spawnThingDef != null && Rand.Chance(spawnChance))
                {
                    // if we have a hostile pawn in range, spawn the thing
                    // do the heavy check only if we have a chance to spawn
                    if (GenClosest.ClosestThing_Global(targetInt, hostilePawns, range) != null)
                    {
                        GenSpawn.Spawn(spawnThingDef, targetInt, map).SetFaction(faction);
                    }
                }

                beamTargets[i] = Vector3.MoveTowards(beamTargets[i], beamDestinations[i], speed);
                if ((beamTargets[i] - beamDestinations[i]).MagnitudeHorizontal() <= 0.01f)
                {
                    beamsActive = false;
                }
            }
        }
    }
}