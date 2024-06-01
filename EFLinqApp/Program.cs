using Azure;
using EFLinqApp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using (EmployeesAppContext context = new())
{
    //context.Employees
    //       .Where(e => e.Company.Title == "Yandex")
    //       .ExecuteUpdate(e => e.SetProperty(
    //                                em => em.Salary,
    //                                em => em.Salary * 1.1m
    //                               )
    //                             .SetProperty(
    //                                em => em.Age,
    //                                em => em.Age + 1
    //                             ));

    //context.Employees
    //       .Where(e => e.Company.Title == "PiterSoft")
    //       .ExecuteDelete();

    //SqlParameter paramId = new SqlParameter("@id", 10);

    //var employee = context.Employees
    //                      .FromSqlRaw("SELECT * FROM [dbo].[GetEmployeeById] (@id)", paramId)
    //                      .ToList();
    //foreach(var e in employee)
    //    Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");

    var employeeOlly = context.GetEmployeeById(10);
    foreach (var e in employeeOlly)
        Console.WriteLine($"{e.Name} {e.Age} {e.Salary}");
}



