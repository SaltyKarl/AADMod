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
                AdjustPos(this.maintainedEffecters[0].first, -0.01f);
                if (this.maintainedEffecters.Count > 1)
                {
                    AdjustPos(this.maintainedEffecters[1].first, 0.01f);
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
