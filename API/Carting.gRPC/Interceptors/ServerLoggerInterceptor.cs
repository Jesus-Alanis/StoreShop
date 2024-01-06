using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Carting.gRPC.Interceptors
{
    public class ServerLoggerInterceptor : Interceptor
    {
        private readonly ILogger _logger;

        public ServerLoggerInterceptor(ILogger<ServerLoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}", MethodType.Unary, context.Method);
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error by {context.Method}.");
                throw;
            }
        }

        public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}", MethodType.Unary, context.Method);
            try
            {
                return await continuation(requestStream, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error by {context.Method}.");
                throw;
            }
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}", MethodType.Unary, context.Method);
            try
            {                
                await continuation(request, responseStream, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error by {context.Method}.");
                throw;
            }
        }

        public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}", MethodType.Unary, context.Method);
            try
            {
                await continuation(requestStream, responseStream, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled error by {context.Method}.");
                throw;
            }
        }
    }
}
