using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ToyCars
{
    public class RentCarInformation
    {
        public RentCarInformation()
        {

        }

        private int id;
        [Key]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int rentedToyaCarId;
        public int RenterToyCarId
        {
            get { return rentedToyaCarId; }
            set { rentedToyaCarId = value; }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private int rentTime;
        public int RentTime
        {
            get { return rentTime; }
            set { rentTime = value; }
        }

        private DateTime rentDateTime;
        public DateTime RentDateTime
        {
            get { return rentDateTime; }
            set { rentDateTime = value; }
        }

    }
}
