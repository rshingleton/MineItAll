using System;
using UnityEngine;
using Verse;

namespace MineItAll
{
    public class MinerDesignatorDef : Def
    {
        public Type designatorClass;
        public string category;
        public Type insertAfter = null;
        public string iconTex;
        public string highlightTex;
        public SoundDef soundSucceeded = null;
        public KeyBindingDef hotkeyDef = null;
        public string messageSuccess = null;
        public string messageFailure = null;

        public bool Injected { get; set; }

        private Texture2D resolvedIconTex;
        public Texture2D IconTex
        {
            get { return resolvedIconTex; }
        }

        private Material resolvedHighlightTex;
        public Material HighlightTex
        {
            get { return resolvedHighlightTex; }
        }

        private DesignationCategoryDef resolvedCategory;
        public DesignationCategoryDef Category
        {
            get { return resolvedCategory; }
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            resolvedCategory = DefDatabase<DesignationCategoryDef>.GetNamed(category);
            // load textures in main thread
            LongEventHandler.ExecuteWhenFinished(() => {
                resolvedIconTex = ContentFinder<Texture2D>.Get(iconTex);
                resolvedHighlightTex = MaterialPool.MatFrom(highlightTex);
            });
        }

        public override void PostLoad()
        {
            Assert(designatorClass != null, "designatorClass field must be set");
            Assert(category != null, "category field must be set");
            Assert(insertAfter != null, "insertAfter field must be set");
            Assert(iconTex != null, "icon texture must be set");
        }

        private void Assert(bool check, string errorMessage)
        {
            if (!check) Log.Error(string.Format("[AllowTool] Invalid data in MinerDesignatorDef {0}: {1}", defName, errorMessage));
        }
    }
}
