using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RulesEngineAPI.Extensions
{
    public static class ErrorOrExtensions
    {
        public static IResult ToProblem(this IErrorOr result)
        {
            if (result.Errors!.Count is 0)
            {
                throw new InvalidOperationException("There is no errors encountered");
            }

            if (result.Errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(result.Errors);
            }

            return Problem(result.Errors[0]);
        }

        private static IResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Results.Problem(statusCode: statusCode, title: error.Description);
        }

        private static IResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();
            var modelErrorDictionary = new Dictionary<string, string[]>();

            Dictionary<string, object>? metadata = errors.FirstOrDefault().Metadata;
            string? detail = errors.FirstOrDefault().Description;

            errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));
            modelErrorDictionary.Add("Messages", errors.Select(x => x.Description).ToArray());
            return Results.ValidationProblem(modelErrorDictionary, detail: detail, extensions: metadata!);
        }
    }
}
