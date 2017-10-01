using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyCars
{
    public class RentCarInformationManager
    {
        ToyCarsContext context;

        public void AddInformationAboutCarRent(ToyCar car, int time, double price)
        {
            using (context = new ToyCarsContext())
            {
                RentCarInformation rci = new RentCarInformation();
                rci.RenterToyCarId = car.ID;
                rci.Price = price;
                rci.RentTime = time;
                rci.RentDateTime = DateTime.Now;

                context.RentCarsInformation.Add(rci);
                context.SaveChanges();
            }
        }
    }
}
