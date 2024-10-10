using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class ValidationPaths
    {
        private const string VALIDATION_TARGET_FILTER = "l:ValidationTarget";
        private const string PREFAB_FILTER = "t:prefab";
        private static readonly string[] assetsFolder = {"Assets"};
        
        public static string[] GetAllPrefabPathsInAssetsFolder() =>
            AssetDatabase.FindAssets(PREFAB_FILTER, assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();

        public static string[] GetAllScenesInAssetsFolder() =>
            AssetDatabase.FindAssets("t:scene", assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();

        public static string[] GetAllScriptableObjectsInAssetsFolder() =>
            AssetDatabase.FindAssets($"t:{nameof(ScriptableObject)}", assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();

        public static string[] GetAllAssetsWithExtensionInAssetsFolder(string extension) =>
            AssetDatabase.FindAssets($"t:{nameof(Object)}", assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .Where(path => path.EndsWith(extension))
                         .ToArray();
        
        public static string[] GetAllValidationTargetFolders() =>
            AssetDatabase.FindAssets(VALIDATION_TARGET_FILTER)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .Where(path => !Path.HasExtension(path))
                         .ToArray();
        
        public static string[] GetAllValidationTargetPrefabs()
        {
            var folders = GetAllValidationTargetFolders();
            var prefabsInFolders = AssetDatabase.FindAssets(PREFAB_FILTER, folders)
                                               .Select(AssetDatabase.GUIDToAssetPath)
                                               .ToHashSet();
            
            var prefabs = AssetDatabase.FindAssets($"{PREFAB_FILTER} {VALIDATION_TARGET_FILTER}", assetsFolder)
                                      .Select(AssetDatabase.GUIDToAssetPath)
                                      .ToHashSet();
            
            prefabs.UnionWith(prefabsInFolders);
            return prefabs.ToArray();
        }
    }
}