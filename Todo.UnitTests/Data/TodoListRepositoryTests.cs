using System.Linq;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Data.Repositories;
using Xunit;

namespace Todo.UnitTests.Data
{
    public class TodoListRepositoryTests : TodoTestsBase
    {
        [Fact]
        public void GetLists_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetLists_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.GetLists(3, new PagingParameters());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLists_ShouldReturnLists()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetLists_ShouldReturnLists));
            var repository = new TodoListRepository(dbContext);
            var input = new PagingParameters
            {
                Search = "shop"
            };

            // Act
            var result = repository.GetLists(1, input);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.PageContent);
            Assert.Equal("Shopping list", result.PageContent[0].Name);
        }

        [Fact]
        public void GetList_ShouldReturnList()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetList_ShouldReturnList));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.GetList(1, 1);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateList_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateList_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.CreateList(3, new CreateTodoListDto());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateList_ShouldCreateList()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateList_ShouldCreateList));
            var repository = new TodoListRepository(dbContext);
            var input = new CreateTodoListDto
            {
                Name = "Christmas Gifts"
            };

            // Act
            var result = repository.CreateList(1, input);

            // Assert
            Assert.NotNull(result);
            var List = dbContext.TodoLists.FirstOrDefault(t => t.Name == "Christmas Gifts");
            Assert.NotNull(List);
        }

        [Fact]
        public void UpdateList_ShouldReturnNullIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateList_ShouldReturnNullIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new TodoListDto
            {
                Id = 20,
                Name = "Some list"
            };

            // Act
            var result = repository.UpdateList(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateList_ShouldUpdateList()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateList_ShouldUpdateList));
            var repository = new TodoListRepository(dbContext);
            var input = new TodoListDto
            {
                Id = 1,
                Name = "Groceries"
            };

            // Act
            var result = repository.UpdateList(1, input);

            // Assert
            Assert.NotNull(result);
            var List = dbContext.TodoLists.FirstOrDefault(t => t.Id == 1);
            Assert.Equal("Groceries", List.Name);
        }

        [Fact]
        public void DeleteList_ShouldReturnFalseIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteList_ShouldReturnFalseIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.DeleteList(1, 20);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteList_ShouldDeleteList()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteList_ShouldDeleteList));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.DeleteList(1, 1);

            // Assert
            Assert.True(result);
            var list = dbContext.TodoLists.FirstOrDefault(t => t.Id == 1);
            Assert.Null(list);
        }

        [Fact]
        public void GetListLabels_ShouldReturnNullIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetListLabels_ShouldReturnNullIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.GetListLabels(1, 20);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetListLabels_ShouldGetListLabels()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetListLabels_ShouldGetListLabels));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.GetListLabels(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Important", result[0].Name);
        }

        [Fact]
        public void CreateLabel_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateList_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoListRepository(dbContext);

            // Act
            var result = repository.CreateLabel(3, new CreateLabelDto());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateLabel_ShouldReturnNullIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateLabel_ShouldReturnNullIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new CreateLabelDto
            {
                ParentId = 20,
                Label = "Cheese"
            };

            // Act
            var result = repository.CreateLabel(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateLabel_ShouldCreateLabel()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateLabel_ShouldCreateLabel));
            var repository = new TodoListRepository(dbContext);
            var input = new CreateLabelDto
            {
                ParentId = 1,
                Label = "Test label"
            };

            // Act
            var result = repository.CreateLabel(1, input);

            // Assert
            Assert.NotNull(result);
            var List = dbContext.Labels.FirstOrDefault(t => t.Name == "Test label");
            Assert.NotNull(List);
        }

        [Fact]
        public void UpdateLabel_ShouldReturnNullIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateLabel_ShouldReturnNullIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new UpdateLabelDto
            {
                ParentId = 30
            };

            // Act
            var result = repository.UpdateLabel(3, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateLabel_ShouldReturnNullIfLabelIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateLabel_ShouldReturnNullIfLabelIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new UpdateLabelDto
            {
                ParentId = 1,
                CurrentValue = "test1",
                NewValue = "test2"
            };

            // Act
            var result = repository.UpdateLabel(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateLabel_ShouldUpdateLabel()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateLabel_ShouldUpdateLabel));
            var repository = new TodoListRepository(dbContext);
            var input = new UpdateLabelDto
            {
                ParentId = 1,
                CurrentValue = "Important",
                NewValue = "Urgent"
            };

            // Act
            var result = repository.UpdateLabel(1, input);

            // Assert
            Assert.NotNull(result);
            var List = dbContext.Labels.FirstOrDefault(t => t.Name == "Urgent");
            Assert.NotNull(List);
        }

        [Fact]
        public void DeleteLabel_ShouldReturnFalseIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteLabel_ShouldReturnFalseIfListIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new DeleteLabelDto
            {
                ParentId = 30
            };

            // Act
            var result = repository.DeleteLabel(3, input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteLabel_ShouldReturnFalseIfLabelIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteLabel_ShouldReturnFalseIfLabelIsNotFound));
            var repository = new TodoListRepository(dbContext);
            var input = new DeleteLabelDto
            {
                ParentId = 1,
                Label = "test1"
            };

            // Act
            var result = repository.DeleteLabel(1, input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteLabel_ShouldDeleteLabel()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteLabel_ShouldDeleteLabel));
            var repository = new TodoListRepository(dbContext);
            var input = new DeleteLabelDto
            {
                ParentId = 1,
                Label = "Important"
            };

            // Act
            var result = repository.DeleteLabel(1, input);

            // Assert
            Assert.True(result);
            var List = dbContext.Labels.FirstOrDefault(t => t.Name == "Important");
            Assert.Null(List);
        }
    }
}
