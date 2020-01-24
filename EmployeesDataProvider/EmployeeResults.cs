using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesDataProvider
{
    public class EmployeeResults
    {
        public int Count { get; set; }
        public List<Employee> Employee { get; set; }
}

    public class Employee
    {
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }

        public int Status { get; set; }

        public Guid id { get; set; }
    }
}
