using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Enums;
using Shared.Common.Models;

namespace ApiAbstractions.Extensions
{
    public static class ApiResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            var message = result.Successes.FirstOrDefault()?.Message;
            var errorMessages = result.Errors.Select(e => e.Message).ToList();
            var errorCodes = result.Errors
                .Select(e => e.Metadata.TryGetValue("ErrorCode", out var code) ? code?.ToString() : null)
                .Where(c => c != null)
                .ToList();

            if (result.IsSuccess)
            {
                return new OkObjectResult(ApiResponse<T>.SuccessResponse(result.Value, message));
            }

            // Handle specific error codes if any
            if (errorCodes.Contains(ErrorCode.NotFound.ToString()))
                return new NotFoundObjectResult(ApiResponse<T>.FailResponse(errorMessages, message));

            if (errorCodes.Contains(ErrorCode.Unauthorized.ToString()) || errorCodes.Contains(ErrorCode.InvalidCredentials.ToString()))
                return new UnauthorizedObjectResult(ApiResponse<T>.FailResponse(errorMessages, message));

            if (errorCodes.Contains(ErrorCode.Forbidden.ToString()))
                return new ObjectResult(ApiResponse<T>.FailResponse(errorMessages, message)) { StatusCode = 403 };

            if (errorCodes.Contains(ErrorCode.Conflict.ToString()))
                return new ConflictObjectResult(ApiResponse<T>.FailResponse(errorMessages, message));

            // Default fallback
            return new BadRequestObjectResult(ApiResponse<T>.FailResponse(errorMessages, message));
        }

        public static IActionResult ToActionResult(this Result result)
        {
            var message = result.Successes.FirstOrDefault()?.Message;
            var errorMessages = result.Errors.Select(e => e.Message).ToList();
            var errorCodes = result.Errors
                .Select(e => e.Metadata.TryGetValue("ErrorCode", out var code) ? code?.ToString() : null)
                .Where(c => c != null)
                .ToList();

            if (result.IsSuccess)
            {
                return new OkObjectResult(ApiResponse<string>.SuccessResponse(null, message));
            }

            if (errorCodes.Contains(ErrorCode.NotFound.ToString()))
                return new NotFoundObjectResult(ApiResponse<string>.FailResponse(errorMessages, message));

            if (errorCodes.Contains(ErrorCode.Unauthorized.ToString()) || errorCodes.Contains(ErrorCode.InvalidCredentials.ToString()))
                return new UnauthorizedObjectResult(ApiResponse<string>.FailResponse(errorMessages, message));

            if (errorCodes.Contains(ErrorCode.Forbidden.ToString()))
                return new ObjectResult(ApiResponse<string>.FailResponse(errorMessages, message)) { StatusCode = 403 };

            if (errorCodes.Contains(ErrorCode.Conflict.ToString()))
                return new ConflictObjectResult(ApiResponse<string>.FailResponse(errorMessages, message));

            return new BadRequestObjectResult(ApiResponse<string>.FailResponse(errorMessages, message));
        }
    }
}
