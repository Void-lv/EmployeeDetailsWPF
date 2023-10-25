using EmployeeCommonLibrary;

namespace EmployeeApiLibrary
{
    public class EmployeeProcessor
    {
        public static void Initialize()
        {
            ApiHelper.InitializeClient();
        }

        public static async Task<List<Employee>> SearchEmployeesAsync(string employeeId, string name)
        {

            string resource = "users";                              // url to get list of employees

            
            if (!string.IsNullOrEmpty(employeeId))
            {
                resource = $"{resource}/{employeeId}";               // set url to get employee by id
            }

            if (string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(name))
            {
                resource = $"{resource}/?name={name}";                // set url to get employee by name
            }

            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(resource);
            if (response.IsSuccessStatusCode)
            {
                List<Employee> employees = new();

                if (string.IsNullOrEmpty(employeeId))
                {
                    employees = await response.Content.ReadAsAsync<List<Employee>>();
                }
                else
                {
                    Employee employee = await response.Content.ReadAsAsync<Employee>();
                    employees.Add(employee);
                }
                return employees;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public static async Task AddEmployeeAsync (Employee employee)
        {
            string resource = "users";
            ApiEmployee apiEmployee = new ApiEmployee(employee);

            using HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(resource, apiEmployee);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }

        public static async Task UpdateEmployeeAsync(Employee employee)
        {
            string resource = $"users/{employee.Id}";
            ApiEmployee apiEmployee = new ApiEmployee(employee);

            using HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync(resource, apiEmployee);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }

        public static async Task DeleteEmployeeAsync(Employee employee)
        {
            string resource = $"users/{employee.Id}";

            using HttpResponseMessage response = await ApiHelper.ApiClient.DeleteAsync(resource);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }

    }
}
