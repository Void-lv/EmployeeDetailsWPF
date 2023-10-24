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

        private List<Employee> employees;

        private Employee selectedEmployee;

        
        public MainWindow()
        {
            
            InitializeComponent();
            EmployeeProcessor.Initialize();

        }

        
        private async void ButtonSearchEmployees_Click(object sender, RoutedEventArgs e)
        {
            TextBlockMessage.Visibility = Visibility.Visible;
            TextBlockMessage.Text = "Loading ...";
            DataGridEmployees.Visibility = Visibility.Hidden;
            GridEmployeeDetails.Visibility = Visibility.Hidden;
            
            try
            {
                employees = await EmployeeProcessor.SearchEmployeesAsync(TextBoxId.Text, TextBoxName.Text);

                DataGridEmployees.ItemsSource = employees;

                ButtonExport.Visibility = Visibility.Visible;

                TextBlockMessage.Visibility = Visibility.Hidden;
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
                    csv.WriteRecords(employees);
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
            DataGridEmployees.Visibility = Visibility.Hidden;
            
            ButtonSubmitNewEmployee.Visibility = Visibility.Visible;
            ButtonUpdateEmployee.Visibility = Visibility.Hidden;
            ButtonExport.Visibility = Visibility.Hidden;

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
            selectedEmployee = (Employee)DataGridEmployees.SelectedItem;

            ButtonSubmitNewEmployee.Visibility = Visibility.Hidden;
            ButtonExport.Visibility = Visibility.Hidden;
            ButtonUpdateEmployee.Visibility = Visibility.Visible;

            TextBoxNewName.Text = selectedEmployee.name;
            TextBoxNewEmail.Text = selectedEmployee.email;
            ComboBoxStatus.Text = selectedEmployee.status;
            ComboBoxGender.Text = selectedEmployee.gender;

            GridEmployeeDetails.Visibility = Visibility.Visible;
            DataGridEmployees.Visibility = Visibility.Hidden;

            DataGridEmployees.SelectedItem = null;



        }

        private async void ButtonUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee updateEmployee = new()
            {
                id = selectedEmployee.id,
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

                selectedEmployee = null;

                GridEmployeeDetails.Visibility = Visibility.Hidden;
            }
        }



        private async void ButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

            selectedEmployee = (Employee)DataGridEmployees.SelectedItem;

            if (selectedEmployee == null) MessageBox.Show("Please select employee to delete");
            else
            {
                try
                {
                    await EmployeeProcessor.DeleteEmployeeAsync(selectedEmployee);

                    MessageBox.Show(selectedEmployee.name + " deleted.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
