namespace EonValidation.Runtime
{
    public interface IValidatable
    {
        public ValidationIssue[] Validate();
    }
}