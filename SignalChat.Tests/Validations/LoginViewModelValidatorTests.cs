using FluentValidation.TestHelper;
using SignalChat.Models;
using SignalChat.Validations;

namespace SignalChat.Tests.Validations
{
    [TestClass]
    public class LoginViewModelValidatorTests
    {
        [TestMethod]
        public void Validate_EmptyUsername_ReturnsInvalid()
        {
            var model = new LoginViewModel { Username = "" };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithLessThanFiveCharacters_ReturnsInvalid()
        {
            var model = new LoginViewModel { Username = "ab" };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithMoreThan256Characters_ReturnsInvalid()
        {
            var model = new LoginViewModel { Username = new string('a', 257) };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithInvalidCharacters_ReturnsInvalid()
        {
            var model = new LoginViewModel { Username = "abc123!@#" };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_EmptyPassword_ReturnsInvalid()
        {
            var model = new LoginViewModel { Password = "" };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [TestMethod]
        public void Validate_PasswordWithLessThanEightCharacters_ReturnsInvalid()
        {
            var model = new LoginViewModel { Password = "short" };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [TestMethod]
        public void Validate_ValidUsernameAndPassword_ReturnsValid()
        {
            var model = new LoginViewModel
            {
                Username = "LongerThanFiveCharsOnlyLetters",
                Password = "LongerThan8Chars"
            };

            var validator = new LoginViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}