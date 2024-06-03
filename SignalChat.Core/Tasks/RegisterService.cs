using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;

namespace SignalChat.Core.Tasks
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProtectPasswordService _passwordService;

        public RegisterService(IUserRepository userRepository, IProtectPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task RegisterUserAsync(string username, string plainTextPassword)
        {
            var existingUser = await _userRepository.FindUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new ArgumentException($"User {username} already exists");
            }

            var saltedPassword = _passwordService.ProtectPassword(plainTextPassword);
            var newUser = new User
            {
                Username = username,
                SaltedPassword = saltedPassword
            };
            await _userRepository.SaveAsync(newUser);
        }
    }
}
