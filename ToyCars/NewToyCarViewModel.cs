using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ToyCars
{
    public class NewToyCarViewModel : INotifyPropertyChanged
    {
        //DB context
        ToyCarsContext context;

        public NewToyCarViewModel(ObservableCollection<ToyCar> toyCars, ToyCar selectedCar = null)
        {
            ToyCars = toyCars;

            if (selectedCar != null)
            {
                ToyCarToChange = selectedCar;
                Title = selectedCar.Title;
                ImageUri = selectedCar.ImageUri;
            }
        }

        #region Properties
        //Title of the Toy Car
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        //Image source for the Toy Car
        private string imageUri;
        public string ImageUri
        {
            get { return imageUri; }
            set { imageUri = value; OnPropertyChanged("ImageUri"); }
        }

        //Toy Car that have to be changed
        private ToyCar toyCarToChange;
        public ToyCar ToyCarToChange
        {
            get { return toyCarToChange; }
            set { toyCarToChange = value; }
        }

        //Collection of the Toy Cars from main ViewModel - to display changes with toy cars in collection
        ObservableCollection<ToyCar> ToyCars;
        #endregion

        #region Commands
        //Command to add new Toy Car to DB
        private RelayCommands addNewToyCar;
        public RelayCommands AddNewToyCar
        {
            get
            {
                return addNewToyCar ?? (addNewToyCar = new RelayCommands(
                    (obj) => 
                    {
                        ToyCar newCar = new ToyCar();
                        newCar.Title = Title;
                        newCar.ImageUri = ImageUri;

                        using (context = new ToyCarsContext())
                        {
                            context.ToyCars.Add(newCar);
                            context.SaveChanges();

                            ToyCars.Add(newCar);

                            OnCloseWindowRequest();
                        }
                    },
                    (obj) => { return !string.IsNullOrEmpty(Title); }
                    ));
            }
        
        }

        //Command to chose image file for Toy Car
        private RelayCommands openImageFile;
        public RelayCommands OpenImageFile
        {
            get
            {
                return openImageFile ?? (openImageFile = new RelayCommands(
                    (obj) => 
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "Image Files(*.BMP;*.JPG;*JPEG;*.GIF;*.PNG)|*.BMP;*.JPG;*JPEG;*.GIF;*.PNG";
                        if (ofd.ShowDialog() == true)
                        {
                            ImageUri = ofd.FileName;
                        }

                    },
                    (obj) => { return true; }
                    ));

            }
        }

        //Command to Change information about Toy Car
        private RelayCommands changeToyCarInfo;
        public RelayCommands ChangeToyCarInfo
        {
            get
            {
                return changeToyCarInfo ?? (changeToyCarInfo = new RelayCommands(
                    (obj) => 
                    {
                        ToyCarToChange.Title = Title;
                        ToyCarToChange.ImageUri = ImageUri;

                        using (context = new ToyCarsContext())
                        {
                            ToyCar dbCar = context.ToyCars.Single(x => x.ID == ToyCarToChange.ID);
                            dbCar.Title = Title;
                            dbCar.ImageUri = ImageUri;

                            context.SaveChanges();

                            OnCloseWindowRequest();
                        }
                    },
                    (obj) => { return !string.IsNullOrEmpty(Title); }
                    ));
            }
            
        }

        #endregion

        #region Events
        //Event to close window after manipulations with Toy Car
        public event EventHandler CloseWindowRequest;
        private void OnCloseWindowRequest()
        {
            if (CloseWindowRequest != null) CloseWindowRequest(this, EventArgs.Empty);
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
