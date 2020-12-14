using System.Linq;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Data.Repositories;
using Xunit;

namespace Todo.UnitTests.Data
{
    public class TodoItemRepositoryTests :TodoTestsBase
    {
        [Fact]
        public void GetItems_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetItems_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.GetItems(3, new PagingParameters());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetItems_ShouldReturnItems()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetItems_ShouldReturnItems));
            var repository = new TodoItemRepository(dbContext);
            var input = new PagingParameters
            {
                Search = "code"
            };

            // Act
            var result = repository.GetItems(1, input);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.PageContent);
            Assert.Equal("Code review", result.PageContent[0].Description);
        }

        [Fact]
        public void GetItem_ShouldReturnItem()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetItem_ShouldReturnItem));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.GetItem(1, 1);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateItem_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateItem_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.CreateItem(3, new CreateTodoItemDto());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateItem_ShouldReturnNullIfListIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateItem_ShouldReturnNullIfListIsNotFound));
            var repository = new TodoItemRepository(dbContext);
            var input = new CreateTodoItemDto
            {
                TodoListId = 10,
                Description = "Cheese"
            };

            // Act
            var result = repository.CreateItem(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateItem_ShouldCreateItem()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateItem_ShouldCreateItem));
            var repository = new TodoItemRepository(dbContext);
            var input = new CreateTodoItemDto
            {
                TodoListId = 1,
                Description = "Cheese"
            };

            // Act
            var result = repository.CreateItem(1, input);

            // Assert
            Assert.NotNull(result);
            var item = dbContext.TodoItems.FirstOrDefault(t => t.Description == "Cheese");
            Assert.NotNull(item);
        }

        [Fact]
        public void UpdateItem_ShouldReturnNullIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateItem_ShouldReturnNullIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);
            var input = new TodoItemDto
            {
                Id = 20,
                Description = "Cheese"
            };

            // Act
            var result = repository.UpdateItem(1, input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateItem_ShouldUpdateItem()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateItem_ShouldUpdateItem));
            var repository = new TodoItemRepository(dbContext);
            var input = new TodoItemDto
            {
                Id = 1,
                Description = "Cheese"
            };

            // Act
            var result = repository.UpdateItem(1, input);

            // Assert
            Assert.NotNull(result);
            var item = dbContext.TodoItems.FirstOrDefault(t => t.Id == 1);
            Assert.Equal("Cheese", item.Description);
        }

        [Fact]
        public void DeleteItem_ShouldReturnFalseIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteItem_ShouldReturnFalseIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.DeleteItem(1, 20);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteItem_ShouldDeleteItem()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteItem_ShouldDeleteItem));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.DeleteItem(1, 1);

            // Assert
            Assert.True(result);
            var item = dbContext.TodoItems.FirstOrDefault(t => t.Id == 1);
            Assert.Null(item);
        }

        [Fact]
        public void GetItemLabels_ShouldReturnNullIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetItemLabels_ShouldReturnNullIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.GetItemLabels(1, 20);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetItemLabels_ShouldGetItemLabels()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(GetItemLabels_ShouldGetItemLabels));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.GetItemLabels(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Today", result[0].Name);
        }

        [Fact]
        public void CreateLabel_ShouldReturnNullIfUserIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateItem_ShouldReturnNullIfUserIsNotFound));
            var repository = new TodoItemRepository(dbContext);

            // Act
            var result = repository.CreateLabel(3, new CreateLabelDto());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateLabel_ShouldReturnNullIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(CreateLabel_ShouldReturnNullIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);
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
            var repository = new TodoItemRepository(dbContext);
            var input = new CreateLabelDto
            {
                ParentId = 1,
                Label = "Test label"
            };

            // Act
            var result = repository.CreateLabel(1, input);

            // Assert
            Assert.NotNull(result);
            var item = dbContext.Labels.FirstOrDefault(t => t.Name == "Test label");
            Assert.NotNull(item);
        }

        [Fact]
        public void UpdateLabel_ShouldReturnNullIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(UpdateLabel_ShouldReturnNullIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);
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
            var repository = new TodoItemRepository(dbContext);
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
            var repository = new TodoItemRepository(dbContext);
            var input = new UpdateLabelDto
            {
                ParentId = 1,
                CurrentValue = "Today",
                NewValue = "tomorrow"
            };

            // Act
            var result = repository.UpdateLabel(1, input);

            // Assert
            Assert.NotNull(result);
            var item = dbContext.Labels.FirstOrDefault(t => t.Name == "tomorrow");
            Assert.NotNull(item);
        }

        [Fact]
        public void DeleteLabel_ShouldReturnFalseIfItemIsNotFound()
        {
            // Arrange
            var dbContext = SetupDatabase(nameof(DeleteLabel_ShouldReturnFalseIfItemIsNotFound));
            var repository = new TodoItemRepository(dbContext);
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
            var repository = new TodoItemRepository(dbContext);
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
            var repository = new TodoItemRepository(dbContext);
            var input = new DeleteLabelDto
            {
                ParentId = 1,
                Label = "Today"
            };

            // Act
            var result = repository.DeleteLabel(1, input);

            // Assert
            Assert.True(result);
            var item = dbContext.Labels.FirstOrDefault(t => t.Id == 1);
            Assert.Null(item);
        }
    }
}
