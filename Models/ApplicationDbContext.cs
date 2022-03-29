using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AdminFolios.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EdiCount> EdiCount { get; set; }
        public virtual DbSet<CodigosLicencias> CodigosLicencias { get; set; }
        public virtual DbSet<PreciosFolios> PreciosFolios { get; set; }
        public virtual DbSet<TipoCods> TipoCods { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            //if (!optionsBuilder.IsConfigured)
//           // {
////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("data source=DESKTOP-VILK201\\SQLEXPRESS;initial catalog=Licencias;Integrated Security=True", builder => builder.UseRowNumberForPaging());
//            //}
//        }

        //        protected override void OnModelCreating(ModelBuilder modelBuilder)
        //        {
        //            OnModelCreatingPartial(modelBuilder);
        //        }

        //        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
