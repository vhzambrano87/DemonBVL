using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PromedioBE
    {
        public string nemonico { get; set; }
        public decimal? actual { get; set; }
        public decimal? promedio { get; set; }
        public decimal? promvsactual { get; set; }
    }
}
