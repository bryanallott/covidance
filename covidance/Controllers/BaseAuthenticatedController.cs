using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace covidance.Controllers
{
    [Authorize]
    public class BaseAuthenticatedController : Controller
    {
        protected JsonResult ThrowJSONError(Exception e)
        {
            //Response.Clear();
            //Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return Json(new { Message = e.Message, Stacktrace = e.StackTrace });
        }
        protected Guid CurrentUser => new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected string CurrentUserName => User.Identity.Name;
    }
}
