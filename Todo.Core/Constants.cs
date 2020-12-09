namespace Todo.Core
{
    /// <summary>
    /// Constants used across the application
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Key used for fetching the connection string from appsettings
        /// </summary>
        public const string ConnectionStringKey = "TodoConnectionString";

        /// <summary>
        /// Key used for fetching the seed setting from appsettings
        /// </summary>
        public const string SeedKey = "SeedDatabase";

        /// <summary>
        /// Key used for fetching the NLog settings from appsettings
        /// </summary>
        public const string NLogKey = "NLog";

        /// <summary>
        /// Correlation Id header
        /// </summary>
        public const string XCorrelationId = "x-correlation-id";
    }
}
