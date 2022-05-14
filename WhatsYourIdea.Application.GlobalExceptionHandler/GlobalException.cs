namespace GlobalExceptionHandler
{
    public abstract class GlobalException : Exception
    {
        public virtual int StatusCode { get; } = 505;

        protected GlobalException() : base("Internal server error")
        {
        }

        protected GlobalException(string message) : base(message)
        {
        }
    }
}