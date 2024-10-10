using System.Collections.Generic;

namespace EonValidation.Runtime
{
    public interface IValidatable
    {
        public void Validate(ref List<ValidationIssue> issues);
    }
}