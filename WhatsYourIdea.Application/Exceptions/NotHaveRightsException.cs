using GlobalExceptionHandler;

namespace WhatsYourIdea.Applications.Services.Exceptions
{
    public class NotHaveRightsException : GlobalException
    {
        public override int StatusCode => 400;

        public NotHaveRightsException() : base("У вас недастаточно прав для изменения записи")
        {
        }

        public NotHaveRightsException(string message) : base(message)
        {
        }
    }
}