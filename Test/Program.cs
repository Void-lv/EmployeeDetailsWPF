// See https://aka.ms/new-console-template for more information
using EmployeeApiLibrary;

internal class Program
{
    private static void Main()
    {

        Console.WriteLine("Hello, World!");

        Task.Run(async () =>
        {
            //string id = null;
            //string name = "Marut Chaturvedi";
            List<Employee> output = await EmployeeProcessor.GetEmployees(null, null);
            // Do any async anything you need here without worry

            foreach (Employee employee in output)
            {
                Console.WriteLine(employee.name);
            }



        }
        ).GetAwaiter().GetResult();
    }

}