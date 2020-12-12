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
    public class TodoListLogicTests
    {
        private readonly Mock<ITodoListRepository> todoListRepository;
        private readonly Mock<IMapper> mapper;
        private readonly TodoListLogic logic;

        public TodoListLogicTests()
        {
            todoListRepository = new Mock<ITodoListRepository>();
            mapper = new Mock<IMapper>();
            logic = new TodoListLogic(todoListRepository.Object, mapper.Object);
        }

        [Fact]
        public void GetLists_ShouldCallGetLists()
        {
            // Arrange
            var input = new PagingParameters();

            // Act
            logic.GetLists(1, input);

            // Assert
            todoListRepository.Verify(t => t.GetLists(1, input));
        }

        [Fact]
        public void GetLists_ShouldReturnNullIfRepositoryReturnsNull()
        {
            // Arrange
            var input = new PagingParameters();

            // Act
            var result = logic.GetLists(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLists_ShouldReturnTodoLists()
        {
            // Arrange
            var input = new PagingParameters();
            var model = new PagedResult<TodoListDto>();
            todoListRepository.Setup(t => t.GetLists(1, input)).Returns(new PagedResult<Todo.Core.Models.Sql.TodoList>());
            mapper.Setup(m => m.Map<PagedResult<TodoListDto>>(It.IsAny<object>())).Returns(model);

            // Act
            var result = logic.GetLists(1, input);

            // Assert
            Assert.Equal(model, result);
        }

        [Fact]
        public void GetList_ShouldCallGetList()
        {
            // Act
            logic.GetList(1, 2);

            // Assert
            todoListRepository.Verify(t => t.GetList(1, 2));
        }

        [Fact]
        public void GetList_ShouldMapResultToDto()
        {
            // Act
            logic.GetList(1, 2);

            // Assert
            mapper.Verify(t => t.Map<TodoListDto>(It.IsAny<TodoList>()));
        }

        [Fact]
        public void CreateList_ShouldCallCreateList()
        {
            // Arrange
            var input = new CreateTodoListDto();

            // Act
            logic.CreateList(1, input);

            // Assert
            todoListRepository.Verify(t => t.CreateList(1, input));
        }

        [Fact]
        public void CreateList_ShouldMapResultToDto()
        {
            // Arrange
            var input = new CreateTodoListDto();

            // Act
            logic.CreateList(1, input);

            // Assert
            mapper.Verify(t => t.Map<TodoListDto>(It.IsAny<TodoList>()));
        }

        [Fact]
        public void UpdateList_ShouldCallUpdateList()
        {
            // Arrange
            var input = new TodoListDto();

            // Act
            logic.UpdateList(1, input);

            // Assert
            todoListRepository.Verify(t => t.UpdateList(1, input));
        }

        [Fact]
        public void UpdateList_ShouldMapResultToDto()
        {
            // Arrange
            var input = new TodoListDto();

            // Act
            logic.UpdateList(1, input);

            // Assert
            mapper.Verify(t => t.Map<TodoListDto>(It.IsAny<TodoList>()));
        }

        [Fact]
        public void DeleteList_ShouldCallDeleteList()
        {
            // Act
            logic.DeleteList(1, 2);

            // Assert
            todoListRepository.Verify(t => t.DeleteList(1, 2));
        }

        [Fact]
        public void GetLabels_ShouldCallGetListLabels()
        {
            // Act
            logic.GetListLabels(1, 2);

            // Assert
            todoListRepository.Verify(t => t.GetListLabels(1, 2));
        }

        [Fact]
        public void GetLabels_ShouldReturnNullIfRepositoryReturnsNull()
        {
            // Act
            var result = logic.GetListLabels(1, 2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLabels_ShouldReturnLabels()
        {
            // Arrange
            var model = new List<LabelDto>();
            todoListRepository.Setup(t => t.GetListLabels(1, 2)).Returns(new List<Label>());
            mapper.Setup(m => m.Map<List<LabelDto>>(It.IsAny<object>())).Returns(model);

            // Act
            var result = logic.GetListLabels(1, 2);

            // Assert
            Assert.Equal(model, result);
        }

        [Fact]
        public void CreateLabel_ShouldCallCreateLabel()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

            // Act
            logic.CreateLabel(1, input);

            // Assert
            todoListRepository.Verify(t => t.CreateLabel(1, input));
        }

        [Fact]
        public void CreateLabel_ShouldMapResultToDto()
        {
            // Arrange
            var input = new CreateOrDeleteLabelDto();

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
            todoListRepository.Verify(t => t.UpdateLabel(1, input));
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
            var input = new CreateOrDeleteLabelDto();

            // Act
            logic.DeleteLabel(1, input);

            // Assert
            todoListRepository.Verify(t => t.DeleteLabel(1, input));
        }
    }
}
