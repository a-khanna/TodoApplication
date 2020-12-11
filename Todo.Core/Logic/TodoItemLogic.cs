using System.Collections.Generic;
using AutoMapper;
using Todo.Core.Abstractions.Data;
using Todo.Core.Abstractions.Logic;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Request;
using Todo.Core.Models.Response;

namespace Todo.Core.Logic
{
    /// <summary>
    /// Logic layer for todo lists
    /// </summary>
    public class TodoItemLogic : ITodoItemLogic
    {
        private readonly ITodoItemRepository todoItemRepository;
        private readonly IMapper mapper;

        public TodoItemLogic(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            this.todoItemRepository = todoItemRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets todo items by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo items Dto</returns>
        public PagedResult<TodoItemDto> GetItems(int userId, PagingParameters pagingParam)
        {
            var pagedDbItems = todoItemRepository.GetItems(userId, pagingParam);

            if (pagedDbItems == null)
                return null;

            return mapper.Map<PagedResult<TodoItemDto>>(pagedDbItems);
        }

        /// <summary>
        /// Gets todo item by list id.
        /// Returns null if no list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">item id</param>
        /// <returns>Todo item Dto</returns>
        public TodoItemDto GetItem(int userId, int itemId)
        {
            var dbItem = todoItemRepository.GetItem(userId, itemId);
            return mapper.Map<TodoItemDto>(dbItem);
        }

        /// <summary>
        /// Creates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo item to be created</param>
        /// <returns>Todo item dto</returns>
        public TodoItemDto CreateItem(int userId, CreateTodoItemDto createDto)
        {
            var dbItem = todoItemRepository.CreateItem(userId, createDto);
            return mapper.Map<TodoItemDto>(dbItem);
        }

        /// <summary>
        /// Updates a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo item to be updated</param>
        /// <returns>Updated Todo item dto</returns>
        public TodoItemDto UpdateItem(int userId, TodoItemDto updateObj)
        {
            var dbList = todoItemRepository.UpdateItem(userId, updateObj);
            return mapper.Map<TodoItemDto>(dbList);
        }

        /// <summary>
        /// Deletes a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">Id of todo item to be deleted</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteItem(int userId, int itemId)
        {
            return todoItemRepository.DeleteItem(userId, itemId);
        }

        /// <summary>
        /// Gets labels for todo item
        /// Returns null if no user or item is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="itemId">List Id</param>
        /// <returns>List of Labels Dto</returns>
        public List<LabelDto> GetItemLabels(int userId, int itemId)
        {
            var dbLabels = todoItemRepository.GetItemLabels(userId, itemId);

            if (dbLabels == null)
                return null;

            return mapper.Map<List<LabelDto>>(dbLabels);
        }

        /// <summary>
        /// Creates a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo List to be created</param>
        /// <returns>Label dto</returns>
        public LabelDto CreateLabel(int userId, CreateOrDeleteLabelDto createLabelDto)
        {
            var dbLabel = todoItemRepository.CreateLabel(userId, createLabelDto);
            return mapper.Map<LabelDto>(dbLabel);
        }

        /// <summary>
        /// Updates label for a todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">label to be updated</param>
        /// <returns>Updated label dto</returns>
        public LabelDto UpdateLabel(int userId, UpdateLabelDto updateObj)
        {
            var dbLabel = todoItemRepository.UpdateLabel(userId, updateObj);
            return mapper.Map<LabelDto>(dbLabel);
        }

        /// <summary>
        /// Deletes a label for todo item.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteLabel(int userId, CreateOrDeleteLabelDto deleteDto)
        {
            return todoItemRepository.DeleteLabel(userId, deleteDto);
        }
    }
}
