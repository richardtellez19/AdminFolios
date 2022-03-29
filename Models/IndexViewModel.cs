using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminFolios.Models
{
    public class IndexViewModel: Paginador
    {
        public List<CodigosLicencias> CodigosLicencias { get; set; }
    }
}
