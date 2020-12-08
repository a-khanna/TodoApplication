using System;
using System.Threading.Tasks;
using Todo.Core.Models.Dtos;

namespace Todo.Core.Abstractions.Logic
{
    /// <summary>
    /// Logic layer for Users
    /// </summary>
    public interface IUserLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto">User DTO</param>
        /// <returns>True if registered successfully, else false</returns>
        /// <exception cref="ArgumentNullException">If the userDto is null</exception>
        Task<bool> RegisterUser(UserDto userDto);
    }
}
