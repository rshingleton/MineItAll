using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace MineItAll.Injectors
{
    public class DesignatorInjector
    {

        public bool Inject(Type designatorClass, String designatorCategory, Type designatorNextTo)
        {
            if (designatorClass == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(designatorCategory))
            {
                // Get the category
                var designationCategory = DefDatabase<DesignationCategoryDef>.GetNamed(designatorCategory, false);

                // First instatiate and inject the designator into the list of resolved designators
                // Create the new designator
                var designatorObject = (Designator) Activator.CreateInstance(designatorClass);
                if (designatorObject == null)
                {
                    Log.Message(string.Format("Unable to create instance of '{0}'", designatorClass));
                    return false;
                }

                if (designatorNextTo == null)
                {
                    // Inject the designator
                    designationCategory._resolvedDesignators().Add(designatorObject);
                }
                else
                {
                    // Prefers to be beside a specific designator
                    var designatorIndex = designationCategory._resolvedDesignators()
                        .FindIndex(d => (
                            (d.GetType() == designatorNextTo)
                        ));

                    if (designatorIndex < 0)
                    {
                        // Other designator doesn't exist (yet?)
                        // Inject the designator at the end
                        designationCategory._resolvedDesignators().Add(designatorObject);
                    }
                    else
                    {
                        // Inject beside desired designator
                        designationCategory._resolvedDesignators()
                            .Insert(designatorIndex + 1, designatorObject);
                    }
                }


                // Now inject the designator class into the list of classes as a saftey net for another mod resolving the category
                if (!designationCategory.specialDesignatorClasses.Exists(s => s == designatorClass))
                {
                    if (designatorNextTo == null)
                    {
                        // Inject the designator class at the end of the list
                        designationCategory.specialDesignatorClasses.Add(designatorClass);
                    }
                    else
                    {
                        // Prefers to be beside a specific designator
                        var designatorIndex =
                            designationCategory.specialDesignatorClasses.FindIndex(
                                s => s == designatorNextTo);

                        if (designatorIndex < 0)
                        {
                            // Can't find desired designator class
                            // Inject the designator at the end
                            designationCategory.specialDesignatorClasses.Add(designatorClass);
                        }
                        else
                        {
                            // Inject beside desired designator class
                            designationCategory.specialDesignatorClasses.Insert(designatorIndex + 1,
                                designatorClass);
                        }
                    }
                }
            }

            return true;
        }

    }

}