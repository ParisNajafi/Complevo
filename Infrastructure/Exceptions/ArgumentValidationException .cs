

namespace Infrastructure.Exceptions
{
    public class ArgumentValidationException : CustomException
    {
        public ArgumentValidationException(string message):base(message)
        {

        }
    }
}
