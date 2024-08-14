using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using vt_systems.CustomerData;
using vt_systems.ReservationData;

namespace vt_systems.InvoiceData
{
    public class InvoiceRepo
    {
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Päivittää laskun maksetuksi tietokantaan.
        /// </summary>
        /// <param name="invoice">Invoice luokan ilmentymä.</param>
        public void ChangePaymentStatus(Invoice invoice)
        {
            var sql = "UPDATE invoice " +
                "SET is_paid = @is_paid " +
                "WHERE invoice_id = @invoice_id";

            using (var conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@is_paid", invoice.IsPaid);
                cmd.Parameters.AddWithValue("@invoice_id", invoice.ID);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Laskun päivittäminen.
        /// </summary>
        /// <param name="invoice">Invoice luokan ilmentymä.</param>
        public void UpdateInvoice(Invoice invoice)
        {
            var sql = "UPDATE invoice " + 
                "SET additional_info = @additional_info, " +
                "customer_id = @customer_id, " +
                "due_date = @due_date, " +
                "additional_price = @additional_price, " +
                "total_price = @total_price " +
                "WHERE invoice_id = @id";

            using (var conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@additional_info", invoice.AdditionalInfo);
                cmd.Parameters.AddWithValue("@customer_id", invoice.CustomerID);
                cmd.Parameters.AddWithValue("@due_date", invoice.DueDate);
                cmd.Parameters.AddWithValue("@additional_price", invoice.PriceAddition);
                cmd.Parameters.AddWithValue("@total_price", invoice.TotalPrice);
                cmd.Parameters.AddWithValue("@id", invoice.ID);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa laskun tietokannasta.
        /// </summary>
        /// <param name="invoice">Invoice luokan ilmentymä.</param>
        public void RemoveInvoice(Invoice invoice)
        {
            var sql = "DELETE FROM invoice WHERE invoice_id = @id";

            using (var conn = new MySqlConnection(localWithDb))
            {

                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", invoice.ID);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Lisää laskun tietokantaan.
        /// </summary>
        /// <param name="invoice">Invoice luokan ilmentymä.</param>
        public void AddNewInvoice(Invoice invoice) //vanha invoiceikkuna
        {
            var sql = "INSERT INTO invoice (billing_date, due_date, reference_num, additional_info, customer_id, reservation_id, additional_price, total_price, is_paid) " +
                "VALUES (@billing_date, @due_date, @reference_num, @additional_info, @customer_id, @reservation_id, @additional_price, @total_price, @is_paid)";

            using(var conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@billing_date", invoice.BillingDate);
                cmd.Parameters.AddWithValue("@due_date", invoice.DueDate);
                cmd.Parameters.AddWithValue("@reference_num", invoice.ReferenceNum);
                cmd.Parameters.AddWithValue("@additional_info", invoice.AdditionalInfo);
                cmd.Parameters.AddWithValue("@customer_id", invoice.CustomerID);
                cmd.Parameters.AddWithValue("@reservation_id", invoice.ReservationID);
                cmd.Parameters.AddWithValue("@additional_price", invoice.PriceAddition);
                cmd.Parameters.AddWithValue("@total_price", invoice.TotalPrice);
                cmd.Parameters.AddWithValue("@is_paid", invoice.IsPaid);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee ja palauttaa kokoelman laskuista tietokannassa.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesByID(int id) //vanha invoiceikkuna
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();
            string sql = "SELECT * FROM invoice " +
                "WHERE invoice_id = @invoice_id";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@invoice_id", id);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        CustomerID = dr.GetInt32("customer_id"),
                        ReservationID = dr.GetInt32("reservation_id")
                    });
                }
            }

            return invoices;
        }

        /// <summary>
        /// Hakee ja palauttaa kokoelman laskuista tietokannassa.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesByReservation(int reservation)
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();
            string sql = "SELECT * FROM invoice " +
                "WHERE reservation_id = @reservation_id";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@reservation_id", reservation);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        CustomerID = dr.GetInt32("customer_id"),
                        ReservationID = dr.GetInt32("reservation_id"),
                        IsPaid = dr.GetBoolean("is_paid")
                    });
                }
            }

