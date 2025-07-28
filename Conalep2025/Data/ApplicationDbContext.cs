using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Conalep2025.Models;


namespace Conalep2025.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<SatIdData> SatId { get; set; }
        public DbSet<reg_nominas> reg_nominas { get; set; }
        public object LineasArchivo { get; internal set; }
    }
}
