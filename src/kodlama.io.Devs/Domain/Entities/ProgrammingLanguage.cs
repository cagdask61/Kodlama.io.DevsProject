using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProgrammingLanguage : CommonEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public ProgrammingLanguage()
        {

        }


        public ProgrammingLanguage(int id,string name, string description, bool status) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
        }
    }
}
