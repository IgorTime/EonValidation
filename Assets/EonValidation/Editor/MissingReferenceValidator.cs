using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EonValidation.Editor
{
    public static class MissingReferenceValidator
    {
        public static List<ValidationIssue> ValidateGameObject(GameObject target, Object context = null)
        {
            if (!target)
            {
                return new List<ValidationIssue>();
            }

            var result = new List<ValidationIssue>();
            context ??= target;
            foreach (var child in target.IterateChildrenRecursively())
            {
                foreach (var component in child.GetComponents<Component>())
                {
                    if (!component)
                    {
                        continue;
                    }

                    var issues = MissingReferenceFinder.FindMissingReferences(component, context);
                    result.AddRange(issues);
                }
            }

            return result;
        }

        public static List<ValidationIssue> ValidateScriptableObject(ScriptableObject scriptableObject) =>
            MissingReferenceFinder.FindMissingReferences(scriptableObject);

        public static List<ValidationIssue> ValidateScene(Scene scene, Object context = null)
        {
            var result = new List<ValidationIssue>();
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var issues = ValidateGameObject(rootGameObject, context);
                result.AddRange(issues);
            }

            return result;
        }
    }
}