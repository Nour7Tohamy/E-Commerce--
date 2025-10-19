namespace E_Commerce.Common
{
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static OperationResult Ok(string message = "Operation completed successfully.")
            => new OperationResult(true, message);

        public static OperationResult Fail(string message = "Operation failed.")
            => new OperationResult(false, message);
    }
}
