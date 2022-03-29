using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminFolios.Models
{
    public partial class PreciosFolios
    {
        [Key]
        public int PrecioId { get; set; }
        [Column("PRECIO", TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
        [Column("CANTIDAD")]
        public int Cantidad { get; set; }
        [Required]
        [Column("DESCRIPCION")]
        [StringLength(256)]
        public string Descripcion { get; set; }
    }
}
