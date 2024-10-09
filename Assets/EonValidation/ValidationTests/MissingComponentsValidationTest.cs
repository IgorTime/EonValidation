using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public class MissingComponentsValidationTest
    {
        private static string[] PrefabsInAssetsFolder => ValidationPaths.GetAllPrefabPathsInAssetsFolder();
        private static string[] ScenesInAssetsFolder => ValidationPaths.GetAllScenesInAssetsFolder();

        [Test]
        public void FindMissingComponentsOnPrefabs([ValueSource(nameof(PrefabsInAssetsFolder))] string assetPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            var issues = MissingComponentsValidator.ValidateGameObject(prefab);
            if (issues.Count <= 0)
            {
                return;
            }

            foreach (var issue in issues)
            {
                issue.LogError();
            }
        }

        [Test]
        public void FindMissingComponentsInScenes([ValueSource(nameof(ScenesInAssetsFolder))] string scenePath)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            try
            {
                var issues = new List<ValidationIssue>();
                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    var validationIssues = MissingComponentsValidator.ValidateGameObject(rootGameObject, sceneAsset);
                    issues.AddRange(validationIssues);
                }

                if (issues.Count <= 0)
                {
                    return;
                }

                foreach (var issue in issues)
                {
                    issue.LogError();
                }

                Assert.Fail();
            }
            finally
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }
}