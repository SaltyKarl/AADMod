using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AADMod
{
    public class Hediff_MechControllable : HediffWithComps
    {
        public override IEnumerable<Gizmo> GetGizmos()
        {
            Command_Toggle command_Toggle = new Command_Toggle
            {
                hotKey = KeyBindingDefOf.Command_ColonistDraft,
                isActive = () => pawn.Drafted,
                toggleAction = delegate
                {
                    pawn.drafter.Drafted = !pawn.drafter.Drafted;
                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
                    if (pawn.Drafted)
                    {
                        LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
                    }
                },
                defaultDesc = "CommandToggleDraftDesc".Translate(),
                icon = (Texture)(object)TexCommand.Draft,
                turnOnSound = SoundDefOf.DraftOn,
                turnOffSound = SoundDefOf.DraftOff,
                groupKey = 81729172,
                defaultLabel = (pawn.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate()
            };
            if (pawn.Downed)
            {
                command_Toggle.Disable("IsIncapped".Translate(pawn.LabelShort, this));
            }
            if (!pawn.Drafted)
            {
                command_Toggle.tutorTag = "Draft";
            }
            else
            {
                command_Toggle.tutorTag = "Undraft";
            }
            yield return command_Toggle;
            if (pawn.Drafted && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
            {
                yield return new Command_Toggle
                {
                    hotKey = KeyBindingDefOf.Misc6,
                    isActive = () => pawn.drafter.FireAtWill,
                    toggleAction = delegate
                    {
                        pawn.drafter.FireAtWill = !pawn.drafter.FireAtWill;
                    },
                    icon = (Texture)(object)TexCommand.FireAtWill,
                    defaultLabel = "CommandFireAtWillLabel".Translate(),
                    defaultDesc = "CommandFireAtWillDesc".Translate(),
                    tutorTag = "FireAtWillToggle"
                };
            }
        }
        public override void PostRemoved()
        {
            base.PostRemoved();
            if (pawn.MapHeld != null)
            {
                AAD_DefOf.AAD_TeleportEffect.Spawn(pawn.PositionHeld, pawn.MapHeld, new Vector3(0, 3, 0));
            }
            Thing thing = (Thing)pawn.Corpse ?? pawn;
            if (thing.Destroyed is false)
            {
                thing.Destroy();
            }
        }
    }
}
