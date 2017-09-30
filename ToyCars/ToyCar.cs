﻿using System;
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
            CarTimer = new Timer(CarTimerInterval);
            CarTimer.AutoReset = true;
            CarTimer.Elapsed += new ElapsedEventHandler(CarTimer_Elapsed);
            
            TimePassed = 0;
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

        private double timePassed;
        [NotMapped]
        public double TimePassed
        {
            get { return timePassed; }
            set { timePassed = value; OnPropertyChanged("TimePassed"); }
        }
        #endregion

        #region Timers
        //Timer for count passed time
        private Timer TimePassedTimer;
        
        void TimePassedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimePassed++;
        }

        //Main timer
        public Timer CarTimer;
        private double CarTimerInterval = 2000;

        public void SetTimerInterval(int minutes)
        {
            CarTimerInterval = (double)minutes * 60000;
            //CarTimerInterval = (double)minutes * 1000;
            
        }
        public void StartCarTimer()
        {
            //Main timer
            CarTimer.Interval = CarTimerInterval;
            CarTimer.Start();
            IsFree = false;

            //Count passed time
            TimePassed = 0;
            TimePassedTimer.Start();
        }

        void CarTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsFree = true;
            CarTimer.Stop();

            TimePassedTimer.Stop();

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