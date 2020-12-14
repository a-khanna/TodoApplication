namespace Todo.Core.Models.Request
{
    /// <summary>
    /// Model used for sending paging requests.
    /// Also contains Search field
    /// </summary>
    public class PagingParameters
    {
        /// <summary>
        /// Search by name/description or label
        /// </summary>
        /// <example>shopping</example>
        public string Search { get; set; }

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 50;
    }
}
