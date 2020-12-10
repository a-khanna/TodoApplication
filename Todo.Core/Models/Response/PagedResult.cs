using System.Collections.Generic;

namespace Todo.Core.Models.Response
{
    /// <summary>
    /// Used to return paged results for a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Page contents
        /// </summary>
        public List<T> PageContent { get; set; }

        /// <summary>
        /// Start index of the current response
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Total items in the database
        /// </summary>
        public int Total { get; set; }
    }
}
