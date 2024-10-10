using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

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
    }
}