using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalChat.Core.Contracts;
using SignalChat.Models;

namespace SignalChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public AccountController(IRegisterService registerService,
                                 ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterViewModel loginViewModel)
        {
            _registerService.RegisterUser(username: loginViewModel.Username,
                                          plainTextPassword: loginViewModel.Password);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            var token = _loginService.Login(username: loginViewModel.Username,
                                            plainTextPassword: loginViewModel.Password);
            return token == null
                ? Unauthorized()
                : Ok(token);
        }
    }
}
