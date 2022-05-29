using System.Net;

namespace API.Helper
{
    public static class ErrorHandler
    {
        public static ErrorDetail ReturnErrorModel(string message,HttpStatusCode code)
        {
            return new ErrorDetail()
            {
                StatusCode = (int)code,
                Message = message
            };
        }
    }
}
