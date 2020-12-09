using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Todo.Core.Abstractions.Data;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Helpers;
using Todo.Core.Models.Dtos;

namespace Todo.Core.Logic
{
    /// <summary>
    /// Logic layer for Users
    /// </summary>
    public class UserLogic : IUserLogic
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public UserLogic(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Verifies the user's login credentials
        /// </summary>
        /// <param name="credentials">Username and password</param>
        /// <returns>JWT, if successful</returns>
        /// <exception cref="ArgumentNullException">If the credentials object is null</exception>
        public string VerifyLogin(CredentialsDto credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException("credentials");

            var userFromDb = userRepository.GetUserByUsername(credentials.Username);
            if (userFromDb == null)
                return null;

            var passwordIsCorrect = CryptographyHelper.CompareHashes(credentials.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt);
            if (!passwordIsCorrect)
                return null;

            var jwtKey = configuration.GetValue<string>(Constants.AppSettingsJwtKey);
            var jwtIssuer = configuration.GetValue<string>(Constants.AppSettingsJwtIssuer);
            return CryptographyHelper.GenerateJSONWebToken(jwtKey, jwtIssuer, userFromDb.Id.ToString(), userFromDb.Username);
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userDto">User DTO</param>
        /// <returns>True if registered successfully, else false</returns>
        /// <exception cref="ArgumentNullException">If the userDto is null</exception>
        public Task<bool> RegisterUser(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException("userDto");

            var saltAndHash = CryptographyHelper.GetSaltAndHash(userDto.Password);
            return userRepository.CreateUser(userDto.Username, saltAndHash.Item1, saltAndHash.Item2, userDto.FirstName, userDto.LastName);
        }
    }
}
