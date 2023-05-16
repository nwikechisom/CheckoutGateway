namespace CheckoutGateway.Api.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get the X-API-Key from the request headers
            var apiKey = context.Request.Headers["X-API-Key"];
            List<string> allowedKeys = _configuration.GetSection("MerchantKeys").Get<List<string>>();
            // If the X-API-Key is not present, return a 401 error
            if (string.IsNullOrEmpty(apiKey) || !allowedKeys.Any(x => x.Equals(apiKey)))
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"CheckoutGateway API\"");
                return;
            }
            // Continue to the next middleware in the pipeline
            await _next(context);
        }
    }
}
