using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.API.Controllers.v1;
using Todo.Core.Abstractions.Logic;
using Xunit;

namespace Todo.UnitTests.API
{
    public class UserControllerTests
    {
        private readonly Mock<IUserLogic> userLogic;

        public UserControllerTests()
        {
            userLogic = new Mock<IUserLogic>();
        }

        [Fact]
        public void Authenticate_ShouldCallVerifyLogin()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.CredentialsDto();

            // Act
            controller.Authenticate(input);

            // Assert
            userLogic.Verify(u => u.VerifyLogin(It.Is<Todo.Core.Models.Dtos.CredentialsDto>(c => c == input)));
        }

        [Fact]
        public void Authenticate_ShouldReturnBadRequestForIncorrectLogin()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.CredentialsDto();

            // Act
            var result = controller.Authenticate(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Authenticate_ShouldReturnOkResultForCorrectLogin()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.CredentialsDto();
            userLogic.Setup(u => u.VerifyLogin(It.Is<Todo.Core.Models.Dtos.CredentialsDto>(c => c == input))).Returns("token");

            // Act
            var result = controller.Authenticate(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
