using Microsoft.AspNet.Http;

namespace MyOwnSearchEngine
{
    public class Query
    {
        public string OriginalInput { get; }
        public object Structure { get; }
        public HttpRequest Request { get; set; }

        public Query(string input)
        {
            OriginalInput = input;
            Structure = Engine.Parse(input);
        }

        public string IpAddress
        {
            get
            {
                return Request?.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public T TryGetStructure<T>()
        {
            return Engine.TryGetStructure<T>(Structure);
        }
    }
}
