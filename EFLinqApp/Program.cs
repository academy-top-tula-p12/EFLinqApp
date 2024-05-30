using Azure;
using EFLinqApp;
using Microsoft.EntityFrameworkCore;


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
                       .Where(e => e.Age > 30)
                       .Union(context.Employees.Where(e => e.Company.Country.Title == "Russia"));

    Console.WriteLine();
    foreach (var u in union)
        Console.WriteLine($"{u.Name}");


}


