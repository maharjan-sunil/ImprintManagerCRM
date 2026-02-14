namespace Shared.Common.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; }
        public string? Message { get; }
        public T? Data { get; }
        public List<string>? Errors { get; }

        private ApiResponse(bool success, string? message, T? data = default, List<string>? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string? message = null)
            => new(true, message, data);

        public static ApiResponse<T> FailResponse(List<string> errors, string? message = null)
            => new(false, message, default, errors);
    }
}
