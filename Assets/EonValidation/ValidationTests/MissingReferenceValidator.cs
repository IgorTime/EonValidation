using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class MissingReferenceValidator
    {
        public static List<ValidationIssue> ValidateGameObject(GameObject target, Object context = null)
        {
            if (!target)
            {
                return new List<ValidationIssue>();
            }
            
            var result = new List<ValidationIssue>();
            context ??= target;
            foreach (var child in target.IterateChildrenRecursively())
            {
                foreach (var component in child.GetComponents<Component>())
                {
                    if (!component)
                    {
                        continue;
                    }

                    var issues = MissingReferenceFinder.FindMissingReferences(component, context);
                    result.AddRange(issues);
                }
            }

            return result;
        }

        public static List<ValidationIssue> ValidateScriptableObject(ScriptableObject scriptableObject)
        {
            return MissingReferenceFinder.FindMissingReferences(scriptableObject);
        }
    }
}