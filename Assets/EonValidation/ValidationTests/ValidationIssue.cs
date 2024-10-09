using UnityEngine;

namespace EonValidation.ValidationTests
{
    public struct ValidationIssue
    {
        public string Message;
        public string HierarchyPath;
        public Object Context;

        public override string ToString()
        {
            return $"Message: {Message}, Object: {Context}, HierarchyPath: {HierarchyPath}";
        }

        public void LogError()
        {
            Debug.LogError(ToString(), Context);
        }
    }
}