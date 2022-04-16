namespace WhatsYourIdea.Applications.DTO
{
    public class OperationResult<TData, TError>
        where TError : class
        where TData : class
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<TError> Errors { get; set; }
        public TData Data { get; set; }
    }
    public class OperationResult<TError> : OperationResult<object, TError>
        where TError : class
    {
    }
}