﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class EmpresaBE
    {
        public int id { get; set; }        
        public string nemonico { get; set; }
        public string nombre { get; set; }
        public string categoria { get; set; }

        public int excel { get; set; }
    }
}
