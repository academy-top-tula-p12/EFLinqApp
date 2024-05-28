using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    public enum Quality
    {
        Junior,
        Middle,
        Senior
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public decimal? Salary { get; set; }

        public Position? Position { get; set; }
        public Company? Company { get; set; }

        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }

    public class Manager : Employee
    {
        public double SaleRate { get; set; }
    }

    public class Developer : Employee
    {
        public Quality Quality { get; set; }
    }

    public class Position
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public IEnumerable<Employee> Employees { get; set;} = new List<Employee>();
    }

    public class Project
    {
        public int Id { get; set; }
        public string? Title { get; set; } = null!;

        public DateTime? DeadLine { get; set; }
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class Company
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public Country? Country { get; set; }
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class Country
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public IEnumerable<Company> Employees { get; set; } = new List<Company>();
    }
}
