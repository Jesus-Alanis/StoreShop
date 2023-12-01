namespace Carting.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                var userName = context.User?.Identity?.Name;
                _logger.LogInformation(string.Format("User: {0} - Request: {1} {2}", userName, context.Request.Method, context.Request.Path));
            }
            
            await _next(context);
        }
    }
}
