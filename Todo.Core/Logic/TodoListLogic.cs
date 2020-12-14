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
    public class TodoListLogic : ITodoListLogic
    {
        private readonly ITodoListRepository todoListRepository;
        private readonly IMapper mapper;

        public TodoListLogic(ITodoListRepository todoListRepository, IMapper mapper)
        {
            this.todoListRepository = todoListRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets todo lists by user id.
        /// Returns null if no user is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pagingParams">Pagin params</param>
        /// <returns>Todo lists Dto</returns>
        public PagedResult<TodoListDto> GetLists(int userId, PagingParameters pagingParam)
        {
            var pagedDbList = todoListRepository.GetLists(userId, pagingParam);

            if (pagedDbList == null)
                return null;

            return mapper.Map<PagedResult<TodoListDto>>(pagedDbList);
        }

        /// <summary>
        /// Gets todo list by list id.
        /// Returns null if no list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List id</param>
        /// <returns>Todo list Dto</returns>
        public TodoListDetailedDto GetList(int userId, int listId)
        {
            var dbList = todoListRepository.GetList(userId, listId);
            return mapper.Map<TodoListDetailedDto>(dbList);
        }

        /// <summary>
        /// Creates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createDto">todo List to be created</param>
        /// <returns>Todo list dto</returns>
        public TodoListDto CreateList(int userId, CreateTodoListDto createDto)
        {
            var dbList = todoListRepository.CreateList(userId, createDto);
            return mapper.Map<TodoListDto>(dbList);
        }

        /// <summary>
        /// Updates a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        public TodoListDto UpdateList(int userId, TodoListDto updateObj)
        {
            var dbList = todoListRepository.UpdateList(userId, updateObj);
            return mapper.Map<TodoListDto>(dbList);
        }

        /// <summary>
        /// Deletes a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">Id of todo List to be deleted</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteList(int userId, int listId)
        {
            return todoListRepository.DeleteList(userId, listId);
        }

        /// <summary>
        /// Gets labels for todo lists by user id.
        /// Returns null if no user or list is found.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="listId">List Id</param>
        /// <returns>List of Labels Dto</returns>
        public List<LabelDto> GetListLabels(int userId, int listId)
        {
            var dbLabels = todoListRepository.GetListLabels(userId, listId);

            if (dbLabels == null)
                return null;

            return mapper.Map<List<LabelDto>>(dbLabels);
        }

        /// <summary>
        /// Creates a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="createLabelDto">label for todo List to be created</param>
        /// <returns>Label dto</returns>
        public LabelDto CreateLabel(int userId, CreateLabelDto createLabelDto)
        {
            var dbLabel = todoListRepository.CreateLabel(userId, createLabelDto);
            return mapper.Map<LabelDto>(dbLabel);
        }

        /// <summary>
        /// Updates label for a todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="updateObj">todo List to be updated</param>
        /// <returns>Updated Todo list dto</returns>
        public LabelDto UpdateLabel(int userId, UpdateLabelDto updateObj)
        {
            var dbLabel = todoListRepository.UpdateLabel(userId, updateObj);
            return mapper.Map<LabelDto>(dbLabel);
        }

        /// <summary>
        /// Deletes a label for todo list.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="deleteDto">Delete Dto</param>
        /// <returns>True if delete was successful</returns>
        public bool DeleteLabel(int userId, DeleteLabelDto deleteDto)
        {
            return todoListRepository.DeleteLabel(userId, deleteDto);
        }
    }
}
