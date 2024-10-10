using System.Collections.Generic;
using EonValidation.Runtime;
using UnityEngine;

public class ValidatableComponent : MonoBehaviour, IValidatable
{
    public int value;
    
    public void Validate(ref List<ValidationIssue> issues)
    {
        if (value < 0)
        {
            issues.Add(new ValidationIssue("Value is negative", this));
        }
    }
}
