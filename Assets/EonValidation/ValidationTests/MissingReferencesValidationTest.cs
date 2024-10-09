﻿using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EonValidation.ValidationTests
{
    public class MissingReferencesValidationTest
    {
        private static string[] PrefabPaths => ValidationPaths.GetAllPrefabPathsInAssetsFolder();
        private static string[] ScriptableObjectPaths => ValidationPaths.GetAllScriptableObjectsInAssetsFolder();
        
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
    }
}