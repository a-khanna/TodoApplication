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
        /// Returns empty list if no user is found.
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
    }
}
