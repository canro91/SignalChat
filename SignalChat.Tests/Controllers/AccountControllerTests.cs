using Microsoft.AspNetCore.Mvc;
using ServiceStack.OrmLite;
using SignalChat.Controllers;
using SignalChat.Core.Tasks;
using SignalChat.Database.Migrations;
using SignalChat.Database.Repositories;
using SignalChat.Models;

namespace SignalChat.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public async Task RegisterAsync_ValidRequest_ReturnsOk()
        {
            var controller = BuildController();

            var request = new RegisterViewModel
            {
                Username = "username",
                Password = "password",
                ConfirmPassword = "password"
            };
            var result = await controller.RegisterAsync(request);

            Assert.IsInstanceOfType<OkResult>(result);
        }

        [TestMethod]
        public async Task RegisterAsync_UserAlreadyExists_ThrowsException()
        {
            var controller = BuildController();

            var request = new RegisterViewModel
            {
                Username = "username",
                Password = "password",
                ConfirmPassword = "password"
            };
            await controller.RegisterAsync(request);

            await Assert.ThrowsExceptionAsync<ArgumentException>(()
                => controller.RegisterAsync(request));
        }

        [TestMethod]
        public async Task LoginAsync_UserDoesNotExist_ReturnsUnauthorized()
        {
            var controller = BuildController();

            var userDoesNotExist = new LoginViewModel
            {
                Username = "username",
                Password = "password"
            };
            var result = await controller.LoginAsync(userDoesNotExist);

            Assert.IsInstanceOfType<UnauthorizedResult>(result);
        }

        [TestMethod]
        public async Task LoginAsync_UserExistAndPasswordIsCorrect_ReturnsOk()
        {
            var controller = BuildController();

            var registerRequest = new RegisterViewModel
            {
                Username = "username",
                Password = "password",
                ConfirmPassword = "password"
            };
            await controller.RegisterAsync(registerRequest);

            var loginRequest = new LoginViewModel
            {
                Username = registerRequest.Username,
                Password = registerRequest.Password
            };
            var result = await controller.LoginAsync(loginRequest);

            Assert.IsInstanceOfType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType<string>(okResult.Value);
        }

        private static AccountController BuildController()
        {
            var factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var migrator = new Migrator(factory, typeof(Migration001).Assembly);
            migrator.Run();

            var userRepository = new UserRepository(factory);
            var passwordService = new ProtectPasswordService();
            var tokenService = new TokenService("ThisIsAVeryVeryVeryVeryVerySecureSecret");

            var registerService = new RegisterService(userRepository, passwordService);
            var loginService = new LoginService(userRepository, passwordService, tokenService);

            return new AccountController(registerService, loginService);
        }
    }
}