using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Exceptions
{
    public class InternalProblemDetails : ProblemDetails
    {
        public string? Description { get; set; }
        public string ToStringify() => JsonConvert.SerializeObject(this);

    }
}
