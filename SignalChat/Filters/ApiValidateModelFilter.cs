using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace SignalChat.Filters
{
    public class ApiValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join(" ", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                throw new ValidationException($"There's something wrong with this request: {errors}");
            }
        }
    }
}
