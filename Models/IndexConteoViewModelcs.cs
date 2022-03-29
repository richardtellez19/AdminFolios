using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminFolios.Models
{
    public class IndexConteoViewModelcs : Paginador
    {
        public List<EdiCount> EdiCount { get; set; }
        public int total { get; set; }
    }
}
