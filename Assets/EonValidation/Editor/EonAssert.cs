using System.Collections.Generic;
using EonValidation.Runtime;
using NUnit.Framework;

namespace EonValidation.Editor
{
    public static class EonAssert
    {
        public static void IssuesAreEmpty(IEnumerable<ValidationIssue> issues)
        {
            if (issues == null)
            {
                return;
            }
            
            var anyIssues = false;
            foreach (var issue in issues)
            {
                issue.LogError();
                anyIssues = true;
            }
            
            if (anyIssues)
            {
                Assert.Fail();
            }
        }
    }
}