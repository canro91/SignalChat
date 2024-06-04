using FluentValidation.TestHelper;
using SignalChat.Models;
using SignalChat.Validations;

namespace SignalChat.Tests.Validations
{
    [TestClass]
    public class RegisterViewModelValidatorTests
    {
        [TestMethod]
        public void Validate_EmptyUsername_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Username = "" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithLessThanFiveCharacters_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Username = "a" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithMoreThan256Characters_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Username = new string('a', 257) };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithInvalidCharacters_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Username = "abc123!@#" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_EmptyPassword_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Password = "" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [TestMethod]
        public void Validate_PasswordWithLessThanEightCharacters_ReturnsInvalid()
        {
            var model = new RegisterViewModel { Password = "short" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [TestMethod]
        public void Validate_EmptyConfirmPassword_ReturnsInvalid()
        {
            var model = new RegisterViewModel { ConfirmPassword = "" };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [TestMethod]
        public void Validate_ConfirmPasswordDifferentFromPassword_ReturnsInvalid()
        {
            var model = new RegisterViewModel
            {
                Password = "password",
                ConfirmPassword = "differentpassword"
            };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [TestMethod]
        public void Validate_ValidUsernamePasswordAndConfirmPassword_ReturnsValid()
        {
            var model = new RegisterViewModel
            {
                Username = "username",
                Password = "password",
                ConfirmPassword = "password"
            };

            var validator = new RegisterViewModelValidator();
            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}