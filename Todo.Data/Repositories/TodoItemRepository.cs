using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Abstractions.Data;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;
using Todo.Core.Models.Sql;

namespace Todo.Data.Repositories
{
    /// <summary>
    /// Repository for handling todo items
    /// </summary>
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoDbContext dbContext;

        public TodoItemRepository(TodoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets todo items by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo items entities</returns>
        public PagedResult<TodoItem> GetItems(int userId, PagingParameters pagingParams)
        {
            if (!dbContext.Users.Any(u => u.Id == userId))
                return null;

            var dbItems = dbContext.TodoItems
                .Include(d => d.Labels).AsNoTracking()
                .Where(t => t.User.Id == userId);

            // search by item name or labels
            if (!string.IsNullOrWhiteSpace(pagingParams.Search))
            {
                var searchLower = pagingParams.Search.ToLower();
                dbItems = dbItems.Where(d => d.Description.ToLower().Contains(searchLower) || d.Labels.Any(l => l.Name.ToLower().Contains(searchLower)));
            }

            var count = dbItems.Count();

            var result = dbItems
                .Skip(pagingParams.Skip)
                .Take(pagingParams.Take)
                .ToList();

            return new PagedResult<TodoItem>
            {
                PageContent = result,
                StartIndex = pagingParams.Skip,
                Total = count
            };
        }

        /// <summary>
        /// Gets todo items by list id.
        /// Returns null if item is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">item id</param>
        /// <returns>Todo item entity</returns>
        public TodoItem GetItem(int userId, int itemId)
        {
            return dbContext.TodoItems
                .AsNoTracking()
                .Include(t => t.Labels)
                .FirstOrDefault(t => t.User.Id == userId && t.Id == itemId);
        }

        /// <summary>
        /// Creates a item.
        /// Returns null if user is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo item to be created</param>
        /// <returns>Todo item</returns>
        public TodoItem CreateItem(int userId, CreateTodoItemDto createDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            var list = dbContext.TodoLists
                .Include(l => l.Items)
                .FirstOrDefault(u => u.Id == createDto.TodoListId);
            if (list == null)
                return null;

            var toBeCreated = new TodoItem
            {
                Description = createDto.Description,
                User = user,
                LastModified = DateTime.Now
            };

            if (list.Items == null) list.Items = new List<TodoItem>();

            list.Items.Add(toBeCreated);

            var result = dbContext.Add(toBeCreated).Entity;
            dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Updates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo item to be updated</param>
        /// <returns>Updated Todo item dto</returns>
        public TodoItem UpdateItem(int userId, TodoItemDto updateDto)
        {
            var existing = dbContext.TodoItems.FirstOrDefault(u => u.User.Id == userId && u.Id == updateDto.Id);
            if (existing == null)
                return null;

            existing.Description = updateDto.Description;
            existing.LastModified = DateTime.Now;

            dbContext.SaveChanges();

            return existing;
        }

        /// <summary>
        /// Deletes a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Id of todo item to be deleted</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteItem(int userId, int itemId)
        {
            var existing = dbContext.TodoItems.FirstOrDefault(u => u.User.Id == userId && u.Id == itemId);
            if (existing == null)
                return false;

            dbContext.Remove(existing);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets labels for todo item by user id.
        /// Returns null if no user or item is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Item Id</param>
        /// <returns>List of Labels Dto</returns>
        public List<Label> GetItemLabels(int userId, int itemId)
        {
            var existing = dbContext.TodoItems
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == itemId);

            if (existing == null)
                return null;

            return existing.Labels.ToList();
        }

        /// <summary>
        /// Creates a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo item to be created</param>
        /// <returns>Label dto</returns>
        public Label CreateLabel(int userId, CreateLabelDto createLabelDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            var existingItem = dbContext.TodoItems
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == createLabelDto.ParentId);
            if (existingItem == null)
                return null;

            // if label already exists return that label instead of creating new
            var existingLabel = existingItem.Labels.FirstOrDefault(l => l.Name == createLabelDto.Label);
            if (existingLabel != null)
                return existingLabel;

            var label = new Label
            {
                Name = createLabelDto.Label,
                User = user,
                LastModified = DateTime.Now
            };

            if (existingItem.Labels == null) existingItem.Labels = new List<Label>();

            existingItem.Labels.Add(label);

            dbContext.SaveChanges();

            return label;
        }

        /// <summary>
        /// Updates label for a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">label todo item to be updated</param>
        /// <returns>Updated label</returns>
        public Label UpdateLabel(int userId, UpdateLabelDto updateObj)
        {
            var existingItem = dbContext.TodoItems
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == updateObj.ParentId);
            if (existingItem == null)
                return null;

            var existingLabel = existingItem.Labels?.FirstOrDefault(l => l.Name == updateObj.CurrentValue);
            if (existingLabel == null)
                return null;

            existingLabel.Name = updateObj.NewValue;
            existingLabel.LastModified = DateTime.Now;

            dbContext.SaveChanges();

            return existingLabel;
        }

        /// <summary>
        /// Deletes a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteLabel(int userId, DeleteLabelDto deleteDto)
        {
            var existingItem = dbContext.TodoItems
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == deleteDto.ParentId);
            if (existingItem == null)
                return false;

            var existingLabel = existingItem.Labels?.FirstOrDefault(l => l.Name == deleteDto.Label);
            if (existingLabel == null)
                return false;

            dbContext.Remove(existingLabel);
            dbContext.SaveChanges();
            return true;
        }
    }
}
