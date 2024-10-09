using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public class MissingComponentsValidationTest
    {
        private static string[] PrefabsInAssetsFolder => ValidationPaths.GetAllPrefabPathsInAssetsFolder();
    
        [Test]
        public void FindMissingComponentsOnPrefabs([ValueSource(nameof(PrefabsInAssetsFolder))] string assetPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            foreach (var component in prefab.GetComponentsInChildren<Component>(true))
            {
                if (component)
                {
                    continue;
                }

                Debug.LogError($"Missing component on Prefab: {assetPath}.", prefab);
                Assert.Fail();
                return;
            }
        }
    }
}
