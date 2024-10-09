using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEditor;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class MissingReferenceFinder
    {
        public static List<ValidationIssue> FindMissingReferences(Object targetObject, Object context = null)
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
                    var message = $"Missing reference. Target: {targetObject}. Property: {serializedProperty.propertyPath}";
                    result.Add(new ValidationIssue
                    {
                        Message = message,
                        HierarchyPath = serializedProperty.propertyPath,
                        Context = context
                    });
                }
            }

            return result;
        }
    }
}