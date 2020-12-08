using System;
using System.Threading.Tasks;
using Moq;
using Todo.Core.Abstractions.Data;
using Todo.Core.Logic;
using Todo.Core.Models.Dtos;
using Xunit;

namespace Todo.UnitTests.Core
{
    public class UserLogicTests
    {
        private readonly Mock<IUserRepository> userRepository;
        private readonly UserLogic userLogic;

        public UserLogicTests()
        {
            userRepository = new Mock<IUserRepository>();
            userLogic = new UserLogic(userRepository.Object);
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
