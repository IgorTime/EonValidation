using System;
using EonValidation.Runtime;
using UnityEngine;

public class ValidatableComponent : MonoBehaviour, IValidatable
{
    public int value;
    
    public ValidationIssue[] Validate()
    {
        if (value < 0)
        {
            return new[]
            {
                new ValidationIssue
                {
                    Message = "Value is negative",
                    Context = this,
                }
            };
        }

        return Array.Empty<ValidationIssue>();
    }
}
