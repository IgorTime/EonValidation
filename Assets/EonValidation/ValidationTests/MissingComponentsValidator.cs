using System.Collections.Generic;
using System.Text;
using EonValidation.Runtime;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class MissingComponentsValidator
    {
        private static readonly StringBuilder stringBuilder = new();
        private static readonly Stack<Transform> transformsBuffer = new();
        
        public static List<ValidationIssue> ValidateGameObject(GameObject target, Object context = null)
        {
            var result = new List<ValidationIssue>();
            context ??= target;
            foreach (var child in target.transform.IterateChildrenRecursively())
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
        
        private static IEnumerable<Transform> IterateChildrenRecursively(this Transform parent)
        {
            transformsBuffer.Clear();
            transformsBuffer.Push(parent);

            while (transformsBuffer.Count > 0)
            {
                var current = transformsBuffer.Pop();
                yield return current;

                foreach (Transform child in current)
                {
                    transformsBuffer.Push(child);
                }
            }
        }
    }
}