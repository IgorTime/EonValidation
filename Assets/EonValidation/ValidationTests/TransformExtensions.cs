using System.Collections.Generic;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class TransformExtensions
    {
        private static readonly Stack<Transform> transformsBuffer = new();

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