using SignalChat.Core.Contracts;
using SignalChat.Core.Insfrastructure;
using SignalChat.Core.Tasks;
using SignalChat.Filters;
using SignalChat.Models;
using System.Configuration;
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

        public AccountController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            var userRepository = new UserRepository(connectionString);
            var passwordService = new ProtectPasswordService();

            _registerService = new RegisterService(userRepository, passwordService);

            var secret = ConfigurationManager.AppSettings["Secret"];
            var tokenService = new TokenService(secret);
            _loginService = new LoginService(userRepository, passwordService, tokenService);
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
