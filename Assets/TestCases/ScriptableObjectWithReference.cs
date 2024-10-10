using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

namespace TestCases
{
    [CreateAssetMenu]
    public class ScriptableObjectWithReference : ScriptableObject, IValidatable
    {
        public int value;
        public Material materialReference;
        public void Validate(ref List<ValidationIssue> issues)
        {
            if (value > 10)
            {
                issues.Add(new ValidationIssue("Value is greater than 10", this));
            }
        }
    }
}