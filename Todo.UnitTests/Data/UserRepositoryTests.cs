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
        private readonly UserRepository userRepository;
        private TodoDbContext dbContext;

        public UserRepositoryTests()
        {
            SetupDatabase();
            userRepository = new UserRepository(dbContext);            
        }        

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetUserByUsername_ShouldThrowExceptionForNullOrWhiteSpaceUsername(string username)
        {
            // Act
            var exception = Record.Exception(() => userRepository.GetUserByUsername(username));

            // Assert
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("username cannot be null or white space.", exception.Message);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnUserWithTheGivenUsername()
        {
            // Act
            var result = userRepository.GetUserByUsername("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("name1", result.FirstName);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnNullIfUsernameIsNotFound()
        {
            // Act
            var result = userRepository.GetUserByUsername("random");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ShouldAddUserToTheDatabase()
        {
            // Arrange
            var saltAndHash = CryptographyHelper.GetSaltAndHash("4321");

            // Act
            var result = await userRepository.CreateUser("testuser2", saltAndHash.Item1, saltAndHash.Item2, "name2");

            // Assert
            Assert.Equal(2, dbContext.Users.Count());
            Assert.True(result);
        }

        private void SetupDatabase()
        {
            var builder = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(UserRepositoryTests));

            dbContext = new TodoDbContext(builder.Options);

            var saltAndHash = CryptographyHelper.GetSaltAndHash("1234");
            dbContext.Users.Add(new Todo.Core.Models.Sql.User
            {
                Username = "testuser",
                PasswordSalt = saltAndHash.Item1,
                PasswordHash = saltAndHash.Item2,
                FirstName = "name1"
            });

            dbContext.SaveChanges();
        }
    }
}
