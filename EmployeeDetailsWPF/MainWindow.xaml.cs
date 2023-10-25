using EmployeeCommonLibrary;
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
        }
    }
}
