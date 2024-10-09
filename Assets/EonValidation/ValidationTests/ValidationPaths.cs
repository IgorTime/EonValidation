using System.Linq;
using UnityEditor;

namespace EonValidation.ValidationTests
{
    public static class ValidationPaths
    {
        public static string[] GetAllPrefabPathsInAssetsFolder() =>
            AssetDatabase.FindAssets("t:prefab", new []{"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();
        
        public static string[] GetAllScenesInAssetsFolder() =>
            AssetDatabase.FindAssets("t:scene", new []{"Assets"})
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();
    }
}
