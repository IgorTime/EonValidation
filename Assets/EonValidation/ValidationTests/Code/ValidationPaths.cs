using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ValidationPaths
{
    public static string[] GetAllPrefabPathsInAssetsFolder() =>
        AssetDatabase.FindAssets("t:prefab", new []{"Assets"})
                     .Select(AssetDatabase.GUIDToAssetPath)
                     .ToArray();
}
