using System;
using System.Threading.Tasks;
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
        private readonly IUserRepository userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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
