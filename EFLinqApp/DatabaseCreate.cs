using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    public class DatabaseCreate
    {
        public static void Create()
        {
            List<Country> countries = new List<Country>()
            {
                new(){ Title = "Russia" },
                new(){ Title = "China" },
                new(){ Title = "Usa" }
            };

            List<Company> companies = new List<Company>()
            {
                new() { Title = "Yandex", Country = countries[0] },
                new() { Title = "Huaway", Country = countries[1] },
                new() { Title = "Google", Country = countries[2] },
                new() { Title = "Ozon", Country = countries[0] },
                new() { Title = "Avito", Country = countries[0] },
                new() { Title = "Microsoft", Country = countries[2] },
                new() { Title = "Xiaomi", Country = countries[1] }
            };

            List<Position> positions = new List<Position>()
            {
                new(){ Title = "Hall manager" },
                new(){ Title = "Saler" },
                new(){ Title = "Coder" },
                new(){ Title = "Tester" },
                new(){ Title = "Team Leader" },
                new(){ Title = "Driver" },
                new(){ Title = "Buhgalter" }
            };

            List<Project> projects = new List<Project>()
            {
                new() { Title = "Contract GazProm", DeadLine = new(2024, 9, 10) },
                new() { Title = "Saleout Tecvhnics", DeadLine = new(2024, 6, 21) },
                new() { Title = "Mobile App ToDo", DeadLine = new(2024, 7, 5) },
                new() { Title = "Website InterShop", DeadLine = new(2024, 8, 12) },
                new() { Title = "Desktop App Calculator", DeadLine = new(2024, 7, 22) },
                new() { Title = "Mobile App RuTinder", DeadLine = new(2024, 9, 30) },
                new() { Title = "Contract MilkFactory", DeadLine = new(2024, 10, 2) },
            };

            List<Employee> employees = new List<Employee>()
            {
                new() { Name = "Ivan", Age = 42, Company = companies[0], Position = positions[5], Salary = 50000 },
                new() { Name = "Anna", Age = 30, Company = companies[1], Position = positions[6], Salary = 89000 },
                new() { Name = "Petr", Age = 29, Company = companies[1], Position = positions[5], Salary = 65000 },
                new() { Name = "Sveta", Age = 22, Company = companies[0], Position = positions[6], Salary = 91000 }
            };

            List<Manager> managers = new List<Manager>()
            {
                new() { Name = "Oleg", Age = 25, Company = companies[0], Position = positions[2], Salary = 75000 },
                new() { Name = "Sergey", Age = 33, Company = companies[1], Position = positions[2], Salary = 82000 },
                new() { Name = "Vitaly", Age = 41, Company = companies[1], Position = positions[3], Salary = 64000 },
                new() { Name = "Maxim", Age = 34, Company = companies[0], Position = positions[3], Salary = 98000 },
                new() { Name = "Kirill", Age = 43, Company = companies[2], Position = positions[4], Salary = 110000 },
                new() { Name = "Dmitry", Age = 25, Company = companies[3], Position = positions[4], Salary = 93000 },
                new() { Name = "Leonid", Age = 36, Company = companies[4], Position = positions[2], Salary = 105000 },
                new() { Name = "Michail", Age = 22, Company = companies[3], Position = positions[3], Salary = 77000 },
                new() { Name = "Rita", Age = 26, Company = companies[4], Position = positions[4], Salary = 115000 },
                new() { Name = "Galla", Age = 25, Company = companies[5], Position = positions[4], Salary = 97000 },
                new() { Name = "Pavel", Age = 45, Company = companies[4], Position = positions[3], Salary = 100000 },
                new() { Name = "Evgeny", Age = 31, Company = companies[5], Position = positions[2], Salary = 84000 },

            };

            List<Developer> developers = new List<Developer>()
            {
                new() { Name = "Billy", Age = 33, Company = companies[0], Position = positions[0], Salary = 150000 },
                new() { Name = "Jonny", Age = 42, Company = companies[1], Position = positions[0], Salary = 120000 },
                new() { Name = "Sammy", Age = 41, Company = companies[1], Position = positions[1], Salary = 90000 },
                new() { Name = "Bobby", Age = 34, Company = companies[0], Position = positions[1], Salary = 210000 },
                new() { Name = "Tommy", Age = 43, Company = companies[2], Position = positions[0], Salary = 135000 },
                new() { Name = "Jimmy", Age = 25, Company = companies[3], Position = positions[0], Salary = 175000 },
                new() { Name = "Vinny", Age = 36, Company = companies[4], Position = positions[1], Salary = 132000 },
                new() { Name = "Donny", Age = 22, Company = companies[3], Position = positions[1], Salary = 95000 },
                new() { Name = "Sonny", Age = 26, Company = companies[4], Position = positions[0], Salary = 140000 },
                new() { Name = "Olly", Age = 25, Company = companies[5], Position = positions[0], Salary = 162000 },
                new() { Name = "Nikky", Age = 45, Company = companies[4], Position = positions[1], Salary = 114000 },
                new() { Name = "Willy", Age = 31, Company = companies[5], Position = positions[1], Salary = 162000 },
            };

            Random random = new();
            for(int i = 0; i < projects.Count; i++)
            {
                HashSet<Employee> hash = new();
                for (int e = 0; e < random.Next(3); e++)
                    hash.Add(employees[random.Next(employees.Count)]);
                projects[i].Employees.AddRange(hash);

                hash.Clear();
                for (int e = 0; e < random.Next(5); e++)
                    hash.Add(managers[random.Next(managers.Count)]);
                projects[i].Employees.AddRange(hash);

                hash.Clear();
                for (int e = 0; e < random.Next(8); e++)
                    hash.Add(developers[random.Next(developers.Count)]);
                projects[i].Employees.AddRange(hash);
            }

            using(EmployeesAppContext context = new())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Employees.AddRange(employees);
                context.Managers.AddRange(managers);
                context.Developers.AddRange(developers);
                context.Countries.AddRange(countries);
                context.Companies.AddRange(companies);
                context.Positions.AddRange(positions);
                context.Projects.AddRange(projects);

                context.SaveChanges();
            }

        }
    }
}
