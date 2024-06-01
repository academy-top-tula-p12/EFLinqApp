using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFLinqApp
{
    internal class Examples
    {
        public static void WhereLinq()
        {
            using (EmployeesAppContext context = new())
            {
                var employeesLang = from e in context.Employees
                                                 .Include(e => e.Company)
                                                 .Include(e => e.Position)
                                    where e.Company.Id == 1 && e.Position.Id == 3
                                    select e;
                Console.WriteLine();
                foreach (var e in employeesLang)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Company?.Title} {e.Position?.Title}");


                var employeesMethod = context.Employees
                                                   .Include(e => e.Company)
                                                   .Include(e => e.Position)
                                                   .Where(e => e.Company.Id == 1 && e.Position.Id == 3)
                                                   .ToList();
                Console.WriteLine();
                foreach (var e in employeesMethod)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Company?.Title}");


                var emplNnLinq = from e in context.Employees
                                 where EF.Functions.Like(e.Name, "%nn%")
                                 select e;
                Console.WriteLine();
                foreach (var e in emplNnLinq)
                    Console.WriteLine($"{e.Name} {e.Age}");


                var emplNnMethod = context.Employees
                                          .Where(e => EF.Functions.Like(e.Name, "%nn%"));
                Console.WriteLine();
                foreach (var e in emplNnMethod)
                    Console.WriteLine($"{e.Name} {e.Age}");

                var employee = context.Employees.Find(3);
                Console.WriteLine($"\n{employee?.Name} {employee?.Age}");

                employee = context.Employees.FirstOrDefault(e => e.Name.StartsWith("J"));
                Console.WriteLine($"\n{employee?.Name} {employee?.Age}");

                try
                {
                    employee = context.Employees.SingleOrDefault(e => e.Name.StartsWith("J"));
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Console.WriteLine($"\n{employee?.Name} {employee?.Age}");
                }


            }
        }

        public static void SelectOrder()
        {
            using (EmployeesAppContext context = new())
            {
                var employeesLang = from e in context.Employees
                                                     .Include(e => e.Position)
                                    select e;
                Console.WriteLine();
                foreach (var e in employeesLang)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Position?.Title}");

                var employeesSelectLang = from e in context.Employees
                                          orderby e.Position.Title, e.Age descending
                                          select new EmployeeModel
                                          {
                                              Name = e.Name,
                                              Age = e.Age,
                                              Position = e.Position!.Title
                                          };
                Console.WriteLine();
                foreach (var e in employeesSelectLang)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Position}");


                var employeesSelectMethod = context.Employees
                                                   .OrderByDescending(e => e.Position.Title)
                                                   .ThenBy(e => e.Age)
                                                   .Select(e => new
                                                   {
                                                       Name = e.Name,
                                                       Age = e.Age,
                                                       Position = e.Position!.Title
                                                   });

                Console.WriteLine();
                foreach (var e in employeesSelectMethod)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Position}");
            }
        }

        public static void Joins()
        {
            using (EmployeesAppContext context = new())
            {
                //var employeesLang = from e in context.Employees
                //                   join c in context.Companies
                //                        on e.Company.Id equals c.Id
                //                   select new
                //                   {
                //                       Name = e.Name,
                //                       Age = e.Age,
                //                       Company = c.Title
                //                   };

                //Console.WriteLine();
                //foreach (var e in employeesLang)
                //    Console.WriteLine($"{e.Name} {e.Age} {e.Company}");


                //var employeesMethod = context.Employees
                //                             .Join(context.Companies,
                //                             e => e.Company.Id,
                //                             c => c.Id,
                //                             (e, c) => new
                //                             {
                //                                 Name = e.Name,
                //                                 Age = e.Age,
                //                                 Company = c.Title
                //                             });

                //Console.WriteLine();
                //foreach (var e in employeesMethod)
                //    Console.WriteLine($"{e.Name} {e.Age} {e.Company}");


                var employeesLang = from e in context.Employees
                                    join c in context.Companies
                                         on e.Company.Id equals c.Id
                                    join p in context.Positions
                                         on e.Position.Id equals p.Id
                                    select new
                                    {
                                        Name = e.Name,
                                        Age = e.Age,
                                        Company = c.Title,
                                        Position = p.Title
                                    };

                Console.WriteLine();
                foreach (var e in employeesLang)
                    Console.WriteLine($"{e.Name}\t{e.Age}\t{e.Company}\t{e.Position}");


                var employeesMethod = context.Employees
                                             .Join(context.Companies,
                                             e => e.Company.Id,
                                             c => c.Id,
                                             (e, c) => new
                                             {
                                                 Name = e.Name,
                                                 Age = e.Age,
                                                 Company = c.Title,
                                                 PositionId = e.Position.Id
                                             })
                                             .Join(context.Positions,
                                             a => a.PositionId,
                                             p => p.Id,
                                             (a, p) => new
                                             {
                                                 a.Name,
                                                 a.Age,
                                                 a.Company,
                                                 Position = p.Title
                                             });

                Console.WriteLine();
                foreach (var e in employeesMethod)
                {
                    var tabs = (e.Company.Length > 8) ? "\t" : "\t\t";
                    Console.WriteLine($"{e.Name}\t{e.Age}\t{e.Company}{tabs}{e.Position}");
                }


            }
        }

        public static void Groups()
        {
            using (EmployeesAppContext context = new())
            {
                var companyGroupLinq = from e in context.Employees
                                       group e by e.Company into g
                                       select new
                                       {
                                           g.Key,
                                           Count = g.Count(),
                                           Employees = g.ToList()
                                       };

                Console.WriteLine();
                foreach (var c in companyGroupLinq)
                {
                    Console.WriteLine($"{c.Key.Title} - {c.Count}");
                    foreach (var e in c.Employees)
                        Console.WriteLine($"\t{e.Name}");
                }

                var companyGroupMethod = context.Employees
                                                .GroupBy(e => e.Company)
                                                .Select(g => new
                                                {
                                                    g.Key,
                                                    Count = g.Count(),
                                                    Employees = g.ToList(),

                                                });


                Console.WriteLine();
                foreach (var c in companyGroupMethod)
                {
                    Console.WriteLine($"{c.Key.Title} - {c.Count}");
                    foreach (var e in c.Employees)
                        Console.WriteLine($"\t{e.Name}");
                }

            }
        }

        public static void SetsOperations()
        {
            using (EmployeesAppContext context = new())
            {
                //var devs = context.Developers;
                //Console.WriteLine();
                //foreach (var dev in devs)
                //    Console.WriteLine($"{dev.Name}");

                var devs = context.Employees
                                  .Where(e => e.Company.Country.Title == "Russia");
                Console.WriteLine();
                foreach (var dev in devs)
                    Console.WriteLine($"{dev.Name}");

                var emps = context.Employees
                                  .Where(e => e.Age > 40);
                Console.WriteLine();
                foreach (var e in emps)
                    Console.WriteLine($"{e.Name}");

                //var union = context.Employees
                //                   .Where(e => e.Age > 30)
                //                   .Union(context.Developers
                //                                 .Select(d => d as Employee))
                //                   .Select(u => u as Employee);

                var union = context.Employees
                                   .Where(e => e.Age > 40)
                                   .Union(context.Employees.Where(e => e.Company.Country.Title == "Russia"));

                Console.WriteLine();
                foreach (var u in union)
                    Console.WriteLine($"{u.Name}");


                var intersect = context.Employees
                                   .Where(e => e.Age > 40)
                                   .Intersect(context.Employees.Where(e => e.Company.Country.Title == "Russia"));

                Console.WriteLine();
                foreach (var u in intersect)
                    Console.WriteLine($"{u.Name}");

                var except = context.Employees
                                   .Where(e => e.Age > 40)
                                   .Except(context.Employees.Where(e => e.Company.Country.Title == "Russia"));

                Console.WriteLine();
                foreach (var u in except)
                    Console.WriteLine($"{u.Name}");



            }
        }

        public static void AgragateOperations()
        {
            using (EmployeesAppContext context = new())
            {
                //var employeesYandex = context.Employees
                //                    .Where(e => e.Company!.Title == "Yandex");
                //Console.WriteLine();
                //foreach (var e in employeesYandex)
                //    Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");

                //var result = context.Employees
                //                    .Where(e => e.Company!.Title == "Yandex")
                //                    .Any(e => e.Age > 40);
                //Console.WriteLine($"\n{result}\n");

                //result = context.Employees
                //                    .Where(e => e.Company!.Title == "Yandex")
                //                    .All(e => e.Salary >= 50000);
                //Console.WriteLine($"\n{result}\n");


                var empls30 = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .OrderBy(e => e.Age);
                Console.WriteLine();
                foreach (var e in empls30)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");

                var empls30Count = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .Count(e => e.Age >= 30);

                Console.WriteLine($"\n{empls30Count}\n");

                var empls30Min = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .Min(e => e.Salary);

                var empls30Max = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .Max(e => e.Salary);

                var empls30Avg = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .Average(e => e.Salary);

                var empls30Sum = context.Employees
                                     .Where(e => e.Company!.Country!.Title == "Russia")
                                     .Sum(e => e.Salary);

                Console.WriteLine($"\nMin = {empls30Min} Max = {empls30Max} Avg = {empls30Avg} Sum = {empls30Sum}");



            }
        }

        public static void Tracking()
        {
            using (EmployeesAppContext context = new())
            {
                var yandex = context.Companies.FirstOrDefault(c => c.Title == "Yandex");
                var russia = context.Countries.FirstOrDefault(c => c.Title == "Russia");
                var dev = context.Positions.FirstOrDefault(p => p.Title == "Coder");

                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var employeesYandex = context.Employees
                                    .Where(e => e.Company!.Title == "Yandex");
                Console.WriteLine();
                foreach (var e in employeesYandex)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");

                //var emplYandexFirst = context.Employees.AsNoTracking()
                //                             .FirstOrDefault(e => e.Company!.Title == "Yandex");
                //Console.WriteLine($"\n{emplYandexFirst.Name} {emplYandexFirst.Age}");
                //emplYandexFirst.Age = 33;



                Developer emelya = new Developer()
                {
                    Name = "Emelya",
                    Age = 34,
                    Company = yandex,
                    Position = dev,
                    Salary = 110000
                };
                context.Developers.Add(emelya);

                context.SaveChanges();

                Console.WriteLine();
                foreach (var e in employeesYandex)
                    Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");
            }
        }

        public static void EnumQueryables()
        {
            using (EmployeesAppContext context = new())
            {
                IEnumerable<Employee> employeesE = context.Employees;
                employeesE = employeesE.Where(e => e.Age > 30);
                employeesE = employeesE.Where(e => e.Company.Country.Title == "Russia");

                foreach (var e in employeesE)
                    Console.WriteLine($"{e.Name}");
                Console.WriteLine();



                IQueryable<Employee> employeesQ = context.Employees;
                employeesQ = employeesQ.Where(e => e.Age > 30);
                employeesQ = employeesQ.Where(e => e.Company.Country.Title == "Russia");

                foreach (var e in employeesQ)
                    Console.WriteLine($"{e.Name}");
                Console.WriteLine();


            }
        }

        public static void Filters()
        {
            using (EmployeesAppContext context = new())
            {
                IQueryable<Position> positions = context.Positions;
                foreach (var p in positions)
                    Console.WriteLine($"{p.Title}");
                Console.WriteLine();

                positions = context.Positions.IgnoreQueryFilters();
                foreach (var p in positions)
                    Console.WriteLine($"{p.Title}");
                Console.WriteLine();
            }
        }
    }

    class EmployeeModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
    }
}
