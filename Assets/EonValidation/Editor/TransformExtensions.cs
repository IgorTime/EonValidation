using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class TransformExtensions
    {
        private static readonly Stack<Transform> transformsBuffer = new();
        private static readonly StringBuilder stringBuilder = new();

        public static string GetHierarchyPath(this Transform target)
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

        public static IEnumerable<Transform> IterateChildrenRecursively(this Transform parent)
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

        public static IEnumerable<Transform> IterateChildrenRecursively(this GameObject gameObject) =>
            IterateChildrenRecursively(gameObject.transform);
    }
}