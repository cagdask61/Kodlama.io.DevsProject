using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ElasticSearch.Models
{
    public class ElasticSearchGetModel<TItem>
    {
        public string ElasticId { get; set; }
        public TItem Item { get; set; }
    }
}
