using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ToyCars
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        ToyCarsContext context;

        public ReportViewModel(ToyCarsContext context)
        {
            this.context = context;
            ReportInformation = new ObservableCollection<RentCarInformation>();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        #region Properties
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
        }

        private ObservableCollection<RentCarInformation> reportInformation;
        public ObservableCollection<RentCarInformation> ReportInformation
        {
            get { return reportInformation; }
            set { reportInformation = value; }
        }

        #endregion

        #region Commands
        private RelayCommands getReport;
        public RelayCommands GetReport
        {
            get
            {
                return getReport ?? (getReport = new RelayCommands(
                    (obj) => { GetReportMethod(); },
                    (obj) => { return true; }
                    ));
            }
        }
        #endregion

        #region Private methods
        private void GetReportMethod()
        {
            ReportInformation.Clear();

            var res = from r in context.RentCarsInformation
                      where r.RentDateTime.Year >= StartDate.Year && r.RentDateTime.Year <= EndDate.Year &&
                            r.RentDateTime.Month >= StartDate.Month && r.RentDateTime.Month <= EndDate.Month &&
                            r.RentDateTime.Day >= StartDate.Day && r.RentDateTime.Day <= EndDate.Day
                      group r by r.RenterToyCarId into carsGrouped
                      select carsGrouped;

            //var res = from r in context.RentCarsInformation
            //          where DateTime.Compare(StartDate, r.RentDateTime) <= 0 && DateTime.Compare(EndDate, r.RentDateTime) >=0
            //          group r by r.RenterToyCarId into carsGrouped
            //          select carsGrouped;

            foreach (IGrouping<int, RentCarInformation> c in res)
            {
                //Key is ToyCarId
                Debug.WriteLine(c.Key);
                
                foreach (RentCarInformation rci in c)
                {
                    Debug.WriteLine(string.Format("{0}-{1}-{2}", rci.RenterToyCarId, rci.RentTime, rci.Price));
                    ReportInformation.Add(rci);
                }
            }

            Debug.Write(DateTime.Compare(new DateTime(2017,10,1), new DateTime(2017,10,2)));
        }

        private bool CompareDates(DateTime dc)
        {
            if (dc.Year >= StartDate.Year && dc.Year <= EndDate.Year &&
                dc.Month >= StartDate.Month && dc.Month <= EndDate.Month &&
                dc.Day >= StartDate.Day && dc.Day <= EndDate.Day)
                return true;

            return false;
        }
        #endregion

        #region INotifyPropertyChanged implementation interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
