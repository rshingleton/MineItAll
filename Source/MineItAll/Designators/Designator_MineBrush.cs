using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MineItAll
{
    public class Designator_MineBrush : Designator_MineTool
    {
        private float radius = 1f;
        private Material circleMat { get; set; }

        public override int DraggableDimensions => 0;

        public Designator_MineBrush(MinerDesignatorDef def) : base(def)
        {
//            this.defaultLabel = "Mine Brush";
//            this.defaultDesc = "A brush for mining. Use q and e to change size.";
//            this.useMouseIcon = true;
//            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
//            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
//            this.soundSucceeded = SoundDefOf.DesignateMine;
//            this.tutorTag = "DesignatorMine";
            Log.Message("Adding tex: " + def.highlightTex);
            this.circleMat = def.HighlightTex;
        }

        public override void DesignateSingleCell(IntVec3 loc)
        {
            var designator_Mine = new Designator_Mine();
            designator_Mine.DesignateMultiCell(this.designateAt(loc));
        }

        private List<IntVec3> designateAt(IntVec3 here)
        {
            return GenRadial.RadialCellsAround(here, this.radius, true).ToList<IntVec3>();
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            return AcceptanceReport.WasAccepted;
        }

        public override void DrawMouseAttachments()
        {
            base.DrawMouseAttachments();
            bool flag = Input.GetKeyUp((KeyCode) 113) && this.radius > 0f;
            if (flag)
            {
                this.radius -= 0.1f;
            }
            bool flag2 = Input.GetKeyUp((KeyCode) 101) && this.radius < 30f;
            if (flag2)
            {
                this.radius += 0.1f;
            }
        }

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            this.drawCircle(UI.MouseCell());
        }

        private void drawCircle(IntVec3 pos)
        {
            Log.Message("Draw Circle");
            foreach (IntVec3 current in GenRadial.RadialCellsAround(pos, this.radius, true))
            {
                Graphics.DrawMesh(MeshPool.plane10, current.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays),
                    Quaternion.identity, this.circleMat, 0);
            }
        }

        public override void ProcessInput(Event ev)
        {
            bool flag = Find.DesignatorManager.SelectedDesignator != null && this.radius == 7.9f;
            if (flag)
            {
                this.radius = DefDatabase<ThingDef>.GetNamed("SunLamp", true).specialDisplayRadius;
            }
            else
            {
                this.radius = 7.9f;
            }
            base.ProcessInput(ev);
        }
    }
}