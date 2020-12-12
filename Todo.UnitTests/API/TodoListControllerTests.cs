using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.API.Controllers.v1;
using Todo.Core;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Response;
using Xunit;

namespace Todo.UnitTests.API
{
    public class TodoListControllerTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<ITodoListLogic> todoListLogic;
        private readonly Mock<IMapper> mapper;
        private readonly TodoListController controller;

        public TodoListControllerTests()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            todoListLogic = new Mock<ITodoListLogic>();
            mapper = new Mock<IMapper>();
            SetupHttpContext();
            controller = new TodoListController(httpContextAccessor.Object, todoListLogic.Object, mapper.Object);
        }

        [Fact]
        public void Get_ShouldCallGetLists()
        {
            // Arrange
            var input = new Todo.Core.Models.Request.PagingParameters();

            // Act
            controller.Get(input);

            // Assert
            todoListLogic.Verify(u => u.GetLists(1, It.Is<Todo.Core.Models.Request.PagingParameters>(c => c == input)));
        }

        [Fact]
        public void Get_ShouldReturnBadRequestWhenGetListsReturnsNull()
        {
            // Arrange
            var input = new Todo.Core.Models.Request.PagingParameters();

            // Act
            var result = controller.Get(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User not found in the database.", response.Message);
        }

        [Fact]
        public void Get_ShouldReturnTodoLists()
        {
            // Arrange
            var input = new Todo.Core.Models.Request.PagingParameters();
            var model = new PagedResult<TodoListDto>();
            todoListLogic.Setup(u => u.GetLists(1, It.Is<Todo.Core.Models.Request.PagingParameters>(c => c == input))).Returns(model);

            // Act
            var result = controller.Get(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<PagedResult<TodoListDto>>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void GetSingle_ShouldCallGetList()
        {
            // Act
            controller.GetSingle(3);

            // Assert
            todoListLogic.Verify(u => u.GetList(1, 3));
        }

        [Fact]
        public void GetSingle_ShouldReturnBadRequestWhenGetListReturnsNull()
        {
            // Act
            var result = controller.GetSingle(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or list not found in the database.", response.Message);
        }

        [Fact]
        public void GetSingle_ShouldReturnTodoList()
        {
            // Arrange
            var model = new TodoListDetailedDto();
            todoListLogic.Setup(u => u.GetList(1, 3)).Returns(model);

            // Act
            var result = controller.GetSingle(3);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<TodoListDetailedDto>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void Create_ShouldCallCreateList()
        {
            // Arrange
            var input = new CreateTodoListDto();

            // Act
            controller.Create(input);

            // Assert
            todoListLogic.Verify(u => u.CreateList(1, It.Is<CreateTodoListDto>(c => c == input)));
        }

        [Fact]
        public void Create_ShouldReturnBadRequestWhenCreateListReturnsNull()
        {
            // Arrange
            var input = new CreateTodoListDto();

            // Act
            var result = controller.Create(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User not found in the database.", response.Message);
        }

        [Fact]
        public void Create_ShouldReturnCreatedList()
        {
            // Arrange
            var input = new CreateTodoListDto();
            var model = new TodoListDto();
            todoListLogic.Setup(u => u.CreateList(1, It.Is<CreateTodoListDto>(c => c == input))).Returns(model);

            // Act
            var result = controller.Create(input);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Update_ShouldCallUpdateList()
        {
            // Arrange
            var input = new UpdateTodoListDto();
            var mappedinput = new TodoListDto();
            mapper.Setup(m => m.Map<TodoListDto>(input)).Returns(mappedinput);

            // Act
            controller.Update(input);

            // Assert
            todoListLogic.Verify(u => u.UpdateList(1, It.Is<TodoListDto>(c => c == mappedinput)));
        }

        [Fact]
        public void Update_ShouldReturnBadRequestWhenUpdateListReturnsNull()
        {
            // Arrange
            var input = new UpdateTodoListDto();
            mapper.Setup(m => m.Map<TodoListDto>(input)).Returns(new TodoListDto());

            // Act
            var result = controller.Update(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("List not found in the database.", response.Message);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedTodoList()
        {
            // Arrange
            var input = new UpdateTodoListDto();
            var mappedinput = new TodoListDto();
            var model = new TodoListDto();
            mapper.Setup(m => m.Map<TodoListDto>(input)).Returns(mappedinput);
            todoListLogic.Setup(u => u.UpdateList(1, It.Is<TodoListDto>(c => c == mappedinput))).Returns(model);

            // Act
            var result = controller.Update(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<TodoListDto>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void Delete_ShouldCallDeleteList()
        {
            // Act
            controller.Delete(3);

            // Assert
            todoListLogic.Verify(u => u.DeleteList(1, 3));
        }

        [Fact]
        public void Delete_ShouldReturnBadRequestWhenDeleteListReturnsFalse()
        {
            // Act
            var result = controller.Delete(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("List not found in the database.", response.Message);
        }

        [Fact]
        public void Delete_ShouldReturnTrueOnSuccessfulDeletion()
        {
            // Arrange
            todoListLogic.Setup(u => u.DeleteList(1, 3)).Returns(true);

            // Act
            var result = controller.Delete(3);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.True(response.Status);
            Assert.Equal("Todo List with id 3 deleted.", response.Model);
        }

        [Fact]
        public void GetLabels_ShouldCallGetListLabels()
        {
            // Act
            controller.GetLabels(3);

            // Assert
            todoListLogic.Verify(u => u.GetListLabels(1, 3));
        }

        [Fact]
        public void GetLabels_ShouldReturnBadRequestWhenGetListLabelsReturnsNull()
        {
            // Act
            var result = controller.GetLabels(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or list not found in the database.", response.Message);
        }

        [Fact]
        public void GetLabels_ShouldReturnLabels()
        {
            // Arrange
            var model = new List<LabelDto>();
            todoListLogic.Setup(u => u.GetListLabels(1, 3)).Returns(model);

            // Act
            var result = controller.GetLabels(3);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<List<LabelDto>>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void CreateLabel_ShouldCallCreateLabel()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

            // Act
            controller.CreateLabel(input);

            // Assert
            todoListLogic.Verify(u => u.CreateLabel(1, It.Is<CreateOrDeleteLabelDto>(c => c == input)));
        }

        [Fact]
        public void CreateLabel_ShouldReturnBadRequestWhenCreateLabelReturnsNull()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

            // Act
            var result = controller.CreateLabel(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or list not found in the database.", response.Message);
        }

        [Fact]
        public void CreateLabel_ShouldReturnCreatedLabel()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();
            var model = new LabelDto();
            todoListLogic.Setup(u => u.CreateLabel(1, It.Is<CreateOrDeleteLabelDto>(c => c == input))).Returns(model);

            // Act
            var result = controller.CreateLabel(input);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void UpdateLabel_ShouldCallUpdateLabel()
        {
            // Arrange
            var input = new UpdateLabelDto();

            // Act
            controller.UpdateLabel(input);

            // Assert
            todoListLogic.Verify(u => u.UpdateLabel(1, It.Is<UpdateLabelDto>(c => c == input)));
        }

        [Fact]
        public void UpdateLabel_ShouldReturnBadRequestWhenUpdateLabelReturnsNull()
        {
            // Arrange
            var input = new UpdateLabelDto();

            // Act
            var result = controller.UpdateLabel(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("List or label not found in the database.", response.Message);
        }

        [Fact]
        public void UpdateLabel_ShouldReturnUpdatedLabel()
        {
            // Arrange
            var input = new UpdateLabelDto();
            var model = new LabelDto();
            todoListLogic.Setup(u => u.UpdateLabel(1, It.Is<UpdateLabelDto>(c => c == input))).Returns(model);

            // Act
            var result = controller.UpdateLabel(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<LabelDto>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void DeleteLabel_ShouldCallDeleteLabel()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

            // Act
            controller.DeleteLabel(input);

            // Assert
            todoListLogic.Verify(u => u.DeleteLabel(1, input));
        }

        [Fact]
        public void DeleteLabel_ShouldReturnBadRequestWhenDeleteLabelReturnsFalse()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

            // Act
            var result = controller.DeleteLabel(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("List or label not found in the database.", response.Message);
        }

        [Fact]
        public void DeleteLabel_ShouldReturnTrueOnSuccessfulDeletion()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto
            {
                ParentId = 3,
                Label = "testlabel"
            };
            todoListLogic.Setup(u => u.DeleteLabel(1, input)).Returns(true);

            // Act
            var result = controller.DeleteLabel(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.True(response.Status);
            Assert.Equal("Label testlabel deleted for list with id 3.", response.Model);
        }

        private void SetupHttpContext()
        {
            var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim(Constants.UserIdClaim, "1") }) });
            var context = new Mock<HttpContext>();
            context.SetupGet(c => c.User).Returns(claimsPrincipal);
            httpContextAccessor.SetupGet(h => h.HttpContext).Returns(context.Object);
        }
    }
}
