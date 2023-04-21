using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Xml;
using TimeX.Converters;
using TimeX.Models;

namespace TimeX.Data
{
    public class TimeXDbContext: IdentityDbContext
    {
        public TimeXDbContext(DbContextOptions options):base(options)
        {

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateOnly>()
                                .HaveConversion<DateOnlyEFConverter>()
                                .HaveColumnType("date");

            configurationBuilder.Properties<TimeOnly>()
                      .HaveConversion<TimeOnlyEFConverter>()
                      .HaveColumnType("time");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Business>()
                .HasMany(b => b.Facilities)
                .WithOne(f => f.Business)
                .HasForeignKey(f => f.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
           }


        public DbSet<Admin> Admin { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<Customer> Customer { get; set; }



    }
}
