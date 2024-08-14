using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.ServiceData
{
    // Palvelut luokka
    public class Service : INotifyPropertyChanged
    {
        private int _id;
        private int quantity;

        public int ServiceID { get { return _id; } set { _id = value; } }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set; }
        public double? ServicePrice { get; set; }
        public double PriceByHour { get; set; }
        public double PriceByDay { get; set; }
        public double PriceByWeek { get; set; }
        public double PriceByMonth { get; set; }
        public bool IsInActive { get; set; }
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
        public int OfficeID { get; set; } // palveluun liitetty OfficeID

        // Oletusarvot
        public Service()
        {
            ServiceName = string.Empty;
            ServiceDescription = string.Empty;
            ServicePrice = 0;
            PriceByHour = 0;
            PriceByDay = 0;
            PriceByWeek = 0;
            PriceByMonth = 0;
            IsInActive = false;
        }
    }
}
