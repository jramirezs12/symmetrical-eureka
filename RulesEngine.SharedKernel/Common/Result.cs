namespace RulesEngine.SharedKernel.Common
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;

        }

        public bool IsSuccess { get; }
        public Error Error { get; }
        public bool IsFailure => !IsSuccess;
        public static Result Success() => new(true, Error.None);
        public static Result Failure() => new(false, Error.None);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(TValue value) => new(value, false, Error.None);
        public static Result<T> Failure<T>(Error error) => new(error, false, error);
    }
}
