namespace JWT.Dtos
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public int StatusCode { get; set; }

        public static ApiResponseDto<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        public static ApiResponseDto<T> ErrorResponse(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                StatusCode = statusCode
            };
        }
    }
}