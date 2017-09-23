namespace DemoEFCore.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DepartmeentId { get; set; }

        public Department Department { get; set; }
    }
}
