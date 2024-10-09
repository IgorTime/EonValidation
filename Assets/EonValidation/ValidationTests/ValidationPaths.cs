using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public static class ValidationPaths
    {
        public static string[] GetAllPrefabPathsInAssetsFolder() =>
            AssetDatabase.FindAssets("t:prefab", new[] {"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();
        
        public static string[] GetAllScenesInAssetsFolder() =>
            AssetDatabase.FindAssets("t:scene", new[] {"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();
        
        public static string[] GetAllScriptableObjectsInAssetsFolder() =>
            AssetDatabase.FindAssets($"t:{nameof(ScriptableObject)}", new[] {"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();
        
        public static string[] GetAllAssetsWithExtensionInAssetsFolder(string extension) =>
            AssetDatabase.FindAssets($"t:{nameof(Object)}", new[] {"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .Where(path => path.EndsWith(extension))
                         .ToArray();
    }
}