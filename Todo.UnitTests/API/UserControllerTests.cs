using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.API.Controllers.v1;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Response;
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
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("username or password is incorrect", response.Message);
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
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.Equal("Successfully authenticated.", response.Message);
        }

        [Fact]
        public async Task Register_ShouldCallRegisterUser()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.UserDto();

            // Act
            await controller.Register(input);

            // Assert
            userLogic.Verify(u => u.RegisterUser(It.Is<Todo.Core.Models.Dtos.UserDto>(c => c == input)));
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequestForIncorrectLogin()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.UserDto();

            // Act
            var result = await controller.Register(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User already exists.", response.Message);
        }

        [Fact]
        public async Task Register_ShouldReturnOkResultForCorrectLogin()
        {
            // Arrange
            var controller = new UserController(userLogic.Object);
            var input = new Todo.Core.Models.Dtos.UserDto();
            userLogic.Setup(u => u.RegisterUser(It.Is<Todo.Core.Models.Dtos.UserDto>(c => c == input))).ReturnsAsync(true);

            // Act
            var result = await controller.Register(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.Equal("Successfully registered user.", response.Message);
        }
    }
}
