using System;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public enum UseCaseResultState
    {
        Success,
        Failed,
    }

    public class UseCaseResult<T>
    {
        public UseCaseResult(UseCaseResultState state, T result, Exception exception)
        {
            State = state;
            Result = result;
            Exception = exception;
        }

        public UseCaseResultState State { get; }
        public T Result { get; }
        public Exception Exception { get; }
    }

    public class UseCaseResult
    {
        private static UseCaseResult SuccessInstance { get; } = new UseCaseResult(UseCaseResultState.Success, null);
        private static UseCaseResult FailedInstance { get; } = new UseCaseResult(UseCaseResultState.Failed, null);
        public UseCaseResult(UseCaseResultState state, Exception exception)
        {
            State = state;
            Exception = exception;
        }

        public UseCaseResultState State { get; }
        public Exception Exception { get; }

        public static UseCaseResult Success() => SuccessInstance;
        public static UseCaseResult Failed() => FailedInstance;
        public static UseCaseResult Error(Exception error) => new UseCaseResult(UseCaseResultState.Failed, error);

        public static UseCaseResult<T> Success<T>(T result) => new UseCaseResult<T>(UseCaseResultState.Success, result, null);
        public static UseCaseResult<T> Failed<T>(T result = default) => new UseCaseResult<T>(UseCaseResultState.Failed, result, null);
        public static UseCaseResult<T> Error<T>(Exception error, T result = default) => new UseCaseResult<T>(UseCaseResultState.Failed, result, error);

    }
}
