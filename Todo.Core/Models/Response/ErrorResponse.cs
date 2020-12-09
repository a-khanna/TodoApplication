namespace Todo.Core.Models.Response
{
    /// <summary>
    /// Model for returning exception details
    /// </summary>
    public class ErrorResponse
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
