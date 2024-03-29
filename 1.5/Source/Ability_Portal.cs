using System.Linq;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace AADMod
{
    public class Ability_Portal : Ability
    {
        public override void Tick()
        {
            base.Tick();
            if (this.maintainedEffecters.Any())
            {
                var extension = this.def.GetModExtension<PortalExtension>();
                if (extension.teleportCaster)
                {
                    AdjustPos(this.maintainedEffecters[0].first, -0.01f);
                    AdjustPos(this.maintainedEffecters[1].first, 0.01f);
                }
                else
                {
                    foreach (var item in this.maintainedEffecters)
                    {
                        AdjustPos(item.first, -0.01f);
                    }
                }
            }
        }

        private void AdjustPos(Effecter effecter, float offset)
        {
            foreach (var sub in effecter.children.OfType<SubEffecter_Sprayer>())
            {
                if (sub?.mote != null)
                {
                    sub.mote.yOffset += offset;
                }
            }
        }
    }
}
