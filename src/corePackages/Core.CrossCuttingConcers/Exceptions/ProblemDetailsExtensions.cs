using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Exceptions
{
    public static class ProblemDetailsExtensions
    {
        public static string ToStringifyTest(this ProblemDetails problemDetails)
        {
            return JsonConvert.SerializeObject(problemDetails);
        }
    }
}
