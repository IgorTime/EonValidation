using EonValidation.Runtime;
using UnityEditor;
using UnityEngine;

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
    }
}