namespace E_Commerce.Common
{
    public class OperationResultGeneric<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static OperationResultGeneric<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static OperationResultGeneric<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }

}
