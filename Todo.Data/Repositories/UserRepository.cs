using System;
using System.Linq;
using System.Threading.Tasks;
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
        /// Gets user entity by username
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns></returns>
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

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            
            return true;
        }
    }
}
