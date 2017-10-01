using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyCars
{
    public class ToyCar : INotifyPropertyChanged
    {
        public ToyCar()
        {
            //Initalizing main timer
            CarTimer = new Timer(CarTimerInterval);
            CarTimer.AutoReset = true;
            CarTimer.Elapsed += new ElapsedEventHandler(CarTimer_Elapsed);
            
            //Initalizing time passed timer
            TimePassedTimer = new Timer(1000);
            TimePassedTimer.Elapsed += new ElapsedEventHandler(TimePassedTimer_Elapsed);

            IsFree = true;
        }

        #region Properties
        private int id;
        [Key]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        private string imageUri;
        public string ImageUri
        {
            get { return imageUri; }
            set { imageUri = value; OnPropertyChanged("ImageUri"); }
        }

        private bool isFree;
        [NotMapped]
        public bool IsFree
        {
            get { return isFree; }
            set { isFree = value; OnPropertyChanged("IsFree"); }
        }

        private TimeSpan timePassed;
        [NotMapped]
        public TimeSpan TimePassed
        {
            get { return timePassed; }
            set { timePassed = value; OnPropertyChanged("TimePassed"); }
        }

        private double todayCash;
        [NotMapped]
        public double TodayCash
        {
            get { return todayCash; }
            set { todayCash = value; OnPropertyChanged("TodayCash"); }
        }

        #endregion

        #region Methods
        //Get Today Cash 
        public void GetTodayCash(ToyCarsContext context)
        {
            TodayCash = 0;
            DateTime today = DateTime.Today;

            var rentInfo = from d in context.RentCarsInformation
                           where d.RentDateTime.Year == today.Year && d.RentDateTime.Month == today.Month &&
                           d.RentDateTime.Day == today.Day && d.RenterToyCarId == ID
                           select d;

            foreach (var r in rentInfo)
            {
                TodayCash += r.Price;
            }
        }
        #endregion

        #region Timers
        ////////////////////////Timer for count passed time
        private Timer TimePassedTimer;
        private DateTime TimePassedTimerEndTime;

        //TimePassedTimer elapsed method wich counts time to end every second
        void TimePassedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePassed = TimeLeft();
        }

        //Take time to end
        private TimeSpan TimeLeft()
        {
            return TimePassedTimerEndTime - DateTime.Now;
        }

        ////////////////////////Main timer
        public Timer CarTimer;
        private double CarTimerInterval = 2000;

        //Setting up main timer interval
        public void SetTimerInterval(int minutes)
        {
            CarTimerInterval = (double)minutes * 60000;            
        }

        //Start all of timers when start button pressed
        public void StartCarTimer()
        {
            //Main timer
            CarTimer.Interval = CarTimerInterval;
            CarTimer.Start();
            IsFree = false;

            //Timer to show time to end
            TimePassedTimerEndTime = DateTime.Now.AddMilliseconds(CarTimerInterval);
            TimePassedTimer.Start();
        }

        //Main CarTimer elapsed method
        void CarTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsFree = true;
            CarTimer.Stop();

            TimePassedTimer.Stop();
            //Event
            OnCarTimerElapsed();
        }

        #endregion

        #region Toy Car events
        public event EventHandler CarTimerElapsed;
        private void OnCarTimerElapsed()
        {
            if (CarTimerElapsed != null) CarTimerElapsed(this, EventArgs.Empty);
        }
        #endregion

        #region INotifyPropertychanged implemented interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Overrided methods
        public override string ToString()
        {
            return Title;
        }
        #endregion
    }
}
