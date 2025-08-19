//RequireWriteAccessAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkflowDocMgmt.Web.Filters
{
    public class RequireWriteAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accessLevel = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "AccessLevel")?.Value;

            if (accessLevel != "ReadWrite")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            }
        }
    }
}
