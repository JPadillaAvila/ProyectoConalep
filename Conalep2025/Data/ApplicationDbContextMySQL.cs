using Conalep2025;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Conalep2025.Models;



namespace Conalep2025.Data

{

    //public class MyService
    //{
    //    private readonly IConfiguration _configuration;

    //    public MyService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //}
    public class ApplicationDbContextMySQL : IdentityDbContext
    {
        public ApplicationDbContextMySQL(DbContextOptions<ApplicationDbContextMySQL> options)
            : base(options)
        {
        }
        //public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        //public DbSet<SatIdData> SatId { get; set; }

        //public DbSet<reg_nominas> reg_nominas { get; set; }
        //public DbSet<SericaHeaderModel> sericaReporteModels { get; set; }
        //public DbSet<SericaDetalleReporteModel> sericaDetalleReporteModels { get; set; }
        //public DbSet<PartidasModel> partidasModels { get; set; }
        //public DbSet<LineaModel> lineaModels { get; set; }
        //public DbSet<FovissteReportModel> fovissteReportModels { get; set; }
        //public DbSet<FonacotReportModel> fonacotReportsModels { get; set; }
        
    //public DbSet<LineaArchivo> LineasArchivo { get; set; }

    }
}
