using RimWorld;
using UnityEngine;
using Verse;

namespace MineItAll
{
    public class Designator_StripMiner : Designator_MineTool
    {
        private IntVec3 startPoint;

        private int spacing = 4;

        private int spacingY = 4;

        public override int DraggableDimensions
        {
            get { return 2; }
        }

        public Designator_StripMiner(MinerDesignatorDef def) : base(def)
        {
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
            var shifted = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (!shifted && Input.GetKeyDown(KeyCode.Q))
            {
                this.spacingY++;
            }
            else if (!shifted && Input.GetKeyDown(KeyCode.E))
            {
                this.spacing++;
            }
            else if ((shifted && Input.GetKeyDown(KeyCode.E)) && this.spacing > 4)
            {
                this.spacing--;
            }
            else if ((shifted && Input.GetKeyDown(KeyCode.Q)) && this.spacingY > 4)
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