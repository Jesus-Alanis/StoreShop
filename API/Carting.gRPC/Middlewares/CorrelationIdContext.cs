namespace Carting.gRPC.Middlewares
{
    public class CorrelationIdContext
    {
        private const string CORRELATION_ID_HEADER = "x-correlation-id";

        private readonly ILogger<CorrelationIdContext> _logger;
        private readonly RequestDelegate _next;

        public CorrelationIdContext(ILogger<CorrelationIdContext> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CORRELATION_ID_HEADER, out var correlationIds);
            var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

            using (_logger.BeginScope( new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                await _next(context);
            }           
        }
    }
}
