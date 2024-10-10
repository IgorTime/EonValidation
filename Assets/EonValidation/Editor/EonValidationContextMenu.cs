using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EonValidation.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EonValidation.Editor
{
    public static class EonValidationContextMenu
    {
        [MenuItem("Assets/EonValidation/Clear Missing References")]
        public static void ClearMissingReferencesAssets()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (!asset)
                {
                    continue;
                }

                var any = MissingReferenceUtils.ClearMissingReferences(asset, true);
                if (!any)
                {
                    Debug.Log($"No missing references in '{assetPath}'");
                }
            }
        }

        [MenuItem("GameObject/EonValidation/Clear Missing References", false, 0)]
        public static void ClearMissingReferencesGameObjects(MenuCommand command)
        {
            var targetGameObject = command.context as GameObject;
            if (!targetGameObject)
            {
                return;
            }
            
            var any = MissingReferenceUtils.ClearMissingReferences(targetGameObject, true);
            if (!any)
            {
                Debug.Log($"No missing references in '{targetGameObject.name}'");
            }
        }

        [MenuItem("GameObject/EonValidation/Validate Components", false, 0)]
        public static void ValidateGameObjects(MenuCommand command)
        {
            var targetGameObject = command.context as GameObject;
            if (targetGameObject)
            {
                var issues = InterfaceValidator.ValidateGameObject(targetGameObject);
                ValidationIssue.LogIssues(issues, targetGameObject);
            }
        }

        [MenuItem("Assets/EonValidation/Validate", false, 0)]
        public static void ValidateAssets()
        {
            foreach (var target in GetAllSelectedValidatableTargets())
            {
                var issues = target switch
                {
                    GameObject gameObject => InterfaceValidator.ValidateGameObject(gameObject),
                    ScriptableObject scriptableObject => InterfaceValidator.ValidateObject(scriptableObject),
                    SceneAsset sceneAsset => InterfaceValidator.ValidateScene(sceneAsset),
                    _ => Array.Empty<ValidationIssue>(),
                };

                ValidationIssue.LogIssues(issues, target);
            }
        }

        [MenuItem("Assets/EonValidation/Validate", true)]
        public static bool ValidateAssetsValidation()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!IsFolder(assetPath) &&
                    !assetPath.EndsWith(".prefab") && 
                    !assetPath.EndsWith(".asset") &&
                    !assetPath.EndsWith(".unity"))
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<Object> GetAllSelectedValidatableTargets()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (IsFolder(path))
                {
                    var objects = AssetDatabase.FindAssets("t:prefab t:ScriptableObject t:scene", new[] {path})
                                               .Select(AssetDatabase.GUIDToAssetPath)
                                               .Select(AssetDatabase.LoadAssetAtPath<Object>);

                    foreach (var obj in objects)
                    {
                        yield return obj;
                    }

                    continue;
                }

                if (path.EndsWith(".prefab") || path.EndsWith(".asset") || path.EndsWith(".unity"))
                {
                    yield return AssetDatabase.LoadAssetAtPath<Object>(path);
                }
            }
        }

        private static bool IsFolder(string path) => !Path.HasExtension(path);
    }
}