using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EonValidation.Editor
{
    public static class InterfaceValidator
    {
        public static ValidationIssue[] ValidateObject(object target)
        {
            var issues = new List<ValidationIssue>();
            if (target is IValidatable validatable)
            {
                validatable.Validate(ref issues);
            }

            return issues.ToArray();
        }

        public static ValidationIssue[] ValidateGameObject(GameObject target)
        {
            var issues = new List<ValidationIssue>();
            foreach (var validatable in target.GetComponentsInChildren<IValidatable>(true))
            {
                validatable.Validate(ref issues);
            }

            return issues.ToArray();
        }

        public static ValidationIssue[] ValidateScene(Scene scene)
        {
            var issues = new List<ValidationIssue>
            {
                new(),
            };

            foreach (var gameObject in scene.GetRootGameObjects())
            {
                issues.AddRange(ValidateGameObject(gameObject));
            }

            if (issues.Count == 1)
            {
                issues.Clear();
            }
            else
            {
                var sceneAssetPath = scene.path;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
                issues[0] = new ValidationIssue
                {
                    Message = "Invalid scene found. See issues for details.",
                    Context = sceneAsset,
                };
            }

            return issues.ToArray();
        }

        public static ValidationIssue[] ValidateScene(SceneAsset sceneAsset)
        {
            var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            var issues = ValidateScene(scene);
            EditorSceneManager.CloseScene(scene, true);
            return issues;
        }
    }
}