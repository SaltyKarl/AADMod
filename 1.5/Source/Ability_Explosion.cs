using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace AADMod
{
    [HotSwappable]
    public class ExplosionExtension : AbilityExtension_AbilityMod
    {
        public DamageDef damageDef;
        public float arc;
    }

    public class Ability_Explosion : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var cell = targets[0].Cell;
            var explosionExtension = def.GetModExtension<ExplosionExtension>();
            var caster = pawn;
            IntVec3 position = caster.Position;
            var distance = caster.Position.DistanceTo(cell);
            float num = Mathf.Atan2(-(cell.z - position.z), cell.x - position.x) * 57.29578f;
            GenExplosion.DoExplosion(affectedAngle: new FloatRange(num - explosionExtension.arc, 
                num + explosionExtension.arc), center: position, 
                map: caster.MapHeld, radius: distance + 1, damType: explosionExtension.damageDef, 
                instigator: caster, doVisualEffects: false);
            var explosion = position.GetFirstThing<Explosion>(caster.MapHeld);
            explosion.doVisualEffects = true;
        }
    }
}
