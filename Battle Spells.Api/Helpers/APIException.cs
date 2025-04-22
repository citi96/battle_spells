using System.Net;

namespace Battle_Spells.Api.Helpers
{
    public class APIException(string message, HttpStatusCode statusCode, Exception? innerException = null) : Exception(message, innerException)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }
}
