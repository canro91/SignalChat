﻿using System;

namespace SignalChat.Core
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

        public void RegisterUser(string username, string plainTextPassword)
        {
            var existingUser = _userRepository.FindUserByUsername(username);
            if (existingUser != null)
                throw new ArgumentException($"User {username} already exists");

            var saltedPassword = _passwordService.ProtectPassword(plainTextPassword);
            var newUser = new User
            {
                ID = Guid.NewGuid(),
                Username = username,
                SaltedPassword = saltedPassword
            };
            _userRepository.Save(newUser);
        }
    }
}
