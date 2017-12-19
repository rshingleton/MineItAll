using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MineItAll
{
    public class Designator_VeinMiner : Designator_MineTool
    {
        public override int DraggableDimensions
        {
            get { return 0; }
        }

        public Designator_VeinMiner(MinerDesignatorDef def) : base(def)
        {
//            this.defaultLabel = "Vein Miner";
//            this.defaultDesc = "Click on a visible ore and you will mine the whole vein.";
//            this.useMouseIcon = true;
//            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
//            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
//            this.soundSucceeded = SoundDefOf.DesignateMine;
//            this.hotKey = KeyBindingDefOf.Misc10;
//            this.tutorTag = "DesignatorMine";
        }


        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!GenGrid.InBounds(c, Find.VisibleMap))
            {
                return false;
            }

            if (Find.VisibleMap.designationManager.DesignationAt(c, DesignationDefOf.Mine) == null)
            {
                foreach (Thing current in Find.VisibleMap.thingGrid.ThingsAt(c))
                {
                    if (!this.isOre(current.def) || GridsUtility.Fogged(c, Find.VisibleMap))
                    {
                        return "Must designate mineable and accessable ore!";
                    }
                }
                return AcceptanceReport.WasAccepted;
            }
            return AcceptanceReport.WasAccepted;
        }

        public override void DesignateSingleCell(IntVec3 loc)
        {
            foreach (Thing current in Find.VisibleMap.thingGrid.ThingsAt(loc))
            {
                if (this.isOre(current.def))
                {
                    var designator_Mine = new Designator_Mine();
                    List<IntVec3> veinCells = this.getVeinCells(loc, current.def);
                    designator_Mine.DesignateMultiCell(veinCells);
                }
            }
        }

        private List<IntVec3> getVeinCells(IntVec3 at, ThingDef oreType)
        {
            List<IntVec3> list = new List<IntVec3>();
            list.Add(at);
            List<IntVec3> list2 = new List<IntVec3>();
            list2.Add(at);
            List<IntVec3> list3 = new List<IntVec3>();
            IntVec3 item = default(IntVec3);
            while (!GenList.NullOrEmpty<IntVec3>(list2))
            {
                foreach (IntVec3 current in list2)
                {
                    foreach (IntVec3 current2 in GenAdjFast.AdjacentCells8Way(current))
                    {
                        if (!GenList.NullOrEmpty<Thing>(Find.VisibleMap.thingGrid.ThingsListAt(current2)))
                        {
                            foreach (Thing current3 in Find.VisibleMap.thingGrid.ThingsListAt(current2))
                            {
                                if (current3.def.Equals(oreType) && !list.Contains(current2))
                                {
                                    list.Add(current2);
                                    list3.Add(current2);
                                }
                            }
                        }
                    }
                    item = current;
                }
                list2.Remove(item);
                List<IntVec3> list4 = list2.Concat(list3).ToList<IntVec3>();
                list2 = list4;
                list3.Clear();
            }
            return list;
        }

        public bool isOre(ThingDef def)
        {
            return def != null && def.building != null && def.building.isResourceRock;
        }
    }
}