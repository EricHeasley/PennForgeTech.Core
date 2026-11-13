using PennForgeTech.Core.Results;
using PennForgeTech.Core.Validation;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ValidationTests
    {
        private sealed class NewCustomer
        {
            public string Name { get; set; } = string.Empty;

            public int Age { get; set; }
        }

        private sealed class NewCustomerValidator : Validator<NewCustomer>
        {
            public NewCustomerValidator()
            {
                this.AddRule(nameof(NewCustomer.Name), customer => !string.IsNullOrWhiteSpace(customer.Name), "Name is required.");
                this.AddRule(nameof(NewCustomer.Age), customer => customer.Age >= 18, "Customer must be at least 18.");
            }
        }

        [Fact]
        public void Validate_PassesWhenEveryRuleIsMet()
        {
            NewCustomerValidator validator = new NewCustomerValidator();

            ValidationResult result = validator.Validate(new NewCustomer { Name = "Eric", Age = 30 });

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ReportsEveryFailedRule()
        {
            NewCustomerValidator validator = new NewCustomerValidator();

            ValidationResult result = validator.Validate(new NewCustomer { Name = " ", Age = 5 });

            Assert.False(result.IsValid);
            Assert.Equal(2, result.Failures.Count);
            Assert.Equal("Name", result.Failures[0].PropertyName);
        }

        [Fact]
        public void ThrowIfInvalid_ThrowsWithTheFailures()
        {
            NewCustomerValidator validator = new NewCustomerValidator();
            ValidationResult result = validator.Validate(new NewCustomer());

            ValidationException exception = Assert.Throws<ValidationException>(() => result.ThrowIfInvalid());

            Assert.Equal(2, exception.Failures.Count);
        }

        [Fact]
        public void ToResult_ReturnsFailureWithValidationCode()
        {
            NewCustomerValidator validator = new NewCustomerValidator();

            Result result = validator.Validate(new NewCustomer()).ToResult();

            Assert.True(result.IsFailure);
            Assert.Equal("Validation.Failed", result.Error.Code);
        }
    }
}
