using Microsoft.EntityFrameworkCore;

namespace TestPVBai2.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<OTP> OTPs { get; set; }
    }
}
