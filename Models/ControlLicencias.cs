using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminFolios.Models
{
    [Table("Control_Licencias")]
    public partial class ControlLicencias
    {
        [Key]
        [Column("LICENCIA")]
        public Guid Licencia { get; set; }
        [Column("NO_SERIE")]
        [StringLength(20)]
        public string NoSerie { get; set; }
        [Column("NO_HD", TypeName = "text")]
        public string NoHd { get; set; }
        [Column("STATUS")]
        [StringLength(2)]
        public string Status { get; set; }
        [Column("NO_USUARIOS")]
        public int? NoUsuarios { get; set; }
        [Column("CLIENTE", TypeName = "text")]
        public string Cliente { get; set; }
        [Column("RFC")]
        [StringLength(13)]
        public string Rfc { get; set; }
        [Column("MEDIO")]
        [StringLength(20)]
        public string Medio { get; set; }
    }
}
