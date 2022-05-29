

namespace Infrastructure.Exceptions
{
    public abstract class CustomException:System.Exception
    {
        public CustomException(string message):base(message)
        {

        }
    }
}
