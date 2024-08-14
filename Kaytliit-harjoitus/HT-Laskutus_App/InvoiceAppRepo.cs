using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using MySqlConnector;

namespace HT_Laskutus_App
{
    public class InvoiceAppRepo
    {
        // TIETOKANTAAN YHDISTÄMISEEN TARVITTAVAT
        private const string local = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=InvoiceAppDb;";


        /// <summary>
        /// Luodaan Tietokanta.
        /// </summary>
        public void CreateInvoiceAppDb()
        {
            using (MySqlConnection conn = new MySqlConnection(local))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS InvoiceAppDb", conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("CREATE DATABASE InvoiceAppDb", conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Luodaan tuotteille taulu.
        /// </summary>
        public void CreateProductsTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Products (productID INT NOT NULL AUTO_INCREMENT PRIMARY KEY, name VARCHAR(50), price DECIMAL(10,3) NOT NULL, unit VARCHAR(50), amount INT NOT NULL)";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Luodaan asiakkaille taulu.
        /// </summary>
        public void CreateCustomersTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Customers (customerID INT NOT NULL AUTO_INCREMENT PRIMARY KEY, firstname VARCHAR(50), lastname VARCHAR(50), phone VARCHAR(50), address VARCHAR(50), postal VARCHAR(50), city VARCHAR(50))";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Luodaan laskuille taulu.
        /// </summary>
        public void CreateInvoicesTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Invoices(InvoiceID INT NOT NULL AUTO_INCREMENT, customerID INT NOT NULL, InvoiceDate DATE NOT NULL, DueDate DATE NOT NULL, Notes VARCHAR(255), ReferenceNumber VARCHAR(50), PRIMARY KEY(InvoiceID), FOREIGN KEY(customerID) REFERENCES Customers(customerID) ON DELETE CASCADE);";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Luodaan laskuriveille taulu.
        /// </summary>
        public void CreateInvoiceRowsTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE InvoiceRows (InvoiceRowID INT NOT NULL AUTO_INCREMENT, InvoiceID INT NOT NULL, productID INT NOT NULL, Quantity INT NOT NULL, PRIMARY KEY (InvoiceRowID), FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID) ON DELETE CASCADE, FOREIGN KEY (productID) REFERENCES Products(productID) ON DELETE CASCADE);";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Lisätään tuotteet tietokantaan.
        /// </summary>
        public void AddDefaultProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string product1 = "INSERT INTO Products(name, price, unit, amount) VALUES('Tiiviste', 1.5, 'Kpl', 999)";
                string product2 = "INSERT INTO Products(name, price, unit, amount) VALUES('Teippi', 2.5, 'Kpl', 999)";
                string product3 = "INSERT INTO Products(name, price, unit, amount) VALUES('Urakkapalkka', 22.5, 'h', 999)";

