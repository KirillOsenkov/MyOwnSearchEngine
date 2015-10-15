using System;
using Microsoft.AspNet.Mvc;
using static MyOwnSearchEngine.HtmlFactory;

namespace MyOwnSearchEngine.Controllers
{
    [Route("api/[controller]")]
    public class AnswersController : Controller
    {
        [HttpGet]
        public string Get(string query)
        {
            string result = null;
            try
            {
                result = Engine.GetResponse(query);
            }
            catch (Exception ex)
            {
                result = Div(Escape(ex.ToString()));
            }

            Context.Response.Headers.Add("Cache-Control", new[] { "no-cache" });
            Context.Response.Headers.Add("Pragma", new[] { "no-cache" });
            Context.Response.Headers.Add("Expires", new[] { "-1" });
            Context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type" });

            return result;
        }
    }
}
