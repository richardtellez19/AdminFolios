using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminFolios.Models
{
    public partial class TipoCods
    {
        [Key]
        public int TipoId { get; set; }
        [Required]
        [Column("DESCRIPCION")]
        [StringLength(256)]
        public string Descripcion { get; set; }
    }
}
