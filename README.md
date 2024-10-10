# EonValidation

A simple and lightweight validation framework for Unity.

## Installation

You can install EonValidation via Unity Package Manager using the following link:

[https://github.com/IgorTime/EonValidation.git?path=/Assets/EonValidation](https://github.com/IgorTime/EonValidation.git?path=/Assets/EonValidation)

## Features

- **Simple and lightweight**: EonValidation is a minimalistic validation tool for Unity. It's built on top of Unity's Test Runner, allowing you to easily integrate validation into your build pipeline.
- **Pre-built validations**: The framework includes three out-of-the-box validations:
  1. **MissingComponents**: Detects missing components in game objects.
  2. **MissingScriptableObjects**: Identifies missing references to scriptable objects.
  3. **MissingReferences**: Scans all assets in the `Assets` folder for broken or missing references.
- **Context menu tool**: A right-click (RMB) context menu tool is available at `EonValidation -> Clear Missing References`, allowing you to automatically clear missing references on selected targets.
- **Custom validation**: Supports custom validation for components and scriptable objects using the `IValidatable` interface.

## Custom Validation

To create custom validation for your components or scriptable objects, follow these steps:

1. **Implement the `IValidatable` interface** by creating a class that defines a `Validate` method. This method should take a list of validation issues as a parameter and add issues to it if the component or scriptable object is invalid (e.g., a negative value).
```csharp
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
```
  
2. **Manual validation**:
   - Use the RMB context menu `EonValidation -> Validate` to run validation on selected items.
   - You can validate multiple items by selecting them simultaneously.
   - You can also validate entire scenes or folders. In the case of folders, all prefabs, scriptable objects, and scenes within the folder will be validated.

3. **Setting up tests for custom validation**:
   - Use the **Asset Label** feature to configure tests for custom validations.
![image](https://github.com/user-attachments/assets/9a1ceab9-d3f3-43dc-abd6-da951e31201e)

   - Add the label `'ValidationTarget'` to your target assets to include them in validation tests.
   - You can also label entire folders. In that case, all prefabs, scenes, and scriptable objects within that folder will be included in the validation tests.
   - A common approach is to organize your project assets into a content folder and label the root folder as a `ValidationTarget`. This ensures that only your project assets are validated, excluding third-party plugins.
