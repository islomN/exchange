using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApi.ActionResults
{
    public class ExtendedObjectResult: ObjectResult
    {
        protected ExtendedObjectResult(object value, HttpStatusCode statusCode):base(value)
        {
            StatusCode = (int)statusCode;
        }
    }
}