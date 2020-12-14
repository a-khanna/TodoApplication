using System.Collections.Generic;
using AutoMapper;
using Moq;
using Todo.Core.Abstractions.Data;
using Todo.Core.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;
using Xunit;

namespace Todo.UnitTests.Core
{
    public class TodoItemLogicTests
    {
        private readonly Mock<ITodoItemRepository> todoItemRepository;
        private readonly Mock<IMapper> mapper;
        private readonly TodoItemLogic logic;

        public TodoItemLogicTests()
        {
            todoItemRepository = new Mock<ITodoItemRepository>();
            mapper = new Mock<IMapper>();
            logic = new TodoItemLogic(todoItemRepository.Object, mapper.Object);
        }

        [Fact]
        public void GetItems_ShouldCallGetItems()
        {
            // Arrange
            var input = new PagingParameters();

            // Act
            logic.GetItems(1, input);

            // Assert
            todoItemRepository.Verify(t => t.GetItems(1, input));
        }

        [Fact]
        public void GetItems_ShouldReturnNullIfRepositoryReturnsNull()
        {
            // Arrange
            var input = new PagingParameters();

            // Act
            var result = logic.GetItems(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetItems_ShouldReturnTodoItems()
        {
            // Arrange
            var input = new PagingParameters();
            var model = new PagedResult<TodoItemDto>();
            todoItemRepository.Setup(t => t.GetItems(1, input)).Returns(new PagedResult<Todo.Core.Models.Sql.TodoItem>());
            mapper.Setup(m => m.Map<PagedResult<TodoItemDto>>(It.IsAny<object>())).Returns(model);

            // Act
            var result = logic.GetItems(1, input);

            // Assert
            Assert.Equal(model, result);
        }

        [Fact]
        public void GetItem_ShouldCallGetItem()
        {
            // Act
            logic.GetItem(1, 2);

            // Assert
            todoItemRepository.Verify(t => t.GetItem(1, 2));
        }

        [Fact]
        public void GetItem_ShouldMapResultToDto()
        {
            // Act
            logic.GetItem(1, 2);

            // Assert
            mapper.Verify(t => t.Map<TodoItemDto>(It.IsAny<TodoItem>()));
        }

        [Fact]
        public void CreateItem_ShouldCallCreateItem()
        {
            // Arrange
            var input = new CreateTodoItemDto();

            // Act
            logic.CreateItem(1, input);

            // Assert
            todoItemRepository.Verify(t => t.CreateItem(1, input));
        }

        [Fact]
        public void CreateItem_ShouldMapResultToDto()
        {
            // Arrange
            var input = new CreateTodoItemDto();

            // Act
            logic.CreateItem(1, input);

            // Assert
            mapper.Verify(t => t.Map<TodoItemDto>(It.IsAny<TodoItem>()));
        }

        [Fact]
        public void UpdateItem_ShouldCallUpdateItem()
        {
            // Arrange
            var input = new TodoItemDto();

            // Act
            logic.UpdateItem(1, input);

            // Assert
            todoItemRepository.Verify(t => t.UpdateItem(1, input));
        }

        [Fact]
        public void UpdateItem_ShouldMapResultToDto()
        {
            // Arrange
            var input = new TodoItemDto();

            // Act
            logic.UpdateItem(1, input);

            // Assert
            mapper.Verify(t => t.Map<TodoItemDto>(It.IsAny<TodoItem>()));
        }

        [Fact]
        public void DeleteItem_ShouldCallDeleteItem()
        {
            // Act
            logic.DeleteItem(1, 2);

            // Assert
            todoItemRepository.Verify(t => t.DeleteItem(1, 2));
        }

        [Fact]
        public void GetLabels_ShouldCallGetItemLabels()
        {
            // Act
            logic.GetItemLabels(1, 2);

            // Assert
            todoItemRepository.Verify(t => t.GetItemLabels(1, 2));
        }

        [Fact]
        public void GetLabels_ShouldReturnNullIfRepositoryReturnsNull()
        {
            // Act
            var result = logic.GetItemLabels(1, 2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLabels_ShouldReturnLabels()
        {
            // Arrange
            var model = new List<LabelDto>();
            todoItemRepository.Setup(t => t.GetItemLabels(1, 2)).Returns(new List<Label>());
            mapper.Setup(m => m.Map<List<LabelDto>>(It.IsAny<object>())).Returns(model);

            // Act
            var result = logic.GetItemLabels(1, 2);

            // Assert
            Assert.Equal(model, result);
        }

        [Fact]
        public void CreateLabel_ShouldCallCreateLabel()
        {
            // Arrange
            var input = new CreateLabelDto();

            // Act
            logic.CreateLabel(1, input);

            // Assert
            todoItemRepository.Verify(t => t.CreateLabel(1, input));
        }

        [Fact]
        public void CreateLabel_ShouldMapResultToDto()
        {
            // Arrange
            var input = new CreateLabelDto();

            // Act
            logic.CreateLabel(1, input);

            // Assert
            mapper.Verify(t => t.Map<LabelDto>(It.IsAny<Label>()));
        }

        [Fact]
        public void UpdateLabel_ShouldCallUpdateLabel()
        {
            // Arrange
            var input = new UpdateLabelDto();

            // Act
            logic.UpdateLabel(1, input);

            // Assert
            todoItemRepository.Verify(t => t.UpdateLabel(1, input));
        }

        [Fact]
        public void UpdateLabel_ShouldMapResultToDto()
        {
            // Arrange
            var input = new UpdateLabelDto();

            // Act
            logic.UpdateLabel(1, input);

            // Assert
            mapper.Verify(t => t.Map<LabelDto>(It.IsAny<Label>()));
        }

        [Fact]
        public void DeleteLabel_ShouldCallDeleteLabel()
        {
            // Arrange
            var input = new DeleteLabelDto();

            // Act
            logic.DeleteLabel(1, input);

            // Assert
            todoItemRepository.Verify(t => t.DeleteLabel(1, input));
        }
    }
}
