
namespace Infrastructure.Exceptions
{
    public class UnAuthorizedException : CustomException
    {
        public UnAuthorizedException(string message):base(message)
        {

        }
    }
}
