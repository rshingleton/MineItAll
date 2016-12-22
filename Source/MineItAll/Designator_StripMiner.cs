using RimWorld;
using UnityEngine;
using Verse;

namespace MineItAll
{
    public class Designator_StripMiner : Designator_Mine
    {
        private IntVec3 startPoint;

        private int spacing = 4;

        private int spacingY = 4;

        public override int DraggableDimensions
        {
            get
            {
                return 2;
            }
        }

        public Designator_StripMiner()
        {
            this.defaultLabel = "Strip Miner";
            this.icon = ContentFinder<Texture2D>.Get("StripMine", true);
            this.defaultDesc = "Drag an area to strip mine. Use 8,4,5,6 on your !NUMBLOCK! to change spacing between strips.";
            this.useMouseIcon = true;
            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
            this.soundSucceeded = SoundDefOf.DesignateMine;
            this.tutorTag = "DesignatorMine";
        }

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                this.startPoint = UI.MouseCell();
            }
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            IntVec3 intVec = c - this.startPoint;
            if ((intVec.x % this.spacing == 0) || (intVec.z % this.spacingY == 0))
            {
                return base.CanDesignateCell(c);
            }
            else
            {
                return AcceptanceReport.WasRejected;
            }
        }

        public override void DrawMouseAttachments()
        {
            base.DrawMouseAttachments();
            if (Input.GetKeyUp((KeyCode)264))
            {
                this.spacingY++;
            }
            if (Input.GetKeyUp((KeyCode)262))
            {
                this.spacing++;
            }
            if (Input.GetKeyUp((KeyCode)260) && this.spacing > 4)
            {
                this.spacing--;
            }
            if (Input.GetKeyUp((KeyCode)261) && this.spacingY > 4)
            {
                this.spacingY--;
            }
            //Log.Message(this.spacing + ", " + this.spacingY);
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            this.spacing = 4;
            this.spacingY = 4;
        }
    }
}
