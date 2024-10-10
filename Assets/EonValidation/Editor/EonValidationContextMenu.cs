using UnityEditor;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class EonValidationContextMenu
    {
        [MenuItem("Assets/EonValidation/Clear Missing References")]
        public static void ClearMissingReferences()
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
    }
}