using System;
using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

namespace EonValidation.Editor
{
    public static class InterfaceValidator
    {
        public static ValidationIssue[] ValidateObject(object target)
        {
            if (target is IValidatable validatable)
            {
                return validatable.Validate();
            }

            return Array.Empty<ValidationIssue>();
        }
        
        public static ValidationIssue[] ValidateGameObject(GameObject target)
        {
            var issues = new List<ValidationIssue>();
            foreach (var validatable in target.GetComponentsInChildren<IValidatable>(true))
            {
                issues.AddRange(validatable.Validate());
            }
            
            return issues.ToArray();
        }
    }
}