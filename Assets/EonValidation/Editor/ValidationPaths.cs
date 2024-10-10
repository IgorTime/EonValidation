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
        private const string SO_FILTER = "t:ScriptableObject";
        private const string SCENE_FILTER = "t:Scene";
        private static readonly string[] assetsFolder = {"Assets"};
        
        public static string[] GetAllPrefabPathsInAssetsFolder() =>
            AssetDatabase.FindAssets(PREFAB_FILTER, assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();

        public static string[] GetAllScenesInAssetsFolder() =>
            AssetDatabase.FindAssets(SCENE_FILTER, assetsFolder)
                         .Select(AssetDatabase.GUIDToAssetPath)
                         .ToArray();

        public static string[] GetAllScriptableObjectsInAssetsFolder() =>
            AssetDatabase.FindAssets(SO_FILTER, assetsFolder)
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

        public static string[] GetAllValidationTargetScriptableObjects()
        {
            var folders = GetAllValidationTargetFolders();
            var scriptableObjectsInFolders = AssetDatabase.FindAssets(SO_FILTER, folders)
                                                         .Select(AssetDatabase.GUIDToAssetPath)
                                                         .ToHashSet();
            
            var scriptableObjects = AssetDatabase.FindAssets($"{SO_FILTER} {VALIDATION_TARGET_FILTER}", assetsFolder)
                                                .Select(AssetDatabase.GUIDToAssetPath)
                                                .ToHashSet();
            
            scriptableObjects.UnionWith(scriptableObjectsInFolders);
            return scriptableObjects.ToArray();
        }

        public static string[] GetAllValidationTargetScenes()
        {
            var folders = GetAllValidationTargetFolders();
            var scenesInFolders = AssetDatabase.FindAssets(SCENE_FILTER, folders)
                                               .Select(AssetDatabase.GUIDToAssetPath)
                                               .ToHashSet();
            
            var scenes = AssetDatabase.FindAssets($"{SCENE_FILTER} {VALIDATION_TARGET_FILTER}", assetsFolder)
                                      .Select(AssetDatabase.GUIDToAssetPath)
                                      .ToHashSet();
            
            scenes.UnionWith(scenesInFolders);
            return scenes.ToArray();
        }
    }
}