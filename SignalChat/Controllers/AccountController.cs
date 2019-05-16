using SignalChat.Core.Contracts;
using SignalChat.Filters;
using SignalChat.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalChat.Controllers
{
    [Authorize]
    [ValidateModel]
    public class AccountController : ApiController
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public AccountController(IRegisterService registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Register([FromBody] RegisterViewModel loginViewModel)
        {
            _registerService.RegisterUser(username: loginViewModel.Username,
                                          plainTextPassword: loginViewModel.Password);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            var token = _loginService.Login(username: loginViewModel.Username, plainTextPassword: loginViewModel.Password);
            return ReplyWith(token);
        }

        private IHttpActionResult ReplyWith(string token)
        {
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
