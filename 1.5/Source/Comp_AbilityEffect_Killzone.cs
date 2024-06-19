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

        public List<Vector3> beamPositions = [];
        public List<float> beamAngles = [];  // Track the angles for each beam
        public List<float> beamOffsets = []; // Track the offsets for each beam

        public List<Effecter> effecters = [];

        public List<MoteDualAttached> motes = [];

        private new CompProperties_Ability_Killzone Props => (CompProperties_Ability_Killzone)props;

        private Pawn Pawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            beamPositions.Clear();
            beamAngles.Clear();
            beamOffsets.Clear();
            effecters.Clear();
            motes.Clear();

            Pawn.stances.stunner.StunFor(Props.StunDuration, Pawn);

            var initialRadius = Props.InitialRadius;
            var beamEndEffecterDef = Props.beamEndEffecterDef;
            var moteDef = Props.MoteDef;
            var laserNum = Props.LaserNum;

            var map = Pawn.Map;
            var position = Pawn.Position;

            var targetInfo = new TargetInfo(position, map);
            var offset = new Vector3(0.5f, 0f, 0.5f);

            for (int i = 0; i < laserNum; i++)
            {
                var angle = i * (360f / laserNum);
                var startPos = VectorHelper.GetVector3_By_AngleFlat(position.ToVector3Shifted(), initialRadius, angle);

                beamPositions.Add(startPos);
                beamAngles.Add(0);  // Initial angle for each beam
                beamOffsets.Add(angle); // Offset each beam by an equal angle

                motes.Add(MoteMaker.MakeInteractionOverlay(moteDef, new TargetInfo(startPos.ToIntVec3(), map), targetInfo));
                if (beamEndEffecterDef != null) effecters.Add(beamEndEffecterDef.Spawn(startPos.ToIntVec3(), map, offset));
            }

            beamsActive = true;

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
            
            var maxDistance = Props.ZoneRadius;
            var beamEndEffecterDef = Props.beamEndEffecterDef;
            var beamGroundFleckDef = Props.beamGroundFleckDef;
            var spawnThingDef = Props.SpawnThingDef;
            var faction = Pawn.Faction;
            var speed = Props.Speed;

            var map = Pawn.Map;
            var position = Pawn.Position;
            var positionVec = position.ToVector3Shifted();
            var pawnTargetInfo = new TargetInfo(position, map);

            var allBeamsDone = true;

            for (int i = 0; i < motes.Count; i++)
            {
                // Update the angle for the spiral effect
                beamAngles[i] += speed; // Adjust speed here for slower spiral

                // Calculate the new position for the beam
                var distance = Mathf.Min((beamAngles[i] / 360f), 1f) * maxDistance;
                var angle = beamAngles[i] + beamOffsets[i]; // Apply the offset to the angle
                beamPositions[i] = VectorHelper.GetVector3_By_AngleFlat(positionVec, distance, angle);

                var beamTargetInt = beamPositions[i].ToIntVec3();
                var beamTargetInfo = new TargetInfo(beamTargetInt, map);
                var offset = beamPositions[i] - beamTargetInt.ToVector3Shifted();

                motes[i].UpdateTargets(beamTargetInfo, pawnTargetInfo, offset, Vector3.zero);
                motes[i].Maintain();

                if (beamEndEffecterDef != null)
                {
                    effecters[i].offset = offset;
                    effecters[i].EffectTick(beamTargetInfo, TargetInfo.Invalid);
                    effecters[i].ticksLeft--;
                }

                if (beamGroundFleckDef != null && Rand.Chance(0.32f))
                {
                    FleckMaker.Static(beamPositions[i] + offset, map, beamGroundFleckDef);
                }

                if (spawnThingDef != null && GenClosest.ClosestThing_Global(beamTargetInt, hostilePawns, range) != null)
                {
                    GenSpawn.Spawn(spawnThingDef, beamTargetInt, map).SetFaction(faction);
                }

                if (distance < maxDistance)
                {
                    allBeamsDone = false;
                }
            }

            beamsActive = !allBeamsDone;
        }
    }
}
