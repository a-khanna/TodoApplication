using System.Threading.Tasks;
using Todo.Core.Models.Sql;

namespace Todo.Core.Abstractions.Data
{
    /// <summary>
    /// Repository for handling User entities
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets user entity by username.
        /// Returns user entity. Returns null if no user is found.
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>User entity</returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Creates a user in the database
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="salt">Salt byte array</param>
        /// <param name="hash">Hash byte array</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name - Optional</param>
        /// <returns>True if created, else false</returns>
        Task<bool> CreateUser(string username, byte[] salt, byte[] hash, string firstName, string lastName = null);
    }
}
