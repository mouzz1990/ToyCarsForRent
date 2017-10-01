using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToyCars
{
    /// <summary>
    /// Логика взаимодействия для NewToyCarWindow.xaml
    /// </summary>
    public partial class NewToyCarWindow : Window
    {
        NewToyCarViewModel vm;
        ObservableCollection<ToyCar> ToyCars;

        public NewToyCarWindow(ObservableCollection<ToyCar> toyCars, bool editMode, ToyCar selectedCar = null)
        {
            InitializeComponent();
            ToyCars = toyCars;
            vm = new NewToyCarViewModel(ToyCars, selectedCar);
            DataContext = vm;

            vm.CloseWindowRequest += Vm_CloseWindowRequest;

            if (!editMode)
            {
                btnChangeInfo.Visibility = Visibility.Hidden;
            }
            else
            {
                btnAddNew.Visibility = Visibility.Hidden;
            }
                
            
        }

        private void Vm_CloseWindowRequest(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
