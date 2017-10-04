using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class CotizacionBE
    {
        public string valBids { get; set; }
        public string valAmt { get; set; }
        public string valAmtDol { get; set; }
        public string valHighs { get; set; }
        public string dscMoneda { get; set; }
        public string valPts { get; set; }
        public string valNop { get; set; }
        public string fecIni { get; set; }
        public string fecFin { get; set; }
        public string valAsks { get; set; }
        public string fecTims { get; set; }
        public string codIsin { get; set; }
        public string valLasts { get; set; }
        public string fecTimp { get; set; }
        public object valExd { get; set; }
        public string dscTipoBene { get; set; }
        public string valAmtSol { get; set; }
        public string valOpen { get; set; }
        public string valVol { get; set; }
        public string fecDt { get; set; }
        public string codTkr { get; set; }
        public string valLows { get; set; }
        public string valCorracum { get; set; }
    }

    public class RootCotizacion
    {
        public List<CotizacionBE> data { get; set; }
        public string message { get; set; }
        public int status { get; set; }
        public string timestamp { get; set; }
    }
}
