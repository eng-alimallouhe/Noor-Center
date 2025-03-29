namespace LMS.Application.common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        private Result(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        private Result(string error) 
        {
            this.IsSuccess = false;
            this.Message = error;
        }

        public static Result Success(string SuccessMessage) => new Result(true, SuccessMessage);
        public static Result Failure(string errorMessage) => new Result(errorMessage);
    }

}
