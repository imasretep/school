using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.DeviceData
{
    // Laiteluokka
    public class Device : INotifyPropertyChanged
    {
        private int _id;
        private string? _location;
        private int quantity;

        public int DeviceID { get { return _id; } set { _id = value; } }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double PriceByHour { get; set; }
        public double PriceByDay { get; set; }
        public double PriceByWeek { get; set; }
        public double PriceByMonth { get; set; }
        public bool IsInActive { get; set; }
        public int OfficeID { get; set; } // office_id foreign key tietokannassa
        public string OfficeCity { get; set; }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string quantity)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(quantity));
        }

        public double UnitPrice { get; set; }
        public string CurrentLocation { get { return _location; } set { _location = "Laite vuokralla tilassa jonka id = " + OfficeID.ToString(); } } // voidaan tiputtaa pois jos ei ole tarvetta


        // Oletusarvot
        public Device()
        {
            Name = string.Empty;
            Description = string.Empty;
            PriceByHour = 0;
            PriceByDay = 0;
            PriceByWeek = 0;
            PriceByMonth = 0;
            IsInActive = false;
        }
    }
}
