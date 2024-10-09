using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEditor;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class MissingReferenceFinder
    {
        public static ValidationIssue[] FindMissingReferences(Object targetObject, Object context = null)
        {
            context ??= targetObject;
            var result = new List<ValidationIssue>();
            using var serializedObject = new SerializedObject(targetObject);
            var serializedProperty = serializedObject.GetIterator();

            while (serializedProperty.NextVisible(true))
            {
                if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference &&
                    serializedProperty.objectReferenceValue == null &&
                    serializedProperty.objectReferenceInstanceIDValue != 0)
                {
                    result.Add(new ValidationIssue
                    {
                        Message = "Missing reference",
                        PropertyPath = $"{targetObject.GetType().Name}/{serializedProperty.propertyPath}",
                        Context = context,
                    });
                }
            }

            return result.ToArray();
        }
    }
}