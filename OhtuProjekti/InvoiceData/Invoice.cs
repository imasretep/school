using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using vt_systems.CustomerData;
using vt_systems.ReservationData;

namespace vt_systems.InvoiceData
{
    public class Invoice : ObservableObject
    {
        private int _id;
        private DateTime _billingdate;
        private DateTime _duedate;
        private double _reservationprice;
        private double _totalprice;
        private double _priceaddition = 0.00;
        private double _rowtotal = 0.00;
        private string _reference = string.Empty;
        private string _customerinfo = string.Empty;
        private Reservation _reservation = new Reservation();
        private Customer _customer = new Customer();

        public int ID { get { return _id; } set { _id = value; } }
        public DateTime BillingDate { get { return _billingdate; } set { _billingdate = value; OnPropertyChanged(); } }
        public DateTime DueDate { get { return _duedate; } set { _duedate = value; OnPropertyChanged(); } }
        public string ReferenceNum { get { return _reference; } set { _reference = value; OnPropertyChanged(); } }
        public string? Bank1 { get; set; } = "OP FI XX 5858 0022 6323 78";
        public string? Bank2 { get; set; } = "NORDEA FI XX 2340 0023 4511 95";
        public string? CompanyDetails { get; set; }
        public string? CustomerDetails { get { return _customerinfo; } set { _customerinfo = value; OnPropertyChanged(); } }
        public double ReservationPrice { get { return _reservationprice; } set { _reservationprice = value; OnPropertyChanged(); } }
        public double PriceAddition { get { return _priceaddition; } set { _priceaddition = value; OnPropertyChanged(); } }
        public double RowTotal { get { return _rowtotal; } set { _rowtotal = value; OnPropertyChanged(); } }
        public double TotalPrice { get { return _totalprice; } set { _totalprice = value; OnPropertyChanged(); } }
        public string AdditionalInfo { get; set; } = string.Empty;
        public int CustomerID { get; set; }
        public int ReservationID { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsPaymentLate { get; set; } // käytetään käyttöliittymän tyylien kanssa, esim. myöhässä oleva lasku on taustaltaan punaisella rivillä.
        public Reservation Reservation { get { return _reservation; } set { _reservation = value; OnPropertyChanged(); } }
        public Customer Customer { get { return _customer; } set { _customer = value; OnPropertyChanged(); } }

        public ObservableCollection<InvoiceRow> WorkspaceRows { get; set; } = new ObservableCollection<InvoiceRow>(); // laskurivi kokoelma toimipisteistä
        public ObservableCollection<InvoiceRow> DeviceRows { get; set; } = new ObservableCollection<InvoiceRow>(); // laskurivi kokoelma laitteista
        public ObservableCollection<InvoiceRow> ServiceRows { get; set; } = new ObservableCollection<InvoiceRow>(); // laskurivi kokoelma palveluista

        // Oletus konstruktori, jossa haetaa käyttöliittymä elementtiin sisältöä.
        public Invoice()
        {
            CompanyDetails = SetCompanyInfo();
        }

        // Konstruktoria käytetään uuden laskun luonnin yhteydessä.
        public Invoice(Reservation reservation)
        {
            BillingDate = DateTime.Today;
            DueDate = BillingDate.AddDays(14);
            CompanyDetails = SetCompanyInfo();
            ReservationID = reservation.ReservationId;
            CustomerID = reservation.CustomerID;
            IsPaid = false;
            GenerateReferenceNum();
        }

        /// <summary>
        /// Palauttaa merkkijonon jossa yrityksen tiedot.
        /// </summary>
        /// <returns></returns>
        private string SetCompanyInfo()
        {
            string value =
                "Jokinoisenkatu 11\n" +
                "80110 Joensuu\n" +
                "(+358)504412312\n" +
                "asiaskaspalvelu@vuokratoimistot.fi\n" +
                "\n" +
                "Saajan tilinumero:\n" +
                Bank1 + "\n" +
                Bank2;

            return value;
        }

        /// <summary>
        /// Palauttaa merkkijonon joka sisältää asiakkaan tiedot.
        /// </summary>
        /// <returns></returns>
        private string SetCustomerInfo()
        {
            string value =
                this.Customer.FirstName + " " + this.Customer.LastName + "\n" +
                this.Customer.StreetAddress + "\n" +
                this.Customer.PostalCode + " " + this.Customer.City + "\n";

            return value;
        }

        /// <summary>
        /// Metodi hakee varauksen tiedot ja datan laskulle.
        /// </summary>
        public void ImportData()
        {
            var reservationRepo = new ReservationRepo();
            var customerRepo = new CustomerRepo();

            this.Reservation.ReservationId = ReservationID;

            this.Reservation = reservationRepo.GetReservationWithData(this.Reservation);

            this.Customer.Id = CustomerID;

            this.Customer = customerRepo.GetCustomer(this.Customer);

            this.CustomerDetails = SetCustomerInfo();
        }

        /// <summary>
        /// Metodi luo viitenumeron käyttäen laskunpäiväystä pohjana ja satunnaisesti arvottuja lukuja jatkona.
        /// </summary>
        private void GenerateReferenceNum()
        {
            if (ReferenceNum == string.Empty)
            {
                string value = string.Empty;

                string basepart = this.BillingDate.ToString("yyMM");

                Random random = new Random();

                int first = random.Next(1, 1000);
                int second = random.Next(1, 900);
                int third = random.Next(1, 90);

                int randNum = first + second + third;

                int end = random.Next(1, 100) * (this.Customer.Id + this.Reservation.ReservationId);

                value = basepart + randNum.ToString() + end.ToString();

                ReferenceNum = value;
            }
        }
    }
}
