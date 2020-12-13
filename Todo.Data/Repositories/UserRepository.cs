using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Abstractions.Data;
using Todo.Core.Models.Sql;

namespace Todo.Data.Repositories
{
    /// <summary>
    /// Repository for handling User entities
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly TodoDbContext dbContext;

        public UserRepository(TodoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets user entity by username.
        /// Returns user entity. Returns null if no user is found.
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>User entity</returns>
        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username cannot be null or white space.");

            return dbContext.Users.FirstOrDefault(u => u.Username == username);
        }

        /// <summary>
        /// Creates a user in the database
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="salt">Salt byte array</param>
        /// <param name="hash">Hash byte array</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name - Optional</param>
        /// <returns>True if created, else false</returns>
        public async Task<bool> CreateUser(string username, byte[] salt, byte[] hash, string firstName, string lastName = null)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                FirstName = firstName,
                LastName = lastName
            };

            var userExists = dbContext.Users.Any(u => u.Username == username);

            if (userExists)
                return false;

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            
            return true;
        }
    }
}
