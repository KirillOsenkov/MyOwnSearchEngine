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
                result = Engine.GetResponse(query, Request);
            }
            catch (Exception ex)
            {
                result = Div(Escape(ex.ToString()));
            }

            Response.Headers.Add("Cache-Control", new[] { "no-cache" });
            Response.Headers.Add("Pragma", new[] { "no-cache" });
            Response.Headers.Add("Expires", new[] { "-1" });
            Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type" });

            return result;
        }
    }
}
