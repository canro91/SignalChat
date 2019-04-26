using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace SignalChat.Controllers
{
    [Authorize]
    public class DefaultController : ApiController
    {
        public IEnumerable<string> Get(string msg)
        {
            var user = HttpContext.Current.User.Identity;

            return new string[] { "value1", "value2" };
        }
    }
}
