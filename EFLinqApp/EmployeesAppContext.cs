using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    internal class EmployeesAppContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Manager> Managers { get; set; } = null!;
        public DbSet<Developer> Developers { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;

        public IQueryable<Employee> GetEmployeeById(int id = 1)
        {
            return FromExpression(() => GetEmployeeById(id));
        }
            

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HrsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Position>()
                        .Property(p => p.Activity)
                        .HasDefaultValue(true);

            modelBuilder.Entity<Position>()
                        .HasQueryFilter(p => p.Activity);

            modelBuilder.HasDbFunction(() => GetEmployeeById(default));
        }
    }
}
