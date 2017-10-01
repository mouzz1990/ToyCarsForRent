using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ToyCars
{
    public class ViewModel : INotifyPropertyChanged
    {
        ToyCarsContext context =  new ToyCarsContext();
        RentCarInformationManager rentManager;

        public ViewModel()
        {
            rentManager = new RentCarInformationManager();
            
            //Temp adding in DB for test
            context.ToyCars.Add(new ToyCar() { Title = "1", ImageUri = @"C:\images\1.jpg" });
            context.ToyCars.Add(new ToyCar() { Title = "2", ImageUri = @"C:\images\2.jpg" });
            context.ToyCars.Add(new ToyCar() { Title = "3", ImageUri = @"C:\images\3.jpg" });
            context.ToyCars.Add(new ToyCar() { Title = "4", ImageUri = @"C:\images\4.jpg" });
            context.ToyCars.Add(new ToyCar() { Title = "5", ImageUri = @"C:\images\5.jpg" });
            context.ToyCars.Add(new ToyCar() { Title = "6", ImageUri = @"C:\images\6.jpg" });

            context.SaveChanges();
            ///////////remove that block before release

            ToyCars = new ObservableCollection<ToyCar>(context.ToyCars);
            foreach (var car in ToyCars)
                car.GetTodayCash(context);

            //Values in textbox Minutes and Price
            Minutes = 10;
            Price = 30;
        }

        #region Properties
        private ObservableCollection<ToyCar> toyCars;
        public ObservableCollection<ToyCar> ToyCars
        {
            get { return toyCars; }
            set { toyCars = value; }
        }

        private int minutes;
        public int Minutes
        {
            get { return minutes; }
            set { minutes = value; OnPropertyChanged("Minutes"); }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged("Price"); }
        }

        private ToyCar selectedCar;
        public ToyCar SelectedCar
        {
            get { return selectedCar; }
            set { selectedCar = value; OnPropertyChanged("SelectedCar"); }
        }
        #endregion

        #region Commands
        //Command that starts all of the timers of Toy Car and writes information about that to DB
        public RelayCommands startRentCar;
        public RelayCommands StartRentCar
        {
            get 
            {
                return startRentCar ?? (startRentCar = new RelayCommands(
                    (obj) => 
                    {
                        SelectedCar.CarTimerElapsed -= new EventHandler(SelectedCar_CarTimerElapsed);
                        SelectedCar.CarTimerElapsed += new EventHandler(SelectedCar_CarTimerElapsed);
                        SelectedCar.SetTimerInterval(Minutes);
                        SelectedCar.StartCarTimer();

                        rentManager.AddInformationAboutCarRent(SelectedCar, Minutes, Price, context);
                        SelectedCar.GetTodayCash(context);
                    },
                    (obj) => { return SelectedCar != null && SelectedCar.IsFree; }
                    )); 
            }
        }

        //Actions after main timer elapsed to show user information about that
        void SelectedCar_CarTimerElapsed(object sender, EventArgs e)
        {
            Task tsk = Task.Factory.StartNew(() => 
            {
                ToyCar car = (ToyCar)sender;
                MessageBox.Show(string.Format("Время проката машинки: {0}, закончено!", car), "Время вышло!", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        //Command to open Add New Toy Car Window
        private RelayCommands addNewToyCar;
        public RelayCommands AddNewToyCar
        {
            get
            {
                return addNewToyCar ?? (addNewToyCar = new RelayCommands(
                    (obj) => 
                    {
                        NewToyCarWindow ntw = new NewToyCarWindow(ToyCars, false, context);
                        ntw.ShowDialog();
                    },
                    (obj) => { return true; }
                    ));
            }
        }

        //Command to open Change Toy Car Information Window
        private RelayCommands changeToyCarInformation;
        public RelayCommands ChangeToyCarInformation
        {
            get
            {
                return changeToyCarInformation ?? (changeToyCarInformation = new RelayCommands(
                    (obj) => 
                    {
                        NewToyCarWindow ntw = new NewToyCarWindow(ToyCars, true, context, SelectedCar);
                        ntw.ShowDialog();

                    },
                    (obj) => { return SelectedCar != null; }
                    ));
            }
        }

        //Command to remove selected Toy Car from DB
        private RelayCommands removeToyCar;
        public RelayCommands RemoveToyCar
        {
            get
            {
                return removeToyCar ?? (removeToyCar =  new RelayCommands(
                    (obj) => 
                    {
                        context.ToyCars.Remove(SelectedCar);
                        ToyCars.Remove(SelectedCar);
                        context.SaveChanges();
                    },
                    (obj) => { return (SelectedCar != null); }
                    ));
            }
            
        }

        private RelayCommands openReport;
        public RelayCommands OpenReport
        {
            get
            {
                return openReport ?? (openReport = new RelayCommands(
                    (obj) => { },
                    (obj) => { return true; }
                    ));
            }
        }


        #endregion

        #region INotifyPropertyChanged implemented interface
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
