namespace Todo.Core.Models.Response
{
    /// <summary>
    /// Generic response wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Status for the response
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Generic model to be sent with response
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Mesage for additional information
        /// </summary>
        public string Message { get; set; }
    }
}
