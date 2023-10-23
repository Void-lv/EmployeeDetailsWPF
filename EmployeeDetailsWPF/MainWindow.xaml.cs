using CsvHelper;
using EmployeeApiLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace EmployeeDetailsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Employee> _employees;
        public List<Employee> Employees { get { return _employees; } set { _employees = value;} }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee { get { return _selectedEmployee; } set { _selectedEmployee = value; } }

        public MainWindow()
        {
            
            InitializeComponent();
            EmployeeProcessor.Initialize();

        }

        private async void ButtonSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            TextBlockMessage.Visibility = Visibility.Visible;
            TextBlockMessage.Text = "Loading ...";
            DataGridEmployees.Visibility = Visibility.Collapsed;
            GridEmployeeDetails.Visibility = Visibility.Collapsed;
            
            try
            {
                Employees = await EmployeeProcessor.SearchEmployeesAsync(TextBoxId.Text, TextBoxName.Text);

                DataGridEmployees.ItemsSource = Employees;

                ButtonExport.Visibility = Visibility.Visible;

                TextBlockMessage.Visibility = Visibility.Collapsed;
                DataGridEmployees.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
      

        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            string filename = "employee_export_" + DateTime.Now.ToString("yyMMddHHmmss") + ".csv";

            try
            {
                using (var writer = new StreamWriter(filename))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(Employees);
                }

                MessageBox.Show(filename + " created.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void ButtonNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            GridEmployeeDetails.Visibility = Visibility.Visible;
            DataGridEmployees.Visibility = Visibility.Collapsed;
            ButtonSubmitNewEmployee.Visibility = Visibility.Visible;

            DataGridEmployees.SelectedItem = null;

            TextBoxNewName.Text = null;
            TextBoxNewEmail.Text = null;
            ComboBoxStatus.Text = null;
            ComboBoxGender.Text = null;
        }

        private async void ButtonSubmitNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee = new()
            {
                name = TextBoxNewName.Text,
                email = TextBoxNewEmail.Text,
                status = ComboBoxStatus.Text,
                gender = ComboBoxGender.Text,
            };

            try
            {
                await EmployeeProcessor.AddEmployeeAsync(newEmployee);
                
                MessageBox.Show(newEmployee.name + " created.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                TextBoxNewName.Text = null;
                TextBoxNewEmail.Text = null;
                ComboBoxStatus.Text = null;
                ComboBoxGender.Text = null;
            }


        }



        private void ButtonSelectUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            SelectedEmployee = (Employee)DataGridEmployees.SelectedItem;

            TextBoxNewName.Text = SelectedEmployee.name;
            TextBoxNewEmail.Text = SelectedEmployee.email;
            ComboBoxStatus.Text = SelectedEmployee.status;
            ComboBoxGender.Text = SelectedEmployee.gender;

            GridEmployeeDetails.Visibility = Visibility.Visible;
            DataGridEmployees.Visibility = Visibility.Collapsed;

            DataGridEmployees.SelectedItem = null;



        }

        private async void ButtonUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee updateEmployee = new()
            {
                id = SelectedEmployee.id,
                name = TextBoxNewName.Text,
                email = TextBoxNewEmail.Text,
                status = ComboBoxStatus.Text,
                gender = ComboBoxGender.Text,
            };

            try
            {
                await EmployeeProcessor.UpdateEmployeeAsync(updateEmployee);

                MessageBox.Show(updateEmployee.name + " updated.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                TextBoxNewName.Text = null;
                TextBoxNewEmail.Text = null;
                ComboBoxStatus.Text = null;
                ComboBoxGender.Text = null;

                SelectedEmployee = null;
            }
        }



        private async void ButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

            SelectedEmployee = (Employee)DataGridEmployees.SelectedItem;

            if (SelectedEmployee == null) MessageBox.Show("Please select employee to delete");
            else
            {
                try
                {
                    await EmployeeProcessor.DeleteEmployeeAsync(SelectedEmployee);

                    MessageBox.Show(SelectedEmployee.name + " deleted.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
