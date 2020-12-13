using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Helpers;
using Todo.Data;
using Todo.Data.Repositories;
using Xunit;

namespace Todo.UnitTests.Data
{
    public class UserRepositoryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetUserByUsername_ShouldThrowExceptionForNullOrWhiteSpaceUsername(string username)
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetUserByUsername_ShouldThrowExceptionForNullOrWhiteSpaceUsername));
            var userRepository = new UserRepository(dbContext);

            // Act
            var exception = Record.Exception(() => userRepository.GetUserByUsername(username));

            // Assert
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("username cannot be null or white space.", exception.Message);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnUserWithTheGivenUsername()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetUserByUsername_ShouldReturnUserWithTheGivenUsername));
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetUserByUsername("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("name1", result.FirstName);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnNullIfUsernameIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetUserByUsername_ShouldReturnNullIfUsernameIsNotFound));
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetUserByUsername("random");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ShouldNotAddDuplicateUserToTheDatabase()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateUser_ShouldNotAddDuplicateUserToTheDatabase));
            var userRepository = new UserRepository(dbContext);

            // Arrange
            var saltAndHash = CryptographyHelper.GetSaltAndHash("4321");

            // Act
            var result = await userRepository.CreateUser("testuser", saltAndHash.Item1, saltAndHash.Item2, "name2");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUser_ShouldAddUserToTheDatabase()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateUser_ShouldAddUserToTheDatabase));
            var userRepository = new UserRepository(dbContext);

            // Arrange
            var saltAndHash = CryptographyHelper.GetSaltAndHash("4321");

            // Act
            var result = await userRepository.CreateUser("testuser2", saltAndHash.Item1, saltAndHash.Item2, "name2");

            // Assert
            Assert.Equal(2, dbContext.Users.Count());
            Assert.True(result);
        }

        private TodoDbContext SetupDatabase(string dbname)
        {
            var builder = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: dbname);

            var dbContext = new TodoDbContext(builder.Options);

            var saltAndHash = CryptographyHelper.GetSaltAndHash("1234");
            dbContext.Users.Add(new Todo.Core.Models.Sql.User
            {
                Username = "testuser",
                PasswordSalt = saltAndHash.Item1,
                PasswordHash = saltAndHash.Item2,
                FirstName = "name1"
            });

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
