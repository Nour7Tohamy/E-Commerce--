namespace E_Commerce.Common
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult Ok(string message = "Operation succeeded") =>
            new() { Success = true, Message = message };

        public static OperationResult Fail(string message = "Operation failed") =>
            new() { Success = false, Message = message };
    }
}
