using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.CrossCuttingConcers.Exceptions
{
    public class AuthorizationProblemDetails : ProblemDetails
    {
        public string ToStringify() => JsonConvert.SerializeObject(this);
    }
}
