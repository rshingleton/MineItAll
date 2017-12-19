using System;
using System.Reflection;
using RimWorld;
using Verse;
using Harmony;
using HugsLib;
using HugsLib.Settings;
using HugsLib.Utils;
using System.Collections.Generic;

namespace MineItAll
{
    public class Bootstrapper : ModBase
    {
        private const string ModId = "MineItAll";
        internal static HarmonyInstance HarmonyInstance { get; set; }

        private static Bootstrapper Instance { get; set; }

        private Bootstrapper()
        {
            Instance = this;
        }

        // we do our injections at world load because some mods overwrite ThingDesignatorDef.resolvedDesignators during init
        public override void WorldLoaded()
        {
            InjectDesignators();
        }

        private Designator_MineTool InstantiateDesignator(Type designatorType, MinerDesignatorDef designatorDef)
        {
            try
            {
                var designator =Activator.CreateInstance(designatorType, designatorDef);
                return (Designator_MineTool) designator;
            }
            catch (Exception e)
            {
                Logger.ReportException(e, null, false,
                    $"instantiation of {(designatorType != null ? designatorType.FullName : "(null)")} with Def {designatorDef}");
            }
            return null;
        }

        private void InjectDesignators()
        {
            var numDesignatorsInjected = 0;
            foreach (var designatorDef in DefDatabase<MinerDesignatorDef>.AllDefs)
            {
                if (designatorDef.Injected) continue;
                var resolvedDesignators = designatorDef.Category.AllResolvedDesignators;
                var insertIndex = -1;
                for (var i = 0; i < resolvedDesignators.Count; i++)
                {
                    if (resolvedDesignators[i].GetType() != designatorDef.insertAfter) continue;
                    insertIndex = i;
                    break;
                }
                if (insertIndex >= 0)
                {
                    var designator = InstantiateDesignator(designatorDef.designatorClass, designatorDef);
                    designator.icon = designatorDef.IconTex;

                    resolvedDesignators.Insert(insertIndex + 1, designator);
                    numDesignatorsInjected++;
                }
                else
                {
                    Logger.Error($"Failed to inject {designatorDef.defName} after {designatorDef.insertAfter.Name}");
                }
                designatorDef.Injected = true;
            }
            if (numDesignatorsInjected > 0)
            {
                Logger.Trace("Injected " + numDesignatorsInjected + " designators");
            }
        }

        private class DesignatorEntry
        {
            private readonly Designator_MineTool designator;
            private readonly KeyBindingDef key;
            public DesignatorEntry(Designator_MineTool designator, KeyBindingDef key)
            {
                this.designator = designator;
                this.key = key;
            }
        }

        public override string ModIdentifier => ModId;

        private static ModLogger staticLogger;

        private new static ModLogger Logger => staticLogger ?? (staticLogger = new ModLogger(ModId));

        protected override bool HarmonyAutoPatch => false;
    }
}