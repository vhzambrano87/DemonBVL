using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DataDetBE
    {
        public string FECHA { get; set; }
        public decimal? ENGIEC1_VAL { get; set; }
        public decimal? ENGIEC1_VAL_MIN { get; set; }
        public decimal? ENGIEC1_VAL_MAX { get; set; }
        public decimal? ENGIEC1_MONTO { get; set; }
        public decimal? CORAREI1_VAL { get; set; }
        public decimal? CORAREI1_VAL_MIN { get; set; }
        public decimal? CORAREI1_VAL_MAX { get; set; }
        public decimal? CORAREI1_MONTO { get; set; }
        public decimal? FERREYC1_VAL { get; set; }
        public decimal? FERREYC1_VAL_MIN { get; set; }
        public decimal? FERREYC1_VAL_MAX { get; set; }
        public decimal? FERREYC1_MONTO { get; set; }
        public decimal? MILPOC1_VAL { get; set; }
        public decimal? MILPOC1_VAL_MIN { get; set; }
        public decimal? MILPOC1_VAL_MAX { get; set; }
        public decimal? MILPOC1_MONTO { get; set; }
        public decimal? MOROCOI1_VAL { get; set; }
        public decimal? MOROCOI1_VAL_MIN { get; set; }
        public decimal? MOROCOI1_VAL_MAX { get; set; }
        public decimal? MOROCOI1_MONTO { get; set; }
        public decimal? RELAPAC1_VAL { get; set; }
        public decimal? RELAPAC1_VAL_MIN { get; set; }
        public decimal? RELAPAC1_VAL_MAX { get; set; }
        public decimal? RELAPAC1_MONTO { get; set; }
        public decimal? SIDERC1_VAL { get; set; }
        public decimal? SIDERC1_VAL_MIN { get; set; }
        public decimal? SIDERC1_VAL_MAX { get; set; }
        public decimal? SIDERC1_MONTO { get; set; }
        public decimal? VOLCABC1_VAL { get; set; }
        public decimal? VOLCABC1_VAL_MIN { get; set; }
        public decimal? VOLCABC1_VAL_MAX { get; set; }
        public decimal? VOLCABC1_MONTO { get; set; }
    }
    public class DataAccionBE
    {
        public string FECHA { get; set; }
        public List<DataAccionDetBE> LIST_DATA_ACCION_DET { get; set; }
    }
    public class DataAccionDetBE
    {        
        public string ACCION { get; set; }
        public decimal? CIERRE { get; set; }
        public decimal? MIN { get; set; }
        public decimal? MAX { get; set; }
        public decimal? MONTO { get; set; }
    }
}