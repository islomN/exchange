using System.Net;
using ExchangeApi.Models;

namespace ExchangeApi.ActionResults
{
    public class ErrorActionResult: ExtendedObjectResult
    {
        public ErrorActionResult(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
            : base(new ErrorResultModel(){Message = message}, statusCode)
        {
            
        }
    }
}