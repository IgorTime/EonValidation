using System;
using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EonValidation.Editor
{
    public static class MissingReferenceFinder
    {
        private static readonly HashSet<Type> ignoredTypes = new()
        {
            typeof(GUISkin),
        };

        public static ValidationIssue[] FindMissingReferences(Object targetObject, Object context = null)
        {
            if (targetObject == null)
            {
                return Array.Empty<ValidationIssue>();
            }

            if (ignoredTypes.Contains(targetObject.GetType()))
            {
                return Array.Empty<ValidationIssue>();
            }

            context ??= targetObject;
            var result = new List<ValidationIssue>();
            foreach (var serializedProperty in IterateOverMissingReferences(targetObject))
            {
                result.Add(new ValidationIssue
                {
                    Message = "Missing reference",
                    PropertyPath = $"{targetObject.GetType().Name}/{serializedProperty.propertyPath}",
                    Context = context,
                });
            }

            return result.ToArray();
        }

        public static IEnumerable<SerializedProperty> IterateOverMissingReferences(Object targetObject)
        {
            if (targetObject == null)
            {
                yield break;
            }

            if (ignoredTypes.Contains(targetObject.GetType()))
            {
                yield break;
            }

            using var serializedObject = new SerializedObject(targetObject);
            var serializedProperty = serializedObject.GetIterator();

            while (serializedProperty.NextVisible(true))
            {
                if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference &&
                    serializedProperty.objectReferenceValue == null &&
                    serializedProperty.objectReferenceInstanceIDValue != 0)
                {
                    yield return serializedProperty;
                }
            }
        }

        public static IEnumerable<SerializedProperty> IterateOverMissingReferences(GameObject gameObject)
        {
            foreach (var child in gameObject.IterateChildrenRecursively())
            {
                foreach (var component in child.GetComponents<Component>())
                {
                    foreach (var missingReference in IterateOverMissingReferences(component))
                    {
                        yield return missingReference;
                    }
                }
            }
        }
    }
}