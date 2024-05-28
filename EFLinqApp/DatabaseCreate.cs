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
            List<Employee> employees = new List<Employee>();
            List<Manager> manager = new List<Manager>();
            List<Developer> developer = new List<Developer>();
            
            List<Country> country = new List<Country>()
            {
                new(){ Title = "Russia" },
                new(){ Title = "China" },
                new(){ Title = "Usa" }
            };
            
            List<Company> company = new List<Company>()
            {
                new() { Title = "Yandex", Country = country[0] },
                new() { Title = "Huaway", Country = country[1] },
                new() { Title = "Google", Country = country[2] },
                new() { Title = "Ozon", Country = country[0] },
                new() { Title = "Avito", Country = country[0] },
                new() { Title = "Microsoft", Country = country[2] },
                new() { Title = "Xiaomi", Country = country[1] }
            };

            List<Position> position = new List<Position>()
            {
                new(){ Title = "Hall manager" },
                new(){ Title = "Saler" },
                new(){ Title = "Coder" },
                new(){ Title = "Tester" },
                new(){ Title = "Team Leader" },
                new(){ Title = "Driver" },
                new(){ Title = "Buhgalter" }
            };

            List<Project> projects = new List<Project>();
            

            
        }
    }
}
