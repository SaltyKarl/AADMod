using System.Linq;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace AADMod;

public class Ability_Portal : Ability
{
    public override void Tick()
    {
        base.Tick();
        if (!maintainedEffecters.Any())
        {
            return;
        }

        PortalExtension extension = def.GetModExtension<PortalExtension>();
        if (extension.teleportCaster)
        {
            AdjustPos(maintainedEffecters[0].first, -0.01f);
            AdjustPos(maintainedEffecters[1].first, 0.01f);
            return;
        }

        foreach (Pair<Effecter, TargetInfo> item in maintainedEffecters)
        {
            AdjustPos(item.first, -0.01f);
        }
    }

    public static void AdjustPos(Effecter effecter, float offset)
    {
        foreach (SubEffecter_Sprayer sub in effecter.children.OfType<SubEffecter_Sprayer>())
        {
            if (sub.mote != null)
            {
                sub.mote.yOffset += offset;
            }
        }
    }
}
