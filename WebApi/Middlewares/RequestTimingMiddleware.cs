using Application.Components;
using System.Diagnostics;

namespace WebApi.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWriteLineInFileService _writeFileService;
        public RequestTimingMiddleware(RequestDelegate next, IWriteLineInFileService writeFileService)
        {
            _next = next;
            _writeFileService = writeFileService;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                await _writeFileService.WriteLineAsync(
                    $"Request: {context.Request.Method} {context.Request.Path} => {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}