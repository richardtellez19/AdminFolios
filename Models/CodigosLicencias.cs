using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminFolios.Models
{
    [Table("Codigos_Licencias")]
    public partial class CodigosLicencias
    {
        [Key]
        [Column("FOLIO")]
        public long Folio { get; set; }
        [Required]
        [Column("LICENCIA")]
        [StringLength(37)]
        public string Licencia { get; set; }
        [Column("ACTIVA")]
        [Display(Name = "Estatus")]
        public int Activa { get; set; }
        [Column("TIPO_PLAN")]
        public short TipoPlan { get; set; }
        [Display(Name = "Paquete")]
        [Column("CANT_CFDIS")]
        public int? CantCfdis { get; set; }
        [Column("CADUCIDAD_MESES")]
        public short CaducidadMeses { get; set; }
        [Required]
        [Column("RFC_CLIENTE")]
        [Display(Name = "RFC")]
        [StringLength(13)]
        public string RfcCliente { get; set; }
        [Column("FECHA_INI", TypeName = "date")]
        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        public DateTime? FechaIni { get; set; }
        [Column("FECHA_FIN", TypeName = "date")]
        [Display(Name = "Caduca")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }
        [Column("CFDIS_RESTANTES")]
        [Display(Name = "Restantes")]
        public int? CfdisRestantes { get; set; }
        [Column("GRATUITOS")]
        public bool? Gratuitos { get; set; }
    }
}
