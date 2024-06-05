using Microsoft.AspNetCore.Mvc;
using SignalChat.Core.Contracts;
using SignalChat.Models;

namespace SignalChat.Controllers;

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
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel loginViewModel)
    {
        await _registerService.RegisterUserAsync(
            username: loginViewModel.Username!,
            plainTextPassword: loginViewModel.Password!);

        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel)
    {
        var token = await _loginService.LoginAsync(
            username: loginViewModel.Username!,
            plainTextPassword: loginViewModel.Password!);

        return token == null
            ? Unauthorized()
            : Ok(token);
    }
}