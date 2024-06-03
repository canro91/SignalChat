using SignalChat.Core.Contracts;

namespace SignalChat.Core.Tasks
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProtectPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginService(IUserRepository userRepository,
                            IProtectPasswordService passwordService,
                            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<string?> LoginAsync(string username, string plainTextPassword)
        {
            var user = await _userRepository.FindUserByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            var isVerified = _passwordService.VerifyPassword(plainTextPassword, user.SaltedPassword);
            if (isVerified)
            {
                var token = _tokenService.CreateToken(username);
                return token;
            }

            return null;
        }
    }
}
