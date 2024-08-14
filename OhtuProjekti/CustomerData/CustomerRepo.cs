using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace vt_systems.CustomerData
{
    // Sisältää kaikki Customer luokalle tarkoitetut metodit.

    internal class CustomerRepo
    {
        // Default database connection strings
        private const string local = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki asiakkaat tietokannasta.
        /// </summary>
        /// <returns>Listan(ObservableCollection) asiakkaita</returns>
        public ObservableCollection<Customer> GetCustomers()
        {
            var customers = new ObservableCollection<Customer>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Customer", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("cust_is_inactive") != true )
                    {
                        customers.Add(new Customer
                        {
                            Id = dr.GetInt32("customer_id"),
                            FirstName = dr.GetString("cust_first_name"),
                            LastName = dr.GetString("cust_last_name"),
                            CompanyName = dr.GetString("cust_company_name"),
                            StreetAddress = dr.GetString("cust_street"),
                            PostalCode = dr.GetString("cust_postal"),
                            City = dr.GetString("cust_city"),
                            Phone = dr.GetString("cust_phone"),
                            Email = dr.GetString("cust_email"),

                        });
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Hakee kaikki asiakkaat ei aktiivist tietokannasta.
        /// </summary>
        /// <returns>Listan(ObservableCollection) asiakkaita</returns>
        public ObservableCollection<Customer> GetInactiveCustomers()
        {
            var customers = new ObservableCollection<Customer>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Customer", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("cust_is_inactive") != false)
                    {
                        customers.Add(new Customer
                        {
                            Id = dr.GetInt32("customer_id"),
                            FirstName = dr.GetString("cust_first_name"),
                            LastName = dr.GetString("cust_last_name"),
                            CompanyName = dr.GetString("cust_company_name"),
                            StreetAddress = dr.GetString("cust_street"),
                            PostalCode = dr.GetString("cust_postal"),
                            City = dr.GetString("cust_city"),
                            Phone = dr.GetString("cust_phone"),
                            Email = dr.GetString("cust_email"),

                        });
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Hakee tietyn asiakkaan tiedot tietokannasta asiakkaan ID:n mukaan.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Asiakkaan</returns>
        public Customer GetCustomer(Customer customer)
        {
            int customerID = customer.Id;
            string sql = "SELECT COUNT(*) FROM Customer WHERE customer_id = @customerID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@customerID", customerID);

                    // Tarkistetaan onko asiakasta tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että asiakas löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Asiakas löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM customer WHERE customer_id=@customerID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@customerID", customerID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                customer.Id = reader.GetInt32("customer_id");
                                customer.FirstName = reader.GetString("cust_first_name");
                                customer.LastName = reader.GetString("cust_last_name");
                                customer.CompanyName = reader.GetString("cust_company_name");
                                customer.StreetAddress = reader.GetString("cust_street");
                                customer.PostalCode = reader.GetString("cust_postal");
                                customer.City = reader.GetString("cust_city");
                                customer.Phone = reader.GetString("cust_phone");
                                customer.Email = reader.GetString("cust_email");
                                customer.IsInActive = reader.GetBoolean("cust_is_inactive");

                            }
                        }
                    }
                    else
                    {
                        // Asiakas ei löytynyt tietokannasta
                        MessageBox.Show("Asiakasta ei löytynyt.", "Virhe!");

                        return null;
                    }
                }
            }
            return customer;
        }

        /// <summary>
        /// Hakee asiakkaan ID:n
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer GetCustomerID(Customer customer)
        {

            string sql = "SELECT customer.customer_id " +
                            "FROM customer " +
                            "WHERE cust_first_name = @custFirstName " +
                            "AND cust_last_name = @custLastName " +
                            "AND cust_street = @custStreet;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();


                MySqlCommand cmdGetInfo = new MySqlCommand(sql, conn);
                cmdGetInfo.Parameters.AddWithValue("@custFirstName", customer.FirstName);
                cmdGetInfo.Parameters.AddWithValue("@custLastName", customer.LastName);
                cmdGetInfo.Parameters.AddWithValue("@custStreet", customer.StreetAddress);
                using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        customer.Id = reader.GetInt32("customer_id");

                    }
                }

            }
            return customer;
        }

        /// <summary>
        /// Lisää uuden asiakkaan.
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Customer(customer_id, cust_first_name, cust_last_name, cust_company_name, cust_street, cust_postal, cust_city, cust_phone, cust_email, cust_is_inactive)" +
                                                    "VALUES(@customer_id, @cust_first_name, @cust_last_name, @cust_company_name, @cust_street, @cust_postal, @cust_city, @cust_phone, @cust_email, @cust_is_inactive)", conn);
                cmd.Parameters.AddWithValue("@customer_id", customer.Id);
                cmd.Parameters.AddWithValue("@cust_first_name", customer.FirstName);
                cmd.Parameters.AddWithValue("@cust_last_name", customer.LastName);
                cmd.Parameters.AddWithValue("@cust_company_name", customer.CompanyName);
                cmd.Parameters.AddWithValue("@cust_street", customer.StreetAddress);
                cmd.Parameters.AddWithValue("@cust_postal", customer.PostalCode);
                cmd.Parameters.AddWithValue("@cust_city", customer.City);
                cmd.Parameters.AddWithValue("@cust_phone", customer.Phone);
                cmd.Parameters.AddWithValue("@cust_email", customer.Email);
                cmd.Parameters.AddWithValue("@cust_is_inactive", customer.IsInActive);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Päivittää asiakkaan tiedot asiakas ID:n avulla.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            int customerID = customer.Id;
            string sql = "UPDATE Customer SET cust_first_name=@cust_first_name, cust_last_name=@cust_last_name, cust_company_name=@cust_company_name, cust_street=@cust_street, cust_postal=@cust_postal," +
                          "cust_city=@cust_city, cust_phone=@cust_phone, cust_email=@cust_email, cust_is_inactive = @cust_is_inactive WHERE customer_id=@customerID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@customerID", customer.Id);
                cmd.Parameters.AddWithValue("@cust_first_name", customer.FirstName);
                cmd.Parameters.AddWithValue("@cust_last_name", customer.LastName);
                cmd.Parameters.AddWithValue("@cust_company_name", customer.CompanyName);
                cmd.Parameters.AddWithValue("@cust_street", customer.StreetAddress);
                cmd.Parameters.AddWithValue("@cust_postal", customer.PostalCode);
                cmd.Parameters.AddWithValue("@cust_city", customer.City);
                cmd.Parameters.AddWithValue("@cust_phone", customer.Phone);
                cmd.Parameters.AddWithValue("@cust_email", customer.Email);
                cmd.Parameters.AddWithValue("@cust_is_inactive", customer.IsInActive);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        /// <summary>
        /// Poistaa asiakkaan tietokannasta
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteCustomer(Customer customer)
        {
            int customerID = customer.Id;
            string sql = "SELECT COUNT(*) FROM Customer WHERE customer_id = @customerID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@customerID", customerID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Asiakas löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Customer WHERE customer_id=@customerID", conn);
                        cmdDelete.Parameters.AddWithValue("@customerID", customer.Id);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Asiakastietojen poistaminen onnistui");
                    }
                    else
                    {
                        // Asiakas ei löytynyt tietokannasta
                        MessageBox.Show("Asiakasnumerolla: " + customerID + " ei löytynyt yhtään henkilöä");
                    }
                }
            }
        }
    }
}
