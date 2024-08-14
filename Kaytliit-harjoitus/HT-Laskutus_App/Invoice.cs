using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HT_Laskutus_App
{
    public class Invoice : INotifyPropertyChanged
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(28);
        public string Notes { get; set; }
        public string ReferenceNumber { get; set; }
        public ObservableCollection<Product> InvoiceRow { get; set; } = new ObservableCollection<Product>();
        public Customer Customer { get; set; }
        public Company Company { get; set; } = new Company();


        private double invoiceTotal;

        public double InvoiceTotal
        {
            get => invoiceTotal;
            set
            {
                if (invoiceTotal != value)
                {
                    invoiceTotal = value;
                    OnPropertyChanged(nameof(InvoiceTotal));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
