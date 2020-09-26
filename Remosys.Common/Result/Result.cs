using Microsoft.AspNetCore.Mvc;

namespace Remosys.Common.Result
{
    public class Result<T>
    {
        public T Data { get; private set; }
        public string Message { get; set; }
        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result<T> SuccessFul(T data) => new Result<T> { ApiResult = new OkObjectResult(data), Data = data, Message = null, Success = true };

        public static Result<T> SuccessFul(ObjectResult success) => new Result<T> { ApiResult = success, Success = true };

        public static Result<T> Failed(ObjectResult error) => new Result<T> { ApiResult = error, Success = false, Message = error.Value?.ToString() };
    }

    public class Result
    {
        public string Message { get; set; }

        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result SuccessFul() => new Result { Message = null, Success = true };

        public static Result SuccessFul(ObjectResult success) => new Result { ApiResult = success, Success = true };

        public static Result Failed(ObjectResult error) => new Result { ApiResult = error, Success = false, Message = error.Value?.ToString() };
    }
}