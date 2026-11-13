namespace PennForgeTech.Core.Validation
{
    public interface IValidator<in TInstance>
    {
        ValidationResult Validate(TInstance instance);
    }
}
