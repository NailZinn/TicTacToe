using Shared;

namespace TicTacToeServer.Middlewares;

public class MoveAuthCookieToHeaderMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Cookies.ContainsKey(Constants.JwtCookie))
            context.Request.Headers[Constants.AuthorizationHeader] = context.Request.Cookies[Constants.JwtCookie];
        return next(context);
    }
}
