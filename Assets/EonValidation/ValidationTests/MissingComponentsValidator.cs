using System.Collections.Generic;
using System.Text;
using EonValidation.Runtime;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class MissingComponentsValidator
    {
        private static readonly StringBuilder stringBuilder = new();
        
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

                    var path = GetPath(child);
                    result.Add(new ValidationIssue
                    {
                        Message = "Missing component.",
                        HierarchyPath = path,
                        Context = context
                    });
                    break;
                }
            }

            return result;
        }
        
        private static string GetPath(Transform target)
        {
            stringBuilder.Clear();
            stringBuilder.Append(target.name);
            target = target.parent;
            while (target)
            {
                stringBuilder.Insert(0, target.name + "/");
                target = target.parent;
            }

            return stringBuilder.ToString();
        }
        
       
    }
}