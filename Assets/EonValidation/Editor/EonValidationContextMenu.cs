using System;
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
        public static void ClearMissingReferencesGameObjects()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                var any = MissingReferenceUtils.ClearMissingReferences(gameObject, true);
                if (!any)
                {
                    Debug.Log($"No missing references in '{gameObject.name}'");
                }
            }
        }

        [MenuItem("GameObject/EonValidation/Validate Components", false, 0)]
        public static void ValidateGameObjects(MenuCommand command)
        {
            var targetGameObject = command.context as GameObject;
            if (targetGameObject)
            {
                var issues = InterfaceValidator.ValidateGameObject(targetGameObject);
                ValidationIssue.LogIssues(issues);
            }
        }
        
        [MenuItem("Assets/EonValidation/Validate", false, 0)]
        public static void ValidateAssets()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                var issues = asset switch
                {
                    GameObject gameObject => InterfaceValidator.ValidateGameObject(gameObject),
                    ScriptableObject scriptableObject => InterfaceValidator.ValidateObject(scriptableObject),
                    _ => Array.Empty<ValidationIssue>()
                };

                ValidationIssue.LogIssues(issues);
            }
        }
        
        [MenuItem("Assets/EonValidation/Validate", true)]
        public static bool ValidateAssetsValidation()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if(!assetPath.EndsWith(".prefab") && 
                   !assetPath.EndsWith(".asset"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}