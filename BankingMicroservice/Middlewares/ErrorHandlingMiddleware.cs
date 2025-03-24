namespace BankingMicroservice.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch(Exception)
            {
                // Return 'Internal Server Error' for any unhandled application exception with the default text 'An error occurred while processing the request'
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred while processing the request").ConfigureAwait(false);
            }
        }
    }
}