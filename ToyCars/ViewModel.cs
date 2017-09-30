using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ToyCars
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            using (ToyCarsContext context = new ToyCarsContext())
            {
                ToyCars = new ObservableCollection<ToyCar>(context.ToyCars);
            }

            Minutes = 2;
            Price = 10;

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
        public RelayCommands startLendCar;
        public RelayCommands StartLendCar
        {
            get 
            {
                return startLendCar ?? (startLendCar = new RelayCommands(
                    (obj) => 
                    {
                        SelectedCar.CarTimerElapsed -= new EventHandler(SelectedCar_CarTimerElapsed);
                        SelectedCar.CarTimerElapsed += new EventHandler(SelectedCar_CarTimerElapsed);
                        SelectedCar.SetTimerInterval(Minutes);
                        SelectedCar.StartCarTimer();
                    },
                    (obj) => { return SelectedCar != null && SelectedCar.IsFree; }
                    )); 
            }
        }

        void SelectedCar_CarTimerElapsed(object sender, EventArgs e)
        {
            Task tsk = Task.Factory.StartNew(() => 
            {
                ToyCar car = (ToyCar)sender;
                MessageBox.Show("Timer elapsed! " + car);
            });
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
