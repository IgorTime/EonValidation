using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EonValidation.ValidationTests.Code
{
    public class MissingComponentsValidationTest
    {
        private static string[] PrefabsInAssetsFolder => ValidationPaths.GetAllPrefabPathsInAssetsFolder();
    
        [Test]
        public void FindMissingComponents([ValueSource(nameof(PrefabsInAssetsFolder))] string assetPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            foreach (var component in prefab.GetComponentsInChildren<Component>(true))
            {
                if (!component)
                {
                    Debug.LogError($"Missing component: {assetPath}.", prefab);
                    Assert.Fail();
                    return;
                }
            }
        
        }
    }
}
