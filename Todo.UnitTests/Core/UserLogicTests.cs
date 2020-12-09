using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Todo.Core.Abstractions.Data;
using Todo.Core.Helpers;
using Todo.Core.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Sql;
using Xunit;

namespace Todo.UnitTests.Core
{
    public class UserLogicTests
    {
        private readonly Mock<IUserRepository> userRepository;
        private readonly IConfiguration configuration;
        private readonly UserLogic userLogic;

        public UserLogicTests()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { Todo.Core.Constants.AppSettingsJwtKey, "key12345623456789876543" }, { Todo.Core.Constants.AppSettingsJwtIssuer, "issuer" } })
                .Build();
            userRepository = new Mock<IUserRepository>();
            userLogic = new UserLogic(configuration, userRepository.Object);
        }

        [Fact]
        public void VerifyLogin_ShouldThrowExceptionForNullCredentials()
        {
            // Act
            var exception = Record.Exception(() => userLogic.VerifyLogin(null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'credentials')", exception.Message);
        }

        [Fact]
        public void VerifyLogin_ShouldReturnNullIfUserIsNotFoundInDatabase()
        {
            // Arrange
            var input = new CredentialsDto
            {
                Username = "testuser",
                Password = "password"
            };

            // Act
            var result = userLogic.VerifyLogin(input);

            // Assert
            Assert.Null(result);           
        }

        [Fact]
        public void VerifyLogin_ShouldReturnNullIfPasswordDoesNotMatch()
        {
            // Arrange
            var saltAndHash = CryptographyHelper.GetSaltAndHash("1234");
            var input = new CredentialsDto
            {
                Username = "testuser",
                Password = "password"
            };
            var dbObject = new User
            {
                Id = 1,
                Username = "testuser",
                PasswordSalt = saltAndHash.Item1,
                PasswordHash = saltAndHash.Item2
            };
            userRepository.Setup(u => u.GetUserByUsername(It.Is<string>(s => s == input.Username))).Returns(dbObject);

            // Act
            var result = userLogic.VerifyLogin(input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void VerifyLogin_ShouldReturnJWTIfPasswordMatches()
        {
            // Arrange
            var saltAndHash = CryptographyHelper.GetSaltAndHash("password");
            var input = new CredentialsDto
            {
                Username = "testuser",
                Password = "password"
            };
            var dbObject = new User
            {
                Id = 1,
                Username = "testuser",
                PasswordSalt = saltAndHash.Item1,
                PasswordHash = saltAndHash.Item2
            };
            userRepository.Setup(u => u.GetUserByUsername(It.Is<string>(s => s == input.Username))).Returns(dbObject);

            // Act
            var result = userLogic.VerifyLogin(input);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowExceptionForNullUserDto()
        {
            // Act
            var exception = await Record.ExceptionAsync(() => userLogic.RegisterUser(null));

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("Value cannot be null. (Parameter 'userDto')", exception.Message);
        }

        [Fact]
        public async Task RegisterUser_ShouldInvokeCreateUserAndReturnResult()
        {
            // Arrange
            var user = new UserDto
            {
                FirstName = "Anirudh",
                LastName = "Khanna",
                Username = "anirudh",
                Password = "1234"
            };

            userRepository.Setup(u =>
                u.CreateUser(
                    It.Is<string>(s => s == user.Username),
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>(),
                    It.Is<string>(s => s == user.FirstName),
                    It.Is<string>(s => s == user.LastName))).ReturnsAsync(true);

            // Act
            var result = await userLogic.RegisterUser(user);

            // Assert
            userRepository.Verify(u => u.CreateUser("anirudh", It.IsAny<byte[]>(), It.IsAny<byte[]>(), "Anirudh", "Khanna"));
            Assert.True(result);
        }
    }
}