                MySqlCommand cmd = new MySqlCommand(product1, conn);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(product2, conn);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(product3, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lisätään kaksi Henkilöä tietokantaan.
        /// </summary>
        public void AddDefaultCustomers()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string customer1 = "INSERT INTO Customers(firstname, lastname, phone, address, postal, city) VALUES('Aku', 'Ankka', 123456, 'Paratiisitie 13', '000012', 'Ankkalinna')";
                string customer2 = "INSERT INTO Customers(firstname, lastname, phone, address, postal, city) VALUES('Roope', 'Ankka', 654321,'Ankkalinna', '000012', 'Ankkalinna')";

                MySqlCommand cmd = new MySqlCommand(customer1, conn);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(customer2, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lisätään kaksi laskua tietokantaan.
        /// </summary>
        public void AddDefaultInvoices()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Akulle lasku
                string invoice1 = "INSERT INTO invoices (customerID, InvoiceDate, DueDate, Notes, ReferenceNumber) VALUES (1, '2023-03-14', '2023-03-31', 'Remonttiin tarvikkeita.', '12344');"
                                + "INSERT INTO invoicerows (InvoiceID, productID, Quantity) VALUES (1, 2, 3);"
                                + "INSERT INTO invoicerows (InvoiceID, productID, Quantity) VALUES (1, 1, 5);";

                // Roopelle lasku
                string invoice2 = "INSERT INTO invoices (customerID, InvoiceDate, DueDate, Notes, ReferenceNumber) VALUES (2, '2023-03-14', '2023-04-05', 'Kassakaapin korjaamiseen.', '12357');"
                                + "INSERT INTO invoicerows (InvoiceID, productID, Quantity) VALUES (2, 2, 10);"
                                + "INSERT INTO invoicerows (InvoiceID, productID, Quantity) VALUES (2, 1, 10);";

                MySqlCommand cmd = new MySqlCommand(invoice1, conn);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand(invoice2, conn);
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Metodi joka hakee kaikki tuotteet tietokannasta.
        /// </summary>
        /// <returns>Tuotteet OBSERVABLECOLLECTION<></returns>
        public ObservableCollection<Product> GetProducts()
        {
            var products = new ObservableCollection<Product>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Products", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        Number = dr.GetInt32("productID"),
                        Name = dr.GetString("name"),
                        Price = dr.GetDouble("price"),
                        Unit = dr.GetString("unit"),
                        Amount = dr.GetInt32("amount")
                    });
                }
            }
            return products;
        }


        /// <summary>
        /// Metodi joka hakee halutun tuotteen tietokannasta.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Tuotteen</returns>
        public Product GetProduct(Product product)
        {
            int productNumber = product.Number;
            string sql = "SELECT COUNT(*) FROM Products WHERE productID = @productNumber";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@productNumber", productNumber);

                    // Jos count > 0 tarkoittaa, että haettu tuote on tietokannassa
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Tuote löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT name, price, unit, amount FROM products WHERE productID=@productNumber", conn);
                        cmdGetInfo.Parameters.AddWithValue("@productNumber", productNumber);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                product.Name = reader.GetString("name");
                                product.Price = reader.GetDouble("price");
                                product.Unit = reader.GetString("unit");
                                product.Amount = reader.GetInt32("amount");

                            }
                        }
                    }
                    else
                    {
                        // Tuote ei löytynyt tietokannasta
                        string errorMessage = "Tuotetta ei löytynyt tietokannasta";
                        Error errorWindow = new Error(errorMessage);
                        errorWindow.ShowDialog();
                        return null;
                    }
                }
            }
            return product;
        }

        /// <summary>
        /// Haetaan kaikki asiakkaat tietokannasta.
        /// </summary>
        /// <returns>Asiakkaat OBSERVABLECOLLECTION<></returns>
        public ObservableCollection<Customer> GetCustomers()
        {
            var customers = new ObservableCollection<Customer>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Customers", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    customers.Add(new Customer
                    {
                        CustomerId = dr.GetInt32("customerID"),
                        FirstName = dr.GetString("firstname"),
                        LastName = dr.GetString("lastname"),
                        Phone = dr.GetString("phone"),
                        Address = dr.GetString("address"),
                        Postal = dr.GetString("postal"),
                        City = dr.GetString("city"),
                    });
                }
            }
            return customers;
        }

        /// <summary>
        /// Haetaan haluttu asiakas tietokannasta.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Asiakas</returns>
        public Customer GetCustomer(Customer customer)
        {
            int customerID = customer.CustomerId;
            string sql = "SELECT COUNT(*) FROM Customer WHERE customer_id = @customerID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@customer_id", customerID);

                    // Jos count > 0 tarkoittaa, että haettu asiakas on tietokannassa
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Asiakas löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT firstname, lastname, phone, address, postal, city FROM customers WHERE customerID=@customerID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@customerID", customerID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                customer.FirstName = reader.GetString("firstname");
                                customer.LastName = reader.GetString("lastname");
                                customer.Phone = reader.GetString("phone");
                                customer.Address = reader.GetString("address");
                                customer.Postal = reader.GetString("postal");
                                customer.City = reader.GetString("city");
                            }
                        }
                    }
                    else
                    {
                        // Asiakas ei löytynyt tietokannasta
                        string errorMessage = "Asiakasnumerolla " + customerID + " ei löytynyt yhtään henkilöä";
                        Error errorWindow = new Error(errorMessage);
                        errorWindow.ShowDialog();
                        return null;
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Luodaan lasku jo olemassa olevalle asiakkaalle.
        /// </summary>
        /// <param name="invoice"></param>
        public void CreateInvoiceExistingCustomer(Invoice invoice)
        {
            string sqlInsertInvoice = "INSERT INTO invoices (customerID, InvoiceDate, DueDate, Notes, ReferenceNumber)"
                                    + "VALUES(@customerID, @InvoiceDate, @DueDate, @Notes, @ReferenceNumber)";
            string sqlInsertInvoiceRow = "INSERT INTO invoicerows (invoiceID, productID, Quantity) VALUES(@invoiceID, @productID, @quantity)";

            UpdateCustomer(invoice.Customer);

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Lisätään lasku
                MySqlCommand cmdInsert = new MySqlCommand(sqlInsertInvoice, conn);

                cmdInsert.Parameters.AddWithValue("@customerID", invoice.Customer.CustomerId);
                cmdInsert.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                cmdInsert.Parameters.AddWithValue("@DueDate", invoice.DueDate);
                cmdInsert.Parameters.AddWithValue("@Notes", invoice.Notes);
                cmdInsert.Parameters.AddWithValue("@ReferenceNumber", invoice.ReferenceNumber);
                cmdInsert.ExecuteNonQuery();

                // Haetaan laskulle numero
                var invoiceNumber = GetInvoiceQuantity();

                // Lisätään laskurivi
                foreach (var item in invoice.InvoiceRow)
                {
                    cmdInsert = new MySqlCommand(sqlInsertInvoiceRow, conn);
                    cmdInsert.Parameters.AddWithValue("@invoiceID", invoiceNumber);
                    cmdInsert.Parameters.AddWithValue("@productID", item.Number);
                    cmdInsert.Parameters.AddWithValue("@quantity", item.Amount);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Päivitetään olemassa oleva lasku.
        /// </summary>
        /// <param name="invoice"></param>
        public void UpdateInvoice(Invoice invoice)
        {

            string sqlUpdateInvoiceRow = "Update invoicerows SET quantity = @quantity WHERE InvoiceID = @invoiceID AND productID = @productID;";
            string sqlInsertInvoiceRow = "INSERT INTO invoicerows (InvoiceID, ProductID, Quantity) VALUES (@invoiceID, @productID, @quantity);";


            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Päivitetään laskurivi
                foreach (var item in invoice.InvoiceRow)
                {
                    MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdateInvoiceRow, conn);
                    cmdUpdate.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                    cmdUpdate.Parameters.AddWithValue("@productID", item.Number);
                    cmdUpdate.Parameters.AddWithValue("@quantity", item.Amount);
                    int rowsUpdated = cmdUpdate.ExecuteNonQuery();

                    // Jos riviä ei päivitetty -> tuotetta ei ollut laskulla -> lisätään se sinne.
                    if (rowsUpdated == 0)
                    {
                        MySqlCommand cmdInsert = new MySqlCommand(sqlInsertInvoiceRow, conn);
                        cmdInsert.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                        cmdInsert.Parameters.AddWithValue("@productID", item.Number);
                        cmdInsert.Parameters.AddWithValue("@quantity", item.Amount);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }


        /// <summary>
        /// Haetaan uuden asiakkaan ID tietokannasta.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>integer numero</returns>
        public int GetNewCustomerID(Customer customer)
        {
            int customersID = 0;
            string sql = "SELECT customerID FROM Customers WHERE FirstName = @FirstName AND LastName = @LastName AND Phone = @Phone AND address = @Address;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Address", customer.Address);

                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        customersID = dr.GetInt32(0);
                    }
                }
            }

            return customersID;
        }

        /// <summary>
        /// Luodaan lasku uudelle asiakkaalle.
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="customer"></param>
        public void CreateInvoiceNewCustomer(Invoice invoice, Customer customer)
        {
            AddNewCustomer(customer);
            int customersId = GetNewCustomerID(customer);

            string sqlInsertInvoice = "INSERT INTO invoices (customerID, InvoiceDate, DueDate, Notes, ReferenceNumber)"
                                    + "VALUES(@customerID, @InvoiceDate, @DueDate, @Notes, @ReferenceNumber)";
            string sqlInsertInvoiceRow = "INSERT INTO invoicerows (invoiceID, productID, Quantity) VALUES(@invoiceID, @productID, @quantity)";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Lisätään lasku
                MySqlCommand cmdInsert = new MySqlCommand(sqlInsertInvoice, conn);
                cmdInsert.Parameters.AddWithValue("@customerID", customersId);
                cmdInsert.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                cmdInsert.Parameters.AddWithValue("@DueDate", invoice.DueDate);
                cmdInsert.Parameters.AddWithValue("@Notes", invoice.Notes);
                cmdInsert.Parameters.AddWithValue("@ReferenceNumber", invoice.ReferenceNumber);
                cmdInsert.ExecuteNonQuery();

                var invoiceNumber = GetInvoiceQuantity();

                // Lisätään laskurivi
                foreach (var item in invoice.InvoiceRow)
                {
                    cmdInsert = new MySqlCommand(sqlInsertInvoiceRow, conn);
                    cmdInsert.Parameters.AddWithValue("@invoiceID", invoiceNumber);
                    cmdInsert.Parameters.AddWithValue("@productID", item.Number);
                    cmdInsert.Parameters.AddWithValue("@quantity", item.Amount);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Haetaan laskun tiedot tietokannasta.
        /// </summary>
        /// <param name="newInvoice"></param>
        /// <returns>Lasku</returns>
        public Invoice GetInvoiceData(Invoice newInvoice)
        {
            var invoice = new Invoice();
            invoice.InvoiceRow = new ObservableCollection<Product>();
            var totalPrice = 0.0;
            var invoiceID = newInvoice.InvoiceID;

            var sql = "SELECT i.InvoiceDate, i.DueDate, i.Notes, c.customerID, c.FirstName, c.LastName, c.phone, c.address, p.name, r.Quantity, p.Price, p.unit, r.Quantity * p.Price as InvoiceRowTotal, i.ReferenceNumber, p.ProductID, c.postal, c.city " +
                      "FROM invoices i " +
                      "JOIN customers c ON i.CustomerID = c.CustomerID " +
                      "LEFT JOIN invoicerows r ON i.InvoiceID = r.InvoiceID " +
                      "LEFT JOIN products p ON r.ProductID = p.ProductID " +
                      "WHERE i.invoiceID = @invoiceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@invoiceID", invoiceID);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    invoice.InvoiceDate = dr.GetDateTime(0);
                    invoice.DueDate = dr.GetDateTime(1);
                    invoice.Notes = dr.GetString(2);
                    invoice.CustomerID = dr.GetInt32(3);
                    invoice.ReferenceNumber = dr.GetString(13);

                    // Luo uusi asiakas
                    Customer customer = new Customer
                    {
                        CustomerId = dr.GetInt32(3),
                        FirstName = dr.GetString(4),
                        LastName = dr.GetString(5),
                        Phone = dr.GetString(6),
                        Address = dr.GetString(7),
                        Postal = dr.GetString(15),
                        City = dr.GetString(16),

                    };

                    // Lisää asiakas laskulle
                    invoice.Customer = customer;

                    // Lisää tuote laskulle vain, jos se löytyy
                    if (!dr.IsDBNull(8))
                    {
                        Product product = new Product
                        {
                            Name = dr.GetString(8),
                            Amount = dr.GetInt32(9),
                            Price = dr.GetDouble(10),
                            Unit = dr.GetString(11),
                            PriceTotal = dr.GetDouble(12),
                            Number = dr.GetInt32(14)
                        };

                        invoice.InvoiceRow.Add(product);

                        totalPrice += product.PriceTotal;
                        invoice.InvoiceTotal = totalPrice;
                    }
                    else
                    {
                        invoice.InvoiceTotal = 0;
                    }

                }

            }

            return invoice;
        }



        /// <summary>
        /// Haetaan kaikkien laskujen ID:t tietokannasta.
        /// </summary>
        /// <returns>Lasku OBSERVABLECOLLECTION<></returns>
        public ObservableCollection<Invoice> GetAllInvoiceID()
        {
            var invoices = new ObservableCollection<Invoice>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT i.InvoiceID, c.FirstName, c.LastName, c.Phone, c.Address, i.Notes FROM invoices i JOIN customers c ON i.customerID = c.customerID;", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    // Luo uusi lasku
                    Invoice invoice = new Invoice
                    {
                        InvoiceID = dr.GetInt32(0),
                        Notes = dr.GetString(5),

                    };

                    // Luo uusi asiakas
                    Customer customer = new Customer
                    {
                        FirstName = dr.GetString(1),
                        LastName = dr.GetString(2),
                        Phone = dr.GetString(3),
                        Address = dr.GetString(4)

                    };

                    invoice.Customer = customer;
                    invoices.Add(invoice);
                }


            }
            return invoices;
        }

        /// <summary>
        /// Lisätään uusi tuote tietokantaan.
        /// </summary>
        /// <param name="product"></param>
        public void AddNewProduct(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Products(productID, name, price, unit, amount) VALUES(@productID, @name, @price, @unit, @amount)", conn);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@amount", product.Amount);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Lisätään uusi asiakas tietokantaan.
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Customers(CustomerId, FirstName, LastName, Phone, Address, postal, City) VALUES(@customerID, @firstname, @lastname, @phone, @address, @postal, @city)", conn);
                cmd.Parameters.AddWithValue("@customerID", customer.CustomerId);
                cmd.Parameters.AddWithValue("@firstname", customer.FirstName);
                cmd.Parameters.AddWithValue("@lastname", customer.LastName);
                cmd.Parameters.AddWithValue("@phone", customer.Phone);
                cmd.Parameters.AddWithValue("@address", customer.Address);
                cmd.Parameters.AddWithValue("@postal", customer.Postal);
                cmd.Parameters.AddWithValue("@city", customer.City);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Päivitetään jo olemassa olevaa tuotetta (Esim. määrä varastossa(amount) ja hinta).
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            int productNumber = product.Number;
            string sql = "UPDATE Products SET amount=@amount, price=@price WHERE productID=@productNumber";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@productNumber", product.Number);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@amount", product.Amount);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Päivitetään asiakkaan tietoja.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            int customerID = customer.CustomerId;
            string sql = "UPDATE Customers SET firstname=@firstname, lastname=@lastname, phone=@phone, address=@address, postal=@postal, city=@city WHERE customerID=@customerID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@firstname", customer.FirstName);
                cmd.Parameters.AddWithValue("@lastname", customer.LastName);
                cmd.Parameters.AddWithValue("@phone", customer.Phone);
                cmd.Parameters.AddWithValue("@address", customer.Address);
                cmd.Parameters.AddWithValue("@postal", customer.Postal);
                cmd.Parameters.AddWithValue("@city", customer.City);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Poistetaan tuote tietokannasta.
        /// </summary>
        /// <param name="product"></param>
        public void DeleteProduct(Product product)
        {

            int number = product.Number;
            string sql = "SELECT COUNT(*) FROM Products WHERE productID = @productID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@productID", number);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Tuote löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Products WHERE productID=@productID", conn);
                        cmdDelete.Parameters.AddWithValue("@productID", product.Number);
                        cmdDelete.ExecuteNonQuery();
                        string successMessage = "Tuotteen poistaminen onnistui";
                        Done successWindow = new Done(successMessage);
                        successWindow.ShowDialog();
                    }
                    else
                    {
                        // Tuotetta ei löytynyt tietokannasta
                        string errorMessage = "Tuotenumerolla: " + number + " ei löytynyt yhtään tuotetta";
                        Error errorWindow = new Error(errorMessage);
                        errorWindow.ShowDialog();
                    }
                }
            }
        }


        /// <summary>
        /// Poistetaan asiakas tietokannasta.
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteCustomer(Customer customer)
        {

            int customerID = customer.CustomerId;
            string sql = "SELECT COUNT(*) FROM Customers WHERE customerID = @CustomerID";

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
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Customers WHERE customerid=@customerID", conn);
                        cmdDelete.Parameters.AddWithValue("@customerID", customer.CustomerId);
                        cmdDelete.ExecuteNonQuery();

                        string successMessage = "Asiakastietojen poistaminen onnistui";
                        Done successWindow = new Done(successMessage);
                        successWindow.ShowDialog();
                    }
                    else
                    {
                        // Asiakas ei löytynyt tietokannasta
                        string errorMessage = "Asiakasnumerolla: " + customerID + " ei löytynyt yhtään henkilöä";
                        Error errorWindow = new Error(errorMessage);
                        errorWindow.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Poistetaan lasku tietokannasta.
        /// </summary>
        /// <param name="invoice"></param>
        public bool DeleteInvoice(Invoice invoice)
        {
            var invoiceID = invoice.InvoiceID;
            int isDeleted = 0;
            string sqlDeleteRows = "DELETE FROM invoicerows WHERE InvoiceID = @invoiceID;";
            string sqlDeleteInvoice = "DELETE FROM invoices WHERE InvoiceID = @invoiceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sqlDeleteRows, conn))
                {
                    cmd.Parameters.AddWithValue("@invoiceID", invoiceID);
                    cmd.ExecuteNonQuery();
                }

                using (MySqlCommand cmd = new MySqlCommand(sqlDeleteInvoice, conn))
                {
                    cmd.Parameters.AddWithValue("@invoiceID", invoiceID);
                   isDeleted = cmd.ExecuteNonQuery();
                }

                if (isDeleted == 0)
                {
                    return false;
                }
                return true;
            }

        }

        /// <summary>
        /// Haetaan laskulle numero hakemalla laskujen määrä tietokannasta SELECT MAX(InvoiceID).
        /// </summary>
        /// <returns></returns>
        public int GetInvoiceQuantity()
        {

            string sql = "SELECT MAX(InvoiceID) FROM invoices;";
            int newInvoiceNumber = 0;
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    newInvoiceNumber = Convert.ToInt32(result);
                }
                else
                {
                    newInvoiceNumber = 1;
                }
            }
            return newInvoiceNumber;
        }

        /// <summary>
        /// Luodaan viitenumero laskulle.
        /// </summary>
        /// <param name="basePart"></param>
        /// <returns>Viitenumero</returns>
        public string RefNumberGenerator(int basePart)
        {
            int[] factor = new int[3];
            factor[0] = 7;
            factor[1] = 3;
            factor[2] = 1;

            string basePartString = basePart.ToString();

            char[] temp = basePartString.ToCharArray();
            int[] refBody = new int[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                refBody[i] = Convert.ToInt32(temp[i].ToString());
            }

            int sum = 0;
            int counter = 0;
            int j = refBody.Length;
            while (j > 0)
            {
                sum += refBody[j - 1] * factor[counter++ % 3];
                j--;
            }

            int refEnd = (10 - (sum % 10)) % 10;

            string refNumber = basePart.ToString() + refEnd.ToString();

            return refNumber;

        }

        /// <summary>
        /// Luodaan satunnainen viitteen perusosa.
        /// </summary>
        /// <returns></returns>
        public int RefBasePartGenerator()
        {
            Random random = new Random();
            int basePart = random.Next(1000, 9999);
            return basePart;
        }

        /// <summary>
        /// Poistetaan tuote laskulta.
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="product"></param>
        public void RemoveProduct(Invoice invoice, Product product)
        {
            string sqlRemoveRow = "DELETE FROM invoicerows WHERE invoiceID = @invoiceID AND productID = @productID;";

            // Poistetaan laskulta laskurivi
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Päivitetään laskurivi

                MySqlCommand cmdRemove = new MySqlCommand(sqlRemoveRow, conn);
                cmdRemove.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                cmdRemove.Parameters.AddWithValue("@productID", product.Number);
                cmdRemove.Parameters.AddWithValue("@quantity", product.Amount);
                cmdRemove.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Lisätään tuote laskulle(laskuriville).
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="product"></param>
        public void InsertProduct(Invoice invoice, Product product)
        {
            //RemoveFromWarehouse(product);
            string sqlInsertInvoiceRow = "INSERT INTO invoicerows (invoiceID, productID, Quantity) VALUES(@invoiceID, @productID, @quantity)";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                // Lisätään laskurivi
                MySqlCommand cmdInsert = new MySqlCommand(sqlInsertInvoiceRow, conn);
                cmdInsert.Parameters.AddWithValue("@invoiceID", invoice.InvoiceID);
                cmdInsert.Parameters.AddWithValue("@productID", product.Number);
                cmdInsert.Parameters.AddWithValue("@quantity", product.Amount);
                cmdInsert.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Lisätään tuote takaisin varastoon.
        /// </summary>
        /// <param name="product"></param>
        public void AddToWarehouse(Product product)
        {
            string sqlUpdate = "UPDATE products SET amount = amount + @Quantity WHERE productID = @productID;";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlUpdate, conn);
                cmd.Parameters.AddWithValue("@Quantity", product.Amount);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Palautetaan varasto tiettyyn pisteeseen.
        /// Käytetään Peruuta ja sulje napin kanssa.
        /// </summary>
        /// <param name="product"></param>
        public void SavePoint(Product product)
        {
            string sqlUpdate = "UPDATE products SET amount = @Quantity WHERE productID = @productID;";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlUpdate, conn);
                cmd.Parameters.AddWithValue("@Quantity", product.Amount);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                cmd.ExecuteNonQuery();

            }

        }

        /// <summary>
        /// Vähennetään tuoteen määrää varastosta.
        /// </summary>
        /// <param name="product"></param>
        public void RemoveFromWarehouse(Product product)
        {
            string sqlUpdate = "UPDATE products SET amount = amount - @Quantity WHERE productID = @productID;";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlUpdate, conn);
                cmd.Parameters.AddWithValue("@Quantity", product.Amount);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Haetaan asiakkaan laskujen määrä.
        /// Tällä tarkistetaan voiko asiakasta poistaa, jos asiakkaalla on laskuja, ei asiakasta voida poistaa.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool GetCustomerInvoice(Customer customer)
        {
            string sql = "SELECT COUNT(*) FROM invoices WHERE customerID = @customerID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@customerID", customer.CustomerId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Haetaan monessako laskurivissä tuote esiintyy
        /// Tällä tarkistetaan voidaanko tuotetta poistaa, jos tuote esiintyy laskuriveillä, sitä ei voida poistaa.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool GetProductsInvoice(Product product)
        {
            string sql = "SELECT COUNT(*) FROM invoicerows WHERE productID = @productID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// Haetaan kuinka paljon varastossa on tiettyä tuotetta
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool GetWarehouseQuantity(Product product)
        {
            string sql = "SELECT amount FROM products WHERE productID = @productID;";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productID", product.Number);
                int quantity = Convert.ToInt32(cmd.ExecuteScalar());

                if (product.Amount > quantity)
                {
                    return false;

                }

            }
            return true;
        }
    }
}
