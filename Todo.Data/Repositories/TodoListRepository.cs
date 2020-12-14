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
    /// Repository for handling todo lists
    /// </summary>
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoDbContext dbContext;

        public TodoListRepository(TodoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets todo lists by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo list entities</returns>
        public PagedResult<TodoList> GetLists(int userId, PagingParameters pagingParams)
        {
            if (!dbContext.Users.Any(u => u.Id == userId))
                return null;

            var dbLists = dbContext.TodoLists
                .Include(d => d.Labels).AsNoTracking()
                .Where(t => t.User.Id == userId);

            // search by list name or labels
            if (!string.IsNullOrWhiteSpace(pagingParams.Search))
            {
                var searchLower = pagingParams.Search.ToLower();
                dbLists = dbLists.Where(d => d.Name.ToLower().Contains(searchLower) || d.Labels.Any(l => l.Name.ToLower().Contains(searchLower)));
            }               

            var count = dbLists.Count();

            var result = dbLists
                .Skip(pagingParams.Skip)
                .Take(pagingParams.Take)
                .ToList();

            return new PagedResult<TodoList>
            {
                PageContent = result,
                StartIndex = pagingParams.Skip,
                Total = count
            };
        }

        /// <summary>
        /// Gets todo lists by list id.
        /// Returns null if list is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List id</param>
        /// <returns>Todo list entity</returns>
        public TodoList GetList(int userId, int listId)
        {
            return dbContext.TodoLists
                .AsNoTracking()
                .Include(t => t.Items).ThenInclude(t => t.Labels)
                .Include(t => t.Labels)
                .FirstOrDefault(t => t.User.Id == userId && t.Id == listId);
        }

        /// <summary>
        /// Creates a list.
        /// Returns null if user is not found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo List to be created</param>
        /// <returns>Todo list</returns>
        public TodoList CreateList(int userId, CreateTodoListDto createDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            var toBeCreated = new TodoList
            {
                Name = createDto.Name,
                User = user,
                LastModified = DateTime.Now
            };

            var result = dbContext.Add(toBeCreated).Entity;
            dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Updates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        public TodoList UpdateList(int userId, TodoListDto updateDto)
        {
            var existing = dbContext.TodoLists.FirstOrDefault(u => u.User.Id == userId && u.Id == updateDto.Id);
            if (existing == null)
                return null;

            existing.Name = updateDto.Name;
            existing.LastModified = DateTime.Now;

            dbContext.SaveChanges();

            return existing;
        }

        /// <summary>
        /// Deletes a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">Id of todo List to be deleted</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteList(int userId, int listId)
        {
            var existing = dbContext.TodoLists.FirstOrDefault(u => u.User.Id == userId && u.Id == listId);
            if (existing == null)
                return false;

            dbContext.Remove(existing);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets labels for todo lists by user id.
        /// Returns null if no user or list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List Id</param>
        /// <returns>List of Labels Dto</returns>
        public List<Label> GetListLabels(int userId, int listId)
        {
            var existing = dbContext.TodoLists
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == listId);

            if (existing == null)
                return null;

            return existing.Labels.ToList();
        }

        /// <summary>
        /// Creates a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo List to be created</param>
        /// <returns>Label dto</returns>
        public Label CreateLabel(int userId, CreateOrDeleteLabelDto createLabelDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            var existingList = dbContext.TodoLists
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == createLabelDto.ParentId);
            if (existingList == null)
                return null;

            // if label already exists return that label instead of creating new
            var existingLabel = existingList.Labels.FirstOrDefault(l => l.Name == createLabelDto.Label);
            if (existingLabel != null)
                return existingLabel;

            var label = new Label
            {
                Name = createLabelDto.Label,
                User = user,
                LastModified = DateTime.Now
            };

            if (existingList.Labels == null) existingList.Labels = new List<Label>();

            existingList.Labels.Add(label);

            dbContext.SaveChanges();

            return label;
        }

        /// <summary>
        /// Updates label for a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        public Label UpdateLabel(int userId, UpdateLabelDto updateObj)
        {
            var existingList = dbContext.TodoLists
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == updateObj.ParentId);
            if (existingList == null)
                return null;

            var existingLabel = existingList.Labels?.FirstOrDefault(l => l.Name == updateObj.CurrentValue);
            if (existingLabel == null)
                return null;

            existingLabel.Name = updateObj.NewValue;
            existingLabel.LastModified = DateTime.Now;

            dbContext.SaveChanges();

            return existingLabel;
        }

        /// <summary>
        /// Deletes a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteLabel(int userId, CreateOrDeleteLabelDto deleteDto)
        {
            var existingList = dbContext.TodoLists
                .Include(l => l.Labels)
                .FirstOrDefault(u => u.User.Id == userId && u.Id == deleteDto.ParentId);
            if (existingList == null)
                return false;

            var existingLabel = existingList.Labels?.FirstOrDefault(l => l.Name == deleteDto.Label);
            if (existingLabel == null)
                return false;

            dbContext.Remove(existingLabel);
            dbContext.SaveChanges();
            return true;
        }
    }
}
