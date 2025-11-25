using Microsoft.EntityFrameworkCore;
using Library.ChartingSystem.Models;

namespace API.ChartingSystem.Database
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
    }
}
