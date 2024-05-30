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
    }

    class EmployeeModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
    }
}
