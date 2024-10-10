# EonValidation

A simple and lightweight validation framework for Unity.

## Installation

You can install EonValidation via Unity Package Manager using the following link:

[https://github.com/IgorTime/EonValidation.git?path=/Assets/EonValidation](https://github.com/IgorTime/EonValidation.git?path=/Assets/EonValidation)

## Features

- **Simple and lightweight**: EonValidation is a minimalistic validation tool for Unity. It's built on top of Unity's Test Runner, allowing you to easily integrate validation into your build pipeline.
- **Pre-built validations**: The framework comes with three out-of-the-box validations:
  1. **MissingComponents**: Detects missing components in game objects.
  2. **MissingScriptableObjects**: Identifies missing references to scriptable objects.
  3. **MissingReferences**: Scans all assets in the `Assets` folder for broken or missing references.
- **Context menu tool**: A right-click (RMB) context menu tool is available at `EonValidation -> Clear Missing References`, allowing you to automatically clear missing references on selected targets.
