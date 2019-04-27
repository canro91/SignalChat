namespace SignalChat.Core
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProtectPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginService(IUserRepository userRepository, IProtectPasswordService passwordService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public string Login(string username, string plainTextPassword)
        {
            var user = _userRepository.FindUserByUsername(username);
            if (user != null)
            {
                var isVerified = _passwordService.VerifyPassword(plainTextPassword, user.SaltedPassword);
                if (isVerified)
                {
                    var token = _tokenService.CreateToken(username);
                    return token;
                }
            }

            return null;
        }
    }
}