            return invoices;
        }

        /// <summary>
        /// Hakee laskut laskun päivämäärän perusteella.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesByDate(string date) // vanha invoiceikkuna
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();
            string sql = "SELECT * FROM invoice " + 
                "WHERE billing_date = @billing_date";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@billing_date", date);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        CustomerID = dr.GetInt32("customer_id"),
                        ReservationID = dr.GetInt32("reservation_id")
                    });
                }
            }

            return invoices;
        }

        /// <summary>
        /// Hakee laskut etunimen perusteella.
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesViaFirstName(string firstName) 
        { 
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            string sql = 
                "SELECT * FROM invoice WHERE customer_id = " +
                "(SELECT customer_id FROM customer WHERE cust_first_name = @firstname)";


            using(MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@firstname", firstName);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        ReservationID = dr.GetInt32("reservation_id"),
                        CustomerID = dr.GetInt32("customer_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        IsPaid = dr.GetBoolean("is_paid")
                    });
                }

            }

            return invoices;
        }


        /// <summary>
        /// Hakee laskut sukunimen perusteella.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesViaLastName(string lastName)
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            string sql =
                "SELECT * FROM invoice WHERE customer_id = " +
                "(SELECT customer_id FROM customer WHERE cust_last_name = @lastname)";


            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@lastname", lastName);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        ReservationID = dr.GetInt32("reservation_id"),
                        CustomerID = dr.GetInt32("customer_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        IsPaid = dr.GetBoolean("is_paid")
                    });
                }

            }

            return invoices;
        }

        /// <summary>
        /// Hakee laskut etu- ja sukunimen perusteella.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetInvoicesViaFullName(string firstName, string lastName)
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            string sql =
                "SELECT * FROM invoice WHERE customer_id = " +
                "(SELECT customer_id FROM customer WHERE cust_first_name = @firstname AND cust_last_name = @lastname)";


            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@firstname", firstName);
                cmd.Parameters.AddWithValue("@lastname", lastName);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        ReservationID = dr.GetInt32("reservation_id"),
                        CustomerID = dr.GetInt32("customer_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        IsPaid = dr.GetBoolean("is_paid")
                    });
                }

            }

            return invoices;
        }

        /// <summary>
        /// Hakee ja palauttaa kokoelman laskuista tietokannassa.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Invoice> GetAllInvoices()
        {
            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();
            string sql = "SELECT * FROM invoice";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var cmd = new MySqlCommand(sql, conn);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        ID = dr.GetInt32("invoice_id"),
                        BillingDate = dr.GetDateTime("billing_date"),
                        DueDate = dr.GetDateTime("due_date"),
                        ReferenceNum = dr.GetString("reference_num"),
                        PriceAddition = dr.GetDouble("additional_price"),
                        TotalPrice = dr.GetDouble("total_price"),
                        AdditionalInfo = dr.GetString("additional_info"),
                        CustomerID = dr.GetInt32("customer_id"),
                        ReservationID = dr.GetInt32("reservation_id"),
                        IsPaid = dr.GetBoolean("is_paid")
                    });
                }
            }

            return invoices;
        }

        /// <summary>
        /// Hakee asiakkaan IDt laskukokoelmaan, mistä tarkistetaan onko asiakkaalla laskuja ( asiakkaan poisto )
        /// </summary>
        /// <param name="id">Asiakkaan id</param>
        /// <returns>Asiakkaan tiedot Customer luokan ilmentymänä.</returns>
        public ObservableCollection<Invoice> GetInvoiceLines(Customer customer)
        {
            var invoiceLines = new ObservableCollection<Invoice>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT customer_id FROM invoice WHERE customer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", customer.Id);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Invoice invoice = new Invoice();
                    {
                        invoice.CustomerID = dr.GetInt32("customer_id");
                    }
                    invoiceLines.Add(invoice);
                }

            }
            return invoiceLines;
        }

        /// <summary>
        /// Päivittää laskun statuksen. Onko maksettu vai ei.
        /// </summary>
        /// <param name="invoice"></param>
        public void UpdateInvoiceStatus(Invoice invoice)
        {

            string sqlUpdateReservation = "Update invoice SET is_paid = @is_paid WHERE invoice_id = @invoiceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdateReservation, conn);
                cmdUpdate.Parameters.AddWithValue("@is_paid", invoice.IsPaid);
                cmdUpdate.Parameters.AddWithValue("@reservationID", invoice.ID);
                cmdUpdate.ExecuteNonQuery();
            }
        }
    }
}
