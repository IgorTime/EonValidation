using System.Text;
using UnityEngine;

namespace EonValidation.Runtime
{
    public struct ValidationIssue
    {
        private static readonly StringBuilder stringBuilder = new();

        public string Message;
        public string HierarchyPath;
        public string PropertyPath;
        public Object Context;

        public ValidationIssue(string message, Object context)
        {
            Message = message;
            HierarchyPath = context is Component component
                ? component.transform.GetHierarchyPath()
                : null;

            PropertyPath = null;
            Context = context;
        }

        public static void LogIssues(ValidationIssue[] issues, Object context = null)
        {
            if (issues.Length == 0)
            {
                Debug.Log(context 
                    ? $"{context.name}: no issues found in" 
                    : "No issues found");
                return;
            }

            for (var index = 0; index < issues.Length; index++)
            {
                issues[index].LogError();
            }
        }

        public override string ToString()
        {
            stringBuilder.Clear();
            AppendIfNotEmpty(Message);
            AppendIfNotEmpty(Context?.ToString(), "Context");
            AppendIfNotEmpty(HierarchyPath, "Hierarchy path");
            AppendIfNotEmpty(PropertyPath, "Property path");
            return stringBuilder.ToString();
        }

        public void LogError()
        {
            Debug.LogError(ToString(), Context);
        }

        private void AppendIfNotEmpty(string value, string prefix = "")
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(", ");
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                stringBuilder.Append(prefix).Append(": ");
            }

            stringBuilder.Append(value);
        }
    }
}