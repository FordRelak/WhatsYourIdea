using GlobalExceptionHandler;

namespace WhatsYourIdea.Applications.Services.Exceptions
{
    public class NotFoundException : GlobalException
    {
        public override int StatusCode => 404;

        public NotFoundException() : base("Искомый объект не найден")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}