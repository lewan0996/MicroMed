using System.Net;

namespace MicroMed.IdentityServer;

public class FixAntiForgeryIssueMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger logger;
    private readonly string _matchPath;

    public FixAntiForgeryIssueMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, string matchPath)
    {
        _next = next;
        _matchPath = matchPath;
        logger = loggerFactory.CreateLogger<FixAntiForgeryIssueMiddleware>(); ;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next.Invoke(context);
        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            if (context.Request.Path.StartsWithSegments(new PathString(_matchPath)))
            {
                if (context.Request.Form.TryGetValue("Input.ReturnUrl", out var returnUrls))
                {
                    var returnUrl = returnUrls.FirstOrDefault();
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        context.Response.Redirect(_matchPath);
                    }
                    else
                    {
                        context.Response.Redirect(returnUrl);
                    }
                }
            }
        }
    }
}
