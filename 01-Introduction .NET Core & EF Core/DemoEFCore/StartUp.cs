namespace DemoEFCore
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new MyDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                ////context.People.Add(new Person { Name = "Pesho", Age = 22 });

                //var department = new Department { Name = "C-Level" };

                //for (int i = 0; i < 10; i++)
                //{
                //    department.Employees
                //        .Add(new Employee { Name = $"Employee N {i}" });
                //}

                //context.Departments.Add(department);
                //context.SaveChanges();

                int departmentId = 1;

                // Include, ThenInclude
                var department = context
                    .Departments
                    .Include(d => d.Employees)
                        //.ThenInclude(e => e.Address)
                    .FirstOrDefault(d => d.Id == departmentId);

                Console.WriteLine($"{department.Name} {department.Employees.Count}");

                // Explicit loading
                var department2 = context
                   .Departments
                   .FirstOrDefault(d => d.Id == departmentId);

                context
                    .Entry(department)
                    .Collection(d => d.Employees)
                    .Load();

                var employees = department.Employees;

                Console.WriteLine($"{department2.Name} {department2.Employees.Count}");

                var bEmpl = context
                    .Entry(department)
                    .Collection(d => d.Employees)
                    .Query()
                    .Where(e => e.Name.StartsWith("E"));

                foreach (var emp in bEmpl)
                {
                    Console.WriteLine(emp.Name);
                }


                // Projection => preferable
                var result = context.Departments
                    .Where(d => d.Id == departmentId)
                    .Select(d => new
                    {
                        d.Name,
                        Employees = d.Employees.Count
                    })
                    .FirstOrDefault();

                Console.WriteLine($"{result.Name} {result.Employees}");

            }
        }
    }
}
