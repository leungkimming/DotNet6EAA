namespace API {
    /// <summary>
    /// Static Utility class that provides extension methods
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Adds the HttpLoggerMiddleaware to the .NET request pipeline
        /// </summary>
        public static IApplicationBuilder UseHttpLogger(
            this IApplicationBuilder builder) {
            return builder.UseMiddleware<HttpLoggerMiddleware>();
        }
    }
}
