using EonValidation.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EonValidation.Tests
{
    public class MissingScriptableObject
    {
        private static string[] ScriptableObjectsInAssetsFolder =>
            ValidationPaths.GetAllAssetsWithExtensionInAssetsFolder(".asset");

        [Test]
        public void FindMissingScriptableObjects(
            [ValueSource(nameof(ScriptableObjectsInAssetsFolder))] string assetPath)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            var scriptableObject = asset as ScriptableObject;
            if (scriptableObject)
            {
                return;
            }

            Debug.LogError($"Missing ScriptableObject: {assetPath}", asset);
        }
    }
}