using System;
using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EonValidation.Editor
{
    public static class MissingReferenceUtils
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

        public static bool ClearMissingReferences(Object target, bool log = false)
        {
            var any = ClearMissingReferencesInternal(target, log);
            if (any)
            {
                AssetDatabase.SaveAssets();
            }
            
            return any;
        }

        private static bool ClearMissingReferencesInternal(Object target, bool log)
        {
            switch (target)
            {
                case GameObject gameObject: return ClearMissingReferencesInGameObject(gameObject, log);
                case ScriptableObject scriptableObject: return ClearMissingReferences(scriptableObject, log);
                case SceneAsset sceneAsset: return ClearMissingReferencesInScene(sceneAsset, log);
                default: return ClearMissingReferencesInObject(target, log);
            }
        }

        private static bool ClearMissingReferencesInGameObject(GameObject gameObject, bool log = false)
        {
            var any = false;
            foreach (var missingReference in IterateOverMissingReferences(gameObject))
            {
                ClearMissingReference(missingReference, log);
                any = true;
            }

            return any;
        }

        private static bool ClearMissingReferences(ScriptableObject asset, bool log = false)
        {
            var any = false;
            foreach (var missingReference in IterateOverMissingReferences(asset))
            {
                ClearMissingReference(missingReference, log);
                any = true;
            }

            return any;
        }

        private static bool ClearMissingReferencesInScene(SceneAsset sceneAsset, bool log)
        {
            var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            var any = false;
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                any |= ClearMissingReferencesInGameObject(rootGameObject, log);
            }

            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.CloseScene(scene, true);
            return any;
        }

        private static bool ClearMissingReferencesInObject(Object target, bool log)
        {
            var any = false;
            foreach (var missingReference in IterateOverMissingReferences(target))
            {
                ClearMissingReference(missingReference, log);
                any = true;
            }

            return any;
        }

        private static void ClearMissingReference(SerializedProperty missingReference, bool log = false)
        {
            missingReference.objectReferenceValue = null;
            missingReference.serializedObject.ApplyModifiedProperties();

            if (!log)
            {
                return;
            }

            var objectName = missingReference.serializedObject.targetObject.name;
            var propertyPath = missingReference.propertyPath;
            Debug.Log($"Cleared missing reference in '{objectName}' at '{propertyPath}'");
        }
    }
}