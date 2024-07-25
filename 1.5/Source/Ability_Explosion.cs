using RimWorld.Planet;
using UnityEngine;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace AADMod;

public class Ability_Explosion : Ability
{
    public override void Cast(params GlobalTargetInfo[] targets)
    {
        base.Cast(targets);
        IntVec3 cell = targets[0].Cell;
        ExplosionExtension explosionExtension = def.GetModExtension<ExplosionExtension>();
        Pawn caster = pawn;
        IntVec3 position = caster.Position;
        float distance = caster.Position.DistanceTo(cell);
        float num = Mathf.Atan2(-(cell.z - position.z), cell.x - position.x) * 57.29578f;
        GenExplosion.DoExplosion(
            affectedAngle: new FloatRange(
                num - explosionExtension.arc,
                num + explosionExtension.arc
            ),
            center: position,
            map: caster.MapHeld,
            radius: distance + 1,
            damType: explosionExtension.damageDef,
            instigator: caster,
            doVisualEffects: false
        );
        Explosion explosion = position.GetFirstThing<Explosion>(caster.MapHeld);
        explosion.doVisualEffects = true;
    }
}
