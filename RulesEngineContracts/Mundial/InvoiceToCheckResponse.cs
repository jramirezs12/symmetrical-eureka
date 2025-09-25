using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RulesEngineContracts.Mundial
{
    public record InvoiceToCheckResponse(int status, string detail, int rulesExecuted, List<string> ruleNamesMatched);
    //{
        //public int Status => rulesExecuted;
        //private readonly InvoiceToCheckResponse invoiceResponse = new InvoiceToCheckResponse(StatusCodes.Status200OK, "Reglas evaluadas exitosamente", rulesExecuted);

        //public static IResult OK => Results.Ok(new InvoiceToCheckResponse(status, detail, rulesExecuted));

        //public static IResult Error(IErrorOr result)
        //{
        //    int statusCode;
        //    string detail = string.Empty;
        //    int countRules = 0;

        //    if (result.Errors!.Count is 0)
        //        throw new InvalidOperationException("There is no errors encountered");
        //    else if (result.Errors.All(error => error.Type == ErrorType.Validation))
        //    {
        //        statusCode = StatusCodes.Status400BadRequest;
        //        detail = result.Errors.Aggregate(string.Empty, (acc, next) => $"{acc} {next.Description}, ").TrimEnd().TrimEnd(',');

        //        countRules = 0;

        //    }
        //    else
        //    {
        //        var error = result.Errors.First();
        //        statusCode = error.Type switch
        //        {
        //            ErrorType.Conflict => StatusCodes.Status409Conflict,
        //            ErrorType.Validation => StatusCodes.Status400BadRequest,
        //            ErrorType.NotFound => StatusCodes.Status404NotFound,
        //            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
        //            _ => StatusCodes.Status500InternalServerError,
        //        };
        //        detail = error.Description;
        //    }

        //    var res = new InvoiceToCheckResponse(statusCode, detail, countRules);
        //    return Results.Json(res, statusCode: statusCode);
        //    //return Results.Json(res, statusCode: StatusCodes.Status200OK);
        //}
        //return Results.Json(res, statusCode: statusCode);
    //}
}
