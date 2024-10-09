using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class MissingComponentsValidator
    {
        public static List<ValidationIssue> ValidateGameObject(GameObject target, Object context = null)
        {
            var result = new List<ValidationIssue>();
            context ??= target;
            foreach (var child in target.IterateChildrenRecursively())
            {
                foreach (var component in child.GetComponents<Component>())
                {
                    if (component)
                    {
                        continue;
                    }

                    var path = child.GetHierarchyPath();
                    result.Add(new ValidationIssue
                    {
                        Message = "Missing component",
                        HierarchyPath = path,
                        Context = context,
                    });

                    break;
                }
            }

            return result;
        }
    }
}