namespace EncryptionDemo.MiddleWare
{
    public class LoggingMiddleWare
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleWare(RequestDelegate next) { 
        _next = next;
        }
        public async Task InvokeAsync(HttpContext context) {
            Console.WriteLine($"Request Received: {context.Request.Method} {context.Request.Path} at {DateTime.UtcNow}");
            await _next(context);
            Console.WriteLine($"Response Sent: Status Code {context.Response.StatusCode}");
        }


    }
}
