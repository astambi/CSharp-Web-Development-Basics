namespace StudentSystem.Client.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using StudentSystem.Data;
    using StudentSystem.Models;

    public class Engine
    {
        private const int TotalStudents = 500;
        private const int TotalCourses = 10;
        private const int CourseDuration = 20;

        private static Random random = new Random();

        public void Run()
        {
            using (var context = new StudentSystemDbContext())
            {
                // Seed data
                SeedInitialData(context);
                SeedLicenses(context);
                Console.WriteLine("Seeding done! Running queries...");

                // Queries on the Initial Database (without licenses)
                PrintStudentsWithHomeworkSubmissions(context);
                PrintCoursesWithResources(context);
                PrintCoursesWithMoreThan5Resources(context);
                PrintActiveCourses(context);
                PrintStudentCoursesAndPrice(context);

                // Queries on Databse with Licenses
                PrintCoursesWithResourcesAndLicenses(context);
                PrintStudentWithCoursesResoursesAndLicenses(context);
            }
        }

        private void PrintStudentWithCoursesResoursesAndLicenses(StudentSystemDbContext context)
        {
            Console.WriteLine("Students with courses, resources & licenses:");

            var studentData = context
                .Students
                .Where(s => s.Courses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courses.Count,
                    Resources = s.Courses
                                .Sum(sc => sc.Course.Resources.Count),
                    Licenses = s.Courses
                                .Sum(sc => sc.Course.Resources
                                                    .Sum(r => r.Licenses.Count))
                })
                .OrderByDescending(s => s.Courses)
                .ThenByDescending(s => s.Resources)
                .ThenBy(s => s.Name)
                .ToList();

            var builder = new StringBuilder();
            foreach (var student in studentData)
            {
                builder.AppendLine($"{student.Name} - Courses {student.Courses} - Resources {student.Resources} - Licenses {student.Licenses} ");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintCoursesWithResourcesAndLicenses(StudentSystemDbContext context)
        {
            Console.WriteLine("Courses with resources & licenses: ");

            var coursesData = context
                .Courses
                .OrderByDescending(c => c.Resources.Count)
                .ThenBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources
                                .OrderByDescending(r => r.Licenses.Count)
                                .ThenBy(r => r.Name)
                                .Select(r => new
                                {
                                    r.Name,
                                    Licenses = r.Licenses.Select(l => l.Name)
                                })
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var course in coursesData)
            {
                builder.AppendLine($"{course.Name}: ");

                foreach (var resource in course.Resources)
                {
                    builder
                        .Append($"  {resource.Name} - ")
                        .AppendLine($"{string.Join(", ", resource.Licenses)}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintStudentCoursesAndPrice(StudentSystemDbContext context)
        {
            Console.WriteLine("Students Course prices:");

            var studentCourses = context
                .Students
                .Where(s => s.Courses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courses.Count,
                    TotalPrice = s.Courses.Sum(sc => sc.Course.Price),
                    AveragePrice = s.Courses.Average(sc => sc.Course.Price)
                })
                .OrderByDescending(s => s.TotalPrice)
                .ThenByDescending(s => s.Courses)
                .ThenBy(s => s.Name)
                .ToList();

            var builder = new StringBuilder();
            foreach (var student in studentCourses)
            {
                builder
                    .Append($"{student.Name} - Courses {student.Courses} - ")
                    .Append($"Total Price {student.TotalPrice:f2} - ")
                    .AppendLine($"Average Price {student.AveragePrice:f2}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintActiveCourses(StudentSystemDbContext context)
        {
            //Console.Write("Enter date in format [dd/MM/yyyy]: ");
            //var date = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var date = DateTime.Now.AddDays(11);
            Console.WriteLine($"Active courses on {date.ToShortDateString()}:");

            var activeCourses = context
                .Courses
                .Where(c => c.StartDate <= date && date <= c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = c.EndDate.Subtract(c.StartDate), // NB!
                    Students = c.Students.Count
                })
                .OrderByDescending(c => c.Students)
                .ThenByDescending(c => c.Duration)
                .ToList();

            var builder = new StringBuilder();

            foreach (var course in activeCourses)
            {
                builder
                    .Append($"  {course.Name}, ")
                    .Append($"{course.StartDate.ToShortDateString()} - {course.EndDate.ToShortDateString()}, ")
                    .Append($"Duration {course.Duration.Days}, ")
                    .AppendLine($"Students {course.Students}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintCoursesWithMoreThan5Resources(StudentSystemDbContext context)
        {
            Console.WriteLine("Courses with more than 5 resources:");

            var coursesData = context
                .Courses
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources.Count
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var course in coursesData)
            {
                builder.AppendLine($"  {course.Name} - Resources {course.Resources}");
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintCoursesWithResources(StudentSystemDbContext context)
        {
            Console.WriteLine("Courses with resources:");

            var coursesData = context
                .Courses
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    Resources = c.Resources.Select(r => new
                    {
                        r.Name,
                        r.ResourceType,
                        r.URL
                    })
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var course in coursesData)
            {
                builder.AppendLine($"{course.Name} - {course.Description}:");
                foreach (var resource in course.Resources)
                {
                    builder.AppendLine($"  {resource.Name} - {resource.ResourceType} - {resource.URL}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void PrintStudentsWithHomeworkSubmissions(StudentSystemDbContext context)
        {
            Console.WriteLine("Students with homeworks:");

            var studentsData = context
                .Students
                .Select(s => new
                {
                    s.Name,
                    Homeworks = s.Homeworks.Select(h => new
                    {
                        h.Content,
                        h.ContentType
                    })
                })
                .ToList();

            var builder = new StringBuilder();
            foreach (var student in studentsData)
            {
                builder.AppendLine($"{student.Name}:");
                foreach (var homework in student.Homeworks)
                {
                    builder.AppendLine($"  {homework.Content} - {homework.ContentType}");
                }
            }

            Console.WriteLine(builder.ToString());
        }

        private void SeedLicenses(StudentSystemDbContext context)
        {
            Console.WriteLine("Seeding Licenses...");

            var resourceIds = context
                             .Resources
                             .Select(r => r.Id)
                             .ToList();

            for (int i = 0; i < resourceIds.Count; i++)
            {
                var totalLicenses = random.Next(1, 4);

                for (int j = 0; j < totalLicenses; j++)
                {
                    context.Licenses.Add(new License
                    {
                        Name = $"License {resourceIds[i]}/{j + 1}",
                        ResourceId = resourceIds[i]
                    });
                }

                context.SaveChanges();
            }
        }

        private void SeedInitialData(StudentSystemDbContext context)
        {
            SeedStudents(context);
            var addedCourses = SeedCourses(context);
            SeedStudentsToCourses(context, addedCourses);
            SeedResources(context, addedCourses);
            SeedHomeworks(context, addedCourses);
        }

        private void SeedHomeworks(StudentSystemDbContext context, List<Course> addedCourses)
        {
            Console.WriteLine("Seeding Homeworks...");

            var contentTypes = Enum.GetValues(typeof(ContentType)).Cast<int>().ToArray();
            var currentDate = DateTime.Now;

            for (int i = 0; i < TotalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourse = currentCourse
                                      .Students
                                      .Select(s => s.StudentId)
                                      .ToList();

                foreach (var studentId in studentsInCourse)
                {
                    var totalHomeworks = random.Next(0, 5);

                    for (int j = 0; j < totalHomeworks; j++)
                    {
                        currentCourse.HomeworkSubmissions.Add(new Homework
                        {
                            StudentId = studentId,
                            Content = $"Homework {currentCourse.Id}/{studentId}/{j + 1}",
                            ContentType = (ContentType)contentTypes[random.Next(0, contentTypes.Length)],
                            SubmissionDate = currentDate.AddDays(-studentId / 10)
                        });
                    }
                }
            }

            context.SaveChanges();
        }

        private void SeedResources(StudentSystemDbContext context, List<Course> addedCourses)
        {
            Console.WriteLine("Seeding Resources...");

            var resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<int>().ToArray();

            for (int i = 0; i < TotalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var resourcesInCourse = random.Next(2, 20);

                for (int j = 0; j < resourcesInCourse; j++)
                {
                    context.Resources.Add(new Resource
                    {
                        Name = $"Resource {currentCourse.Id}/{j + 1}",
                        URL = $"www.softuni.bg/course{currentCourse.Id}/resource{j + 1}",
                        ResourceType = (ResourceType)resourceTypes[random.Next(0, resourceTypes.Length)],
                        Course = currentCourse
                    });
                }
            }

            context.SaveChanges();
        }

        private void SeedStudentsToCourses(StudentSystemDbContext context, List<Course> addedCourses)
        {
            Console.WriteLine("Seeding Students to Courses...");

            var studentIds = context.Students.Select(s => s.Id).ToList();

            for (int i = 0; i < TotalCourses; i++)
            {
                var currentCourse = addedCourses[i];
                var studentsInCourse = random.Next(10, TotalStudents);

                for (int j = 0; j < studentsInCourse; j++)
                {
                    var studentId = studentIds[random.Next(0, studentIds.Count)];

                    if (!currentCourse.Students.Any(s => s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentCourse { StudentId = studentId });
                    }
                }
            }

            context.SaveChanges();
        }

        private List<Course> SeedCourses(StudentSystemDbContext context)
        {
            Console.WriteLine("Seeding Courses...");

            var addedCourses = new List<Course>();
            var currentDate = DateTime.Now;

            for (int i = 0; i < TotalCourses; i++)
            {
                var course = new Course
                {
                    Name = $"Course {i + 1}",
                    Description = $"Course Details {i + 1}",
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddDays(random.Next(CourseDuration - 5, CourseDuration + 5)),
                    Price = 100 + i * 10
                };

                context.Courses.Add(course);
                addedCourses.Add(course);
            }

            context.SaveChanges();

            return addedCourses;
        }

        private void SeedStudents(StudentSystemDbContext context)
        {
            Console.WriteLine("Seeding Students...");

            var currentDate = DateTime.Now;

            for (int i = 0; i < TotalStudents; i++)
            {
                context.Students.Add(new Student
                {
                    Name = $"Student {i + 1}",
                    RegistrationDate = currentDate.AddDays(-random.Next(0, i / 2)),
                    Birthday = currentDate.AddYears(-18 - random.Next(0, 20)),
                    PhoneNumber = $"+359.87{i + 7}{i * 2}{i}"
                });
            }

            context.SaveChanges();
        }
    }
}
