using EonValidation.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EonValidation.Tests
{
    public class ValidatableTargets
    {
        private static string[] Prefabs => ValidationPaths.GetAllValidationTargetPrefabs();
        private static string[] ScriptableObjects => ValidationPaths.GetAllValidationTargetScriptableObjects();
        private static string[] Scenes => ValidationPaths.GetAllValidationTargetScenes();

        [Test]
        public void ValidateComponents([ValueSource(nameof(Prefabs))] string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var issues = InterfaceValidator.ValidateGameObject(prefab);
            EonAssert.IssuesAreEmpty(issues);
        }

        [Test]
        public void ValidateScriptableObjects([ValueSource(nameof(ScriptableObjects))] string path)
        {
            var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            var issues = InterfaceValidator.ValidateObject(scriptableObject);
            EonAssert.IssuesAreEmpty(issues);
        }

        [Test]
        public void ValidateScenes([ValueSource(nameof(Scenes))] string path)
        {
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            var issues = InterfaceValidator.ValidateScene(scene);
            EditorSceneManager.CloseScene(scene, true);
            EonAssert.IssuesAreEmpty(issues);
        }
    }
}