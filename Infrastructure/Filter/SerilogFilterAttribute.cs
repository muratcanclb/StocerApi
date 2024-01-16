using Microsoft.AspNetCore.Mvc.Filters;
//using Serilog;

namespace Intra.Api.Infrastructure.Filter
{
    public class SerilogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //if (context.Exception != null)
            //    Log.Error(context.Exception, context.Exception.Message);

            base.OnActionExecuted(context);
        }
    }
}