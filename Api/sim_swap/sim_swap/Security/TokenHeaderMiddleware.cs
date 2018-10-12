using sim_swap.Models;
using sim_swap.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

public class TokenHeaderAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConnectionManager _connectionManager;

    public TokenHeaderAuthorizationMiddleware(RequestDelegate next, ConnectionManager connectionManager)
    {
        _next = next;
        _connectionManager = connectionManager;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Value.ToLower().Trim().EndsWith("/user/login"))
        {
            await _next.Invoke(context);
            return;
        }

        string authHeader = context.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader))
        {
            var queryString = context.Request.Query["authToken"];
            if (!string.IsNullOrEmpty(queryString))
            {
                authHeader = queryString;
            }
        }
        if (!string.IsNullOrEmpty(authHeader))
        {
            Task<bool> isHeaderValid = ValidateCredentials(authHeader);
            if (await isHeaderValid)
            {
                if (_next != null)
                {
                    try
                    {
                        await _next.Invoke(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                return;
            }

        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");

    }

    private async Task<bool> ValidateCredentials(string authHeader)
    {
        try
        {
            int result = await _connectionManager.CreateCommand("is_credentials_valid")
                  .WithSqlParam("authtoken", authHeader)
                  .ExecuteReturningScalarAsync<int>();

            if (result == 1)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return false;

    }

}

public static class TokenHeaderAuthorizationMiddlewareExtension
{
    public static IApplicationBuilder UseTokenHeaderAuthorization(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new System.ArgumentNullException(nameof(app));
        }

        return app.UseMiddleware<TokenHeaderAuthorizationMiddleware>();
    }
}

public class TokenHeaderAuthorizationPipeline
{
    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseTokenHeaderAuthorization();
    }
}