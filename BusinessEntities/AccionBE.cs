using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class AccionBE
    {
        public int id { get; set; }
        public string nemonico { get; set; }
        public string fecha { get; set; }
        public decimal? valor { get; set; }
        public decimal? monto_negociado { get; set; }
        public decimal? num_acciones { get; set; }
        public decimal? maximo { get; set; }
        public decimal? minimo { get; set; }
    }
}
