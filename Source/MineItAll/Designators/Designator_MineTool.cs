using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MineItAll
{
    public abstract class Designator_MineTool : Designator
    {
        internal readonly MinerDesignatorDef def;
        protected int numThingsDesignated;

        protected Designator_MineTool(MinerDesignatorDef def)
        {
            this.def = def;
            defaultLabel = def.label;
            defaultDesc = def.description;
            icon = def.IconTex;
            useMouseIcon = true;
            soundDragSustain = SoundDefOf.DesignateDragStandard;
            soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
            soundSucceeded = def.soundSucceeded;
            hotKey = def.hotkeyDef;
        }

        public override int DraggableDimensions => 2;

        public override bool DragDrawMeasurements => true;

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(this.Map))
                return (AcceptanceReport) false;
            if (this.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null)
                return AcceptanceReport.WasRejected;
            if (c.Fogged(this.Map))
                return (AcceptanceReport) true;
            var firstMineable = c.GetFirstMineable(this.Map);
            if (firstMineable == null)
                return (AcceptanceReport) "MessageMustDesignateMineable".Translate();
            var acceptanceReport = this.CanDesignateThing((Thing) firstMineable);
            return !acceptanceReport.Accepted ? acceptanceReport : AcceptanceReport.WasAccepted;
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            if (!t.def.mineable)
                return (AcceptanceReport) false;
            if (this.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
                return AcceptanceReport.WasRejected;
            return (AcceptanceReport) true;
        }

        public override void DesignateSingleCell(IntVec3 loc)
        {
            this.Map.designationManager.AddDesignation(new Designation((LocalTargetInfo) loc, DesignationDefOf.Mine));
        }

        public override void DesignateThing(Thing t)
        {
            this.DesignateSingleCell(t.Position);
        }

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
        }

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
        }
    }
}