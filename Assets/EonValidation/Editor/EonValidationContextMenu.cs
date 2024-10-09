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
                
                if (asset is GameObject gameObject)
                {
                    foreach (var missingReference in MissingReferenceFinder.IterateOverMissingReferences(gameObject))
                    {
                        missingReference.objectReferenceValue = null;
                        missingReference.serializedObject.ApplyModifiedProperties();
                    }
                    continue;
                }
                
                if (asset is ScriptableObject scriptableObject)
                {
                    foreach (var missingReference in MissingReferenceFinder.IterateOverMissingReferences(scriptableObject))
                    {
                        missingReference.objectReferenceValue = null;
                        missingReference.serializedObject.ApplyModifiedProperties();
                    }
                    continue;
                }
            }
        }
    }
}