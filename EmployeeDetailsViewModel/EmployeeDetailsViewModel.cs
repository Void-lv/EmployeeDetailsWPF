using CsvHelper;
using EmployeeApiLibrary;
using EmployeeCommonLibrary;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Globalization;
using System.Windows.Input;

namespace EmployeeDetailsViewModel
{
    public class EmployeeDetailsViewModel : INotifyPropertyChanged
    {
        private string? employeeId;
        public string? EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }
        private string? employeeName;
        public string? EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        private Employee? processingEmployee;
        public Employee? ProcessingEmployee
        {
            get { return processingEmployee; }
            set
            {
                processingEmployee = value;
                OnPropertyChanged(nameof(ProcessingEmployee));
            }
        }

        private Employee? selectedEmployee;
        public Employee? SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        private List<Employee>? employeeList;
        public List<Employee>? EmployeeList
        {
            get { return employeeList; }
            set
            {
                employeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }

        public Employee? newEmployee;

        private bool detailFormActive;
        public bool DetailFormActive
        {
            get { return detailFormActive; }
            set
            {
                detailFormActive = value;
                OnPropertyChanged(nameof(DetailFormActive));
            }
        }

        private string? dialogueMessage;
        public string? DialogueMessage
        {
            get { return dialogueMessage; }
            set
            {
                dialogueMessage = value;
                OnPropertyChanged(nameof(DialogueMessage));
            }
        }

        private string? operationType;
        public string? OperationType
        {
            get { return operationType; }
            set
            {
                operationType = value;
                OnPropertyChanged(nameof(OperationType));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DelegateCommand SearchEmployeesCommand {  get; set; }
        public DelegateCommand PrepareNewEmployeeCommand { get; set; }
        public DelegateCommand PrepareUpdateEmployeeCommand { get; set; }
        public DelegateCommand SubmitEmployeeOperationCommand { get; set; }
        public DelegateCommand DeleteEmployeeCommand { get; set; }
        public DelegateCommand ExportCommand { get; set; }

        public EmployeeDetailsViewModel()
        {

            EmployeeProcessor.Initialize();

            SearchEmployeesCommand = new DelegateCommand(SearchEmployees);
            PrepareNewEmployeeCommand = new DelegateCommand(PrepareNewEmployee);
            PrepareUpdateEmployeeCommand = new DelegateCommand(PrepareUpdateEmployee);
            SubmitEmployeeOperationCommand = new DelegateCommand(SubmitEmployeeOperation);
            DeleteEmployeeCommand = new DelegateCommand(DeleteEmployee);
            ExportCommand = new DelegateCommand(Export);
        }


        private async void SearchEmployees()
        {
            DialogueMessage = "Loading ...";
            DetailFormActive = false;

            try
            {
                EmployeeList = await EmployeeProcessor.SearchEmployeesAsync(EmployeeId, EmployeeName);
            }
            catch (Exception ex)
            {
                DialogueMessage = ex.Message;
            }

            EmployeeName = null;
            DialogueMessage = "Select customer to update or delete.";
        }


        private void PrepareNewEmployee()  
        {
            EmployeeList = null;
            ProcessingEmployee = new Employee();
            DetailFormActive = true;
            DialogueMessage = "Fill in new employee details";
            OperationType = "create";
        }

        public void PrepareUpdateEmployee() 
        {
            DetailFormActive = true;
            ProcessingEmployee = SelectedEmployee;
            OperationType = "update";
            selectedEmployee = null;
            EmployeeList = null;
        }

        public async void SubmitEmployeeOperation()
        {
            DialogueMessage = null;     //reset dialog message
            try
            {
                if (OperationType == "create") await EmployeeProcessor.AddEmployeeAsync(new ApiEmployee(ProcessingEmployee));
                if (OperationType == "update") await EmployeeProcessor.UpdateEmployeeAsync(new ApiEmployee(ProcessingEmployee));
                DialogueMessage = $"{ProcessingEmployee.Name} {OperationType}d";
            }
            catch (Exception ex)
            {
                DialogueMessage = ex.Message;
            }
        }

        public async void DeleteEmployee()
        {
            try
            {
                await EmployeeProcessor.DeleteEmployeeAsync(new ApiEmployee(SelectedEmployee));
                DialogueMessage = $"{SelectedEmployee.Name} deleted.";

            }
            catch (Exception ex)
            {
                DialogueMessage = ex.Message;
            }
        }

        private void Export()
        {
            string filename = $"employee_export_{DateTime.Now.ToString("yyMMddHHmmss")}.csv";

            try
            {
                using (var writer = new StreamWriter(filename))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(EmployeeList);
                }
                DialogueMessage = $"{filename} created.";
            }
            catch (Exception ex)
            {
                DialogueMessage = ex.Message;
            }
        }
    }
}

    