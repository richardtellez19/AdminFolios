using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminFolios.Models
{
    public partial class EdiCount
    {
        [Key]
        public long EdiCountId { get; set; }
        [Required]
        [Column("RFC")]
        [StringLength(13)]
        public string Rfc { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Conteo { get; set; }
    }
}
