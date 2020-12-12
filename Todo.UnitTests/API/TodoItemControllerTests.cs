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
    public class TodoItemControllerTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<ITodoItemLogic> todoItemLogic;
        private readonly Mock<IMapper> mapper;
        private readonly TodoItemController controller;

        public TodoItemControllerTests()
        {
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            todoItemLogic = new Mock<ITodoItemLogic>();
            mapper = new Mock<IMapper>();
            SetupHttpContext();
            controller = new TodoItemController(httpContextAccessor.Object, todoItemLogic.Object, mapper.Object);
        }

        [Fact]
        public void Get_ShouldCallGetItems()
        {
            // Arrange
            var input = new Todo.Core.Models.Request.PagingParameters();

            // Act
            controller.Get(input);

            // Assert
            todoItemLogic.Verify(u => u.GetItems(1, It.Is<Todo.Core.Models.Request.PagingParameters>(c => c == input)));
        }

        [Fact]
        public void Get_ShouldReturnBadRequestWhenGetItemsReturnsNull()
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
        public void Get_ShouldReturnTodoItems()
        {
            // Arrange
            var input = new Todo.Core.Models.Request.PagingParameters();
            var model = new PagedResult<TodoItemDto>();
            todoItemLogic.Setup(u => u.GetItems(1, It.Is<Todo.Core.Models.Request.PagingParameters>(c => c == input))).Returns(model);

            // Act
            var result = controller.Get(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<PagedResult<TodoItemDto>>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void GetSingle_ShouldCallGetItem()
        {
            // Act
            controller.GetSingle(3);

            // Assert
            todoItemLogic.Verify(u => u.GetItem(1, 3));
        }

        [Fact]
        public void GetSingle_ShouldReturnBadRequestWhenGetItemReturnsNull()
        {
            // Act
            var result = controller.GetSingle(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or item not found in the database.", response.Message);
        }

        [Fact]
        public void GetSingle_ShouldReturnTodoItem()
        {
            // Arrange
            var model = new TodoItemDto();
            todoItemLogic.Setup(u => u.GetItem(1, 3)).Returns(model);

            // Act
            var result = controller.GetSingle(3);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<TodoItemDto>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void Create_ShouldCallCreateItem()
        {
            // Arrange
            var input = new CreateTodoItemDto();

            // Act
            controller.Create(input);

            // Assert
            todoItemLogic.Verify(u => u.CreateItem(1, It.Is<CreateTodoItemDto>(c => c == input)));
        }

        [Fact]
        public void Create_ShouldReturnBadRequestWhenCreateItemReturnsNull()
        {
            // Arrange
            var input = new CreateTodoItemDto();

            // Act
            var result = controller.Create(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or list not found in the database.", response.Message);
        }

        [Fact]
        public void Create_ShouldReturnCreatedItem()
        {
            // Arrange
            var input = new CreateTodoItemDto();
            var model = new TodoItemDto();
            todoItemLogic.Setup(u => u.CreateItem(1, It.Is<CreateTodoItemDto>(c => c == input))).Returns(model);

            // Act
            var result = controller.Create(input);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Update_ShouldCallUpdateItem()
        {
            // Arrange
            var input = new UpdateTodoItemDto();
            var mappedinput = new TodoItemDto();
            mapper.Setup(m => m.Map<TodoItemDto>(input)).Returns(mappedinput);

            // Act
            controller.Update(input);

            // Assert
            todoItemLogic.Verify(u => u.UpdateItem(1, It.Is<TodoItemDto>(c => c == mappedinput)));
        }

        [Fact]
        public void Update_ShouldReturnBadRequestWhenUpdateItemReturnsNull()
        {
            // Arrange
            var input = new UpdateTodoItemDto();
            mapper.Setup(m => m.Map<TodoItemDto>(input)).Returns(new TodoItemDto());

            // Act
            var result = controller.Update(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("Item not found in the database.", response.Message);
        }

        [Fact]
        public void Update_ShouldReturnUpdatedTodoItem()
        {
            // Arrange
            var input = new UpdateTodoItemDto();
            var mappedinput = new TodoItemDto();
            var model = new TodoItemDto();
            mapper.Setup(m => m.Map<TodoItemDto>(input)).Returns(mappedinput);
            todoItemLogic.Setup(u => u.UpdateItem(1, It.Is<TodoItemDto>(c => c == mappedinput))).Returns(model);

            // Act
            var result = controller.Update(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<TodoItemDto>;
            Assert.Equal(model, response.Model);
        }

        [Fact]
        public void Delete_ShouldCallDeleteItem()
        {
            // Act
            controller.Delete(3);

            // Assert
            todoItemLogic.Verify(u => u.DeleteItem(1, 3));
        }

        [Fact]
        public void Delete_ShouldReturnBadRequestWhenDeleteItemReturnsFalse()
        {
            // Act
            var result = controller.Delete(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("Item not found in the database.", response.Message);
        }

        [Fact]
        public void Delete_ShouldReturnTrueOnSuccessfulDeletion()
        {
            // Arrange
            todoItemLogic.Setup(u => u.DeleteItem(1, 3)).Returns(true);

            // Act
            var result = controller.Delete(3);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.True(response.Status);
            Assert.Equal("Todo item with id 3 deleted.", response.Model);
        }

        [Fact]
        public void GetLabels_ShouldCallGetItemLabels()
        {
            // Act
            controller.GetLabels(3);

            // Assert
            todoItemLogic.Verify(u => u.GetItemLabels(1, 3));
        }

        [Fact]
        public void GetLabels_ShouldReturnBadRequestWhenGetItemLabelsReturnsNull()
        {
            // Act
            var result = controller.GetLabels(3);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var response = (result as BadRequestObjectResult).Value as ErrorResponse;
            Assert.Equal("User or item not found in the database.", response.Message);
        }

        [Fact]
        public void GetLabels_ShouldReturnLabels()
        {
            // Arrange
            var model = new List<LabelDto>();
            todoItemLogic.Setup(u => u.GetItemLabels(1, 3)).Returns(model);

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
            todoItemLogic.Verify(u => u.CreateLabel(1, It.Is<CreateOrDeleteLabelDto>(c => c == input)));
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
            Assert.Equal("User or item not found in the database.", response.Message);
        }

        [Fact]
        public void CreateLabel_ShouldReturnCreatedLabel()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();
            var model = new LabelDto();
            todoItemLogic.Setup(u => u.CreateLabel(1, It.Is<CreateOrDeleteLabelDto>(c => c == input))).Returns(model);

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
            todoItemLogic.Verify(u => u.UpdateLabel(1, It.Is<UpdateLabelDto>(c => c == input)));
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
            Assert.Equal("Item or label not found in the database.", response.Message);
        }

        [Fact]
        public void UpdateLabel_ShouldReturnUpdatedLabel()
        {
            // Arrange
            var input = new UpdateLabelDto();
            var model = new LabelDto();
            todoItemLogic.Setup(u => u.UpdateLabel(1, It.Is<UpdateLabelDto>(c => c == input))).Returns(model);

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
            todoItemLogic.Verify(u => u.DeleteLabel(1, input));
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
            Assert.Equal("Item or label not found in the database.", response.Message);
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
            todoItemLogic.Setup(u => u.DeleteLabel(1, input)).Returns(true);

            // Act
            var result = controller.DeleteLabel(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var response = (result as OkObjectResult).Value as Response<string>;
            Assert.True(response.Status);
            Assert.Equal("Label testlabel deleted for item with id 3.", response.Model);
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
