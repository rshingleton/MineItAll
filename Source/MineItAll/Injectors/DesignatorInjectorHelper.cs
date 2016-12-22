using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace MineItAll.Injectors
{
    public static class DesignatorInjectorHelper
    {
        public static FieldInfo _resolvedDesignatorsField = typeof(DesignationCategoryDef).GetField(
            "resolvedDesignators", BindingFlags.NonPublic | BindingFlags.Instance);

        public static List<Designator> _resolvedDesignators(this DesignationCategoryDef category)
        {
            return _resolvedDesignatorsField.GetValue(category) as List<Designator>;
        }
    }
}