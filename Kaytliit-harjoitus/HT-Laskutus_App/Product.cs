using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HT_Laskutus_App
{
    public class Product : INotifyPropertyChanged
    {
        private int amount;
        public int Number { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
                UpdatePriceTotal();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string amount)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(amount));
        }

        public double PriceTotal { get; set; }


        private void UpdatePriceTotal()
        {
            PriceTotal = Price * amount;
            OnPropertyChanged("PriceTotal"); 
        }
    }

}
