using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNet.Mvc;

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
                result = "div class=\"exception\">" + ex.ToString() + "</div>";
            }

            return result;
        }
    }
}
