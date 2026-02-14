using FluentResults;
using Shared.Common.Enums;

namespace Shared.Common.Extensions
{
    public static class ResultHelper
    {
        public static Error WithErrorCode(string message, ErrorCode code)
        {
            return new Error(message).WithMetadata(nameof(ErrorCode), code);
        }

        public static Success WithSuccess(string message)
        {
            return new Success(message);
        }
    }
}
