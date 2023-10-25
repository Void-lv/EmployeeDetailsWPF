using EmployeeCommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApiLibrary
{

    public class ApiEmployee
    {
        public string? id;
        public string? name;
        public string? email;
        public string? gender;
        public string? status;

        public ApiEmployee(Employee employee)
        {
            id = employee.Id;
            name = employee.Name;
            email = employee.Email;
            gender = employee.Gender;
            status = employee.Status;
        }
    }
}
