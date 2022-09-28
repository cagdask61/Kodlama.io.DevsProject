﻿using Application.Features.ProgrammingLanguages.Dtos.Queries;
using Core.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Models
{
    public class ProgrammingLanguageListModel : BasePageableModel
    {
        IPaginate<ProgrammingLanguageListDto> Items { get; set; }
    }
}
