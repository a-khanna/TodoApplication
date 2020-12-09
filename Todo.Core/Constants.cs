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

        /// <summary>
        /// Claim name for storing user id
        /// </summary>
        public const string UserIdClaim = "userid";

        /// <summary>
        /// Claim name for storing username
        /// </summary>
        public const string UsernameClaim = "username";

        /// <summary>
        /// Claim name for storing issuer
        /// </summary>
        public const string IssuerClaim = "issuer";

        /// <summary>
        /// Claim name for storing issuer
        /// </summary>
        public const string AppSettingsJwtKey = "JWT:Key";

        /// <summary>
        /// Claim name for storing issuer
        /// </summary>
        public const string AppSettingsJwtIssuer = "JWT:Issuer";
    }
}
