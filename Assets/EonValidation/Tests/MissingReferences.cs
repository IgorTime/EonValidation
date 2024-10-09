using EonValidation.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EonValidation.Tests
{
    public class MissingReferences
    {
        private static string[] PrefabPaths => ValidationPaths.GetAllPrefabPathsInAssetsFolder();
        private static string[] ScriptableObjectPaths => ValidationPaths.GetAllScriptableObjectsInAssetsFolder();
        private static string[] ScenePaths => ValidationPaths.GetAllScenesInAssetsFolder();

        [Test]
        public void FindMissingReferencesInPrefabs([ValueSource(nameof(PrefabPaths))] string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var issues = MissingReferenceValidator.ValidateGameObject(prefab);

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
        public void FindMissingReferencesInScriptableObjects([ValueSource(nameof(ScriptableObjectPaths))] string path)
        {
            var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            var issues = MissingReferenceValidator.ValidateScriptableObject(scriptableObject);

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
        public void FindMissingReferencesInScenes([ValueSource(nameof(ScenePaths))] string path)
        {
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            try
            {
                var issues = MissingReferenceValidator.ValidateScene(scene, sceneAsset);

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