using EonValidation.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EonValidation.Tests
{
    public class ValidatableComponents
    {
        private static string[] Prefabs => ValidationPaths.GetAllValidationTargetPrefabs();
        
        [Test]
        public void ValidateComponents([ValueSource(nameof(Prefabs))] string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var issues = InterfaceValidator.ValidateGameObject(prefab);
            EonAssert.IssuesAreEmpty(issues);
        }
    }
}