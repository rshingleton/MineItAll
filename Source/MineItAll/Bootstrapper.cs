using System;
using System.Reflection;
using MineItAll.Injectors;
using RimWorld;
using Verse;

namespace MineItAll
{
    [StaticConstructorOnStartup]
    public class Bootstrapper
    {
        public static BindingFlags bindingFlags = GenGeneric.BindingFlagsAll;

        static Bootstrapper()
        {
            var designatorInjector = new DesignatorInjector();
            if (!injectStripMiner(designatorInjector))
            {
                Log.Error("Failed to inject Designator_StripMiner");
            }
            if (!injectMineBrush(designatorInjector))
            {
                Log.Error("Failed to inject Designator_MineBrush");
            }
            if (!injectVeinMiner(designatorInjector))
            {
                Log.Error("Failed to inject Designator_VeinMiner");
            }
            Log.Message("MineItAll injected.");
        }

        private static bool injectVeinMiner(DesignatorInjector injector)
        {
            return injector.Inject(typeof(Designator_VeinMiner), "Orders", typeof(Designator_Mine));
        }
        private static bool injectMineBrush(DesignatorInjector injector)
        {
            return injector.Inject(typeof(Designator_MineBrush), "Orders", typeof(Designator_Mine));
        }
        private static bool injectStripMiner(DesignatorInjector injector)
        {
            return injector.Inject(typeof(Designator_StripMiner), "Orders", typeof(Designator_Mine));
        }

    }
}