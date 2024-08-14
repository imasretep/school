using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace vt_systems.Database
{
    internal class DatabaseScripts
    {
        // Default database connection strings
        private const string local = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";


        public bool CheckIfDBExists()
        {
            bool isFound = true;
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {

                try
                {


                    conn.Open();

                    string checkForDb = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'vtreservationsdb'";
                    MySqlCommand cmd = new MySqlCommand(checkForDb, conn);

                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }
            return isFound;
        }

        // Database creation script for vtreservationsdb
        private void CreateDatabase()
        {
            using (MySqlConnection conn = new MySqlConnection(local))
            {
                conn.Open();


                //MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS vtreservationsdb", conn);
                //cmd.ExecuteNonQuery();

                MySqlCommand cmd = new MySqlCommand("CREATE DATABASE vtreservationsdb", conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Alustaa tietokannan
        /// </summary>
        private void FormatDatabase()
        {
            using (MySqlConnection conn = new MySqlConnection(local))
            {
                conn.Open();


                MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS vtreservationsdb", conn);
                cmd.ExecuteNonQuery();

                cmd = new MySqlCommand("CREATE DATABASE vtreservationsdb", conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Customer table create script
        private void CreateCustomerTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE customer(customer_id INT NOT NULL AUTO_INCREMENT, " +
                                        "cust_first_name VARCHAR(25) NOT NULL," +
                                        "cust_last_name VARCHAR(25) NOT NULL, " +
                                        "cust_company_name VARCHAR(100) NULL," +
                                        "cust_street VARCHAR(75) NOT NULL," +
                                        "cust_postal VARCHAR(15) NOT NULL, " +
                                        "cust_city VARCHAR(50) NOT NULL," +
                                        "cust_phone VARCHAR(15) NOT NULL, " +
                                        "cust_email VARCHAR(75) NOT NULL," +
                                        "cust_is_inactive TINYINT(1) NOT NULL," +
                                        "PRIMARY KEY(customer_id))";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Office table create script
        private void CreateOfficeTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE office(office_id INT NOT NULL AUTO_INCREMENT," +
                                    "office_street VARCHAR(75) NOT NULL," +
                                    "office_postal VARCHAR(15) NOT NULL," +
                                    "office_city VARCHAR(50) NOT NULL," +
                                    "office_phone VARCHAR(15) NOT NULL," +
                                    "office_email VARCHAR(50) NOT NULL," +
                                    "office_is_inactive TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(office_id))";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create workspace table script
        private void CreateWorkSpaceTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE workspace(workspace_id INT NOT NULL AUTO_INCREMENT," +
                                    "ws_name VARCHAR(50) NOT NULL," +
                                    "ws_description VARCHAR(1000) NOT NULL," +
                                    "ws_price_hh DOUBLE(8,2) NULL," +
                                    "ws_price_dd DOUBLE(8,2) NULL," +
                                    "ws_price_ww DOUBLE(8,2) NULL," +
                                    "ws_price_mm DOUBLE(8,2) NULL," +
                                    "office_id INT NOT NULL," +
                                    "ws_is_inactive TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(workspace_id)," +
                                    "FOREIGN KEY(office_id) REFERENCES office(office_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create service table script
        private void CreateServiceTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE service(service_id INT NOT NULL AUTO_INCREMENT," +
                                    "service_name VARCHAR(50) NOT NULL," +
                                    "service_description VARCHAR(250) NOT NULL," +
                                    "service_price DOUBLE(8,2) NULL," +
                                    "service_price_hh DOUBLE(8,2) NULL," +
                                    "service_price_dd DOUBLE(8,2) NULL," +
                                    "service_price_ww DOUBLE(8,2) NULL," +
                                    "service_price_mm DOUBLE(8,2) NULL," +
                                    "office_id INT NOT NULL," +
                                    "service_is_inactive TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(service_id)," +
                                    "FOREIGN KEY(office_id) REFERENCES office(office_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create device table script
        private void CreateDeviceTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE device(device_id INT NOT NULL AUTO_INCREMENT," +
                                    "device_name VARCHAR(50) NOT NULL," +
                                    "device_description VARCHAR(250) NOT NULL," +
                                    "device_price_hh DOUBLE(8,2) NULL," +
                                    "device_price_dd DOUBLE(8,2) NULL," +
                                    "device_price_ww DOUBLE(8,2) NULL," +
                                    "device_price_mm DOUBLE(8,2) NULL," +
                                    "office_id INT NOT NULL," +
                                    "device_is_inactive TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(device_id)," +
                                    "FOREIGN KEY(office_id) REFERENCES office(office_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create reservation table script
        private void CreateReservationTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE reservation(reservation_id INT NOT NULL AUTO_INCREMENT," +
                                    "reservation_date DATE NOT NULL," +
                                    "release_date DATE NOT NULL," +
                                    "reservation_info VARCHAR(1000) NULL," +
                                    "customer_id INT NOT NULL," +
                                    "office_id INT NOT NULL," +
                                    "is_billed TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(reservation_id)," +
                                    "FOREIGN KEY(customer_id) REFERENCES customer(customer_id) ON DELETE CASCADE," +
                                    "FOREIGN KEY(office_id) REFERENCES office(office_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create reserved_objects table script
        private void CreateReservedObjectsTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE reserved_objects(reserve_line_id INT NOT NULL AUTO_INCREMENT," +
                                    "reservation_id INT NOT NULL," +
                                    "service_id INT NULL," +
                                    "device_id INT NULL," +
                                    "workspace_id INT NULL," +
                                    "qty INT NOT NULL," +
                                    "unit_price DOUBLE NOT NULL," +
                                    "PRIMARY KEY(reserve_line_id)," +
                                    "FOREIGN KEY(reservation_id) REFERENCES reservation(reservation_id) ON DELETE CASCADE," +
                                    "FOREIGN KEY(service_id) REFERENCES service(service_id) ON DELETE CASCADE," +
                                    "FOREIGN KEY(device_id) REFERENCES device(device_id) ON DELETE CASCADE," +
                                    "FOREIGN KEY(workspace_id) REFERENCES workspace(workspace_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Create invoice table script
        private void CreateInvoiceTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string createTable = "CREATE TABLE invoice(invoice_id INT NOT NULL AUTO_INCREMENT," +
                                    "billing_date DATE NOT NULL," +
                                    "due_date DATE NOT NULL," +
                                    "reference_num VARCHAR(25) NOT NULL," +
                                    "total_price DOUBLE(10,2) NOT NULL," +
                                    "additional_price DOUBLE(10,2) NOT NULL," +
                                    "additional_info VARCHAR(1000) NULL," +
                                    "customer_id INT NOT NULL," +
                                    "reservation_id INT NOT NULL," +
                                    "is_paid TINYINT(1) NOT NULL," +
                                    "PRIMARY KEY(invoice_id)," +
                                    "FOREIGN KEY(customer_id) REFERENCES customer(customer_id) ON DELETE CASCADE," +
                                    "FOREIGN KEY(reservation_id) REFERENCES reservation(reservation_id) ON DELETE CASCADE)";

                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // DEMODATA --------------------------------------------------------
        // Add customers to database
        private void AddDefaultCustomers()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string customersAdd = "INSERT INTO `customer` (`cust_first_name`, `cust_last_name`, `cust_company_name`, `cust_street`, `cust_postal`, `cust_city`, `cust_phone`, `cust_email`, `cust_is_inactive`) VALUES" +
                                    "('Mauno', 'Ahonen', '','Kummelikuja15', '33100', 'TAMPERE', '0xx-1231231', 'mauno.ahonen@onjossain.com', 0)," +
                                    "('Matti', 'Korhonen', '','Suvantokatu xxc6', '80110', 'JOENSUU', '0xx-1241241', 'korhosmasa@jossakin.com', 0)," +
                                    "('Tollo', 'Peloton', '','Kuuselantie xxc7', '80220', 'JOENSUU', '0xx-1465871', 'tollopeloton@jossakin.com', 0)," +
                                    "('Irmeli', 'Virveli', '','Arkadianmäen kellari', '00100', 'HELSINKI', '0xx-7983871', 'irmelivirveli@eduskunnankellari.fi', 0)," +
                                    "('Mikko', 'Mäkinen', '','Jyväkuja xx', '04130', 'JYVÄSKYLÄ', '0xx-7854262', 'mik.makineeen@jossakin.com', 0)," +
                                    "('Ville', 'Virtanen', '','Hiekkamyrsky', '00100', 'HELSINKI', '0xx-3332223', 'v.virtanen@roskaposti.fi', 0)";

                MySqlCommand cmd = new MySqlCommand(customersAdd, conn);
                cmd.ExecuteNonQuery();
            }
        }

        // Add offices to database
        private void AddDefaultProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string products = "INSERT INTO `office` (`office_street`, `office_postal`, `office_city`, `office_phone`, `office_email`, `office_is_inactive`) VALUES" +
                                "('Yliopistonkatu xx', '00100', 'Helsinki', '0xx-5155155', 'aspahelsinki@vuokratoimistot.fi', 0)," +
                                "('Pikku Kakkosen Posti pl 10', '33101', 'Tampere', '0xx-1121214', 'aspatampere@vuokratoimistot.fi', 0)," +
                                "('Länsikatu 15', '80110', 'Joensuu', '0xx-4564545', 'aspajoensuu@vuokratoimistot.fi', 0)," +
                                "('Kalakukontie 12', '70110', 'Kuopio', '0xx-1599511', 'aspakuopio@vuokratoimistot.fi', 0)," +
                                "('Toripolliisi 5', '90110', 'Oulu', '0xx-9011090', 'aspaoulu@vuokratoimistot.fi', 0)," +
                                "('Lutakko', '04100', 'Jyväskylä', '0xx-4214124', 'aspajyväskylä@vuokratoimistot.fi', 0)," +
                                "('Kauppatori xx', '57130', 'Savonlinna', '0xx-3045214', 'aspasavonlinna@vuokratoimistot.fi', 0)," +
                                "('Kisakatu 9', '53200', 'Lappeenranta', '0xx-5454555', 'aspalappeenranta@vuokratoimistot.fi', 0)";

                MySqlCommand cmd = new MySqlCommand(products, conn);
                cmd.ExecuteNonQuery();
            }
        }

        // Add services to database
        private void AddDefaultServices()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string services = "INSERT INTO `service` (`service_name`, `service_description`, `service_price`, `service_price_hh`, `service_price_dd`, `service_price_ww`, `service_price_mm`, `office_id`, `service_is_inactive`) VALUES" +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 1, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 2, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 3, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 4, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 5, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 6, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 7, 0)," +
                                "('Kahvi/tee', 'Kahvi/tee, kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 3, NULL, 6, 30, 120, 8, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 1, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 2, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 3, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 4, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 5, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 6, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 7, 0)," +
                                "('Kahvi/tee & Pulla', 'Kahvi/tee & Päivän pulla, valikoima vaihtelee. kerro tilatessa kuinka monta kuppia toimitetaan sekä haluttu toimitusaika ', 7, NULL, 14, 60, 240, 8, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 1, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 2, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 3, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 4, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 5, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 6, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 7, 0)," +
                                "('Vesikannu', 'Vesikannu päiväksi', NULL, NULL, 3, NULL, NULL, 8, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 1, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 2, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 3, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 4, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 5, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 6, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 7, 0)," +
                                "('Vesilasi', 'Vesilasi päiväksi', NULL, NULL, 1, NULL, NULL, 8, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 1, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 2, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 3, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 4, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 5, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 6, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 7, 0)," +
                                "('Siivous', 'Siivous kokouksen päätyttyä', NULL, 50, 50, 200, 500, 8, 0)," +
                                "('Lihalörtsy', 'Aito käsintehty paikallisherkku', 5, NULL, 10, 50, 210, 7, 0)," +
                                "('Kasvislörtsy', 'Aito käsintehty paikallisherkku', 5, NULL, 10, 50, 210, 7, 0)," +
                                "('Omenalörtsy', 'Aito käsintehty paikallisherkku', 5, NULL, 10, 50, 210, 7, 0)," +
                                "('Mustikkalörtsy', 'Aito käsintehty paikallisherkku', 5, NULL, 10, 50, 210, 7, 0)," +
                                "('Sauna', 'Saunan varaus 4h. Saunaan mahtuu max 12hlö. Sisältää keittiön, wc.n, oleskelutilan, pukuhuoneen, pesuhuoneen ja saunan. Hintaan sisältyy loppusiivous. Omat ruoat ja juomat sallittu', 580, NULL, 1000, NULL, NULL, 4, 0)," +
                                "('Sauna', 'Saunan varaus 4h. Saunaan mahtuu max 12hlö. Sisältää keittiön, wc.n, oleskelutilan, pukuhuoneen, pesuhuoneen ja saunan. Hintaan sisältyy loppusiivous. Omat ruoat ja juomat sallittu', 580, NULL, 1000, NULL, NULL, 3, 0)," +
                                "('Sauna', 'Saunan varaus 4h. Saunaan mahtuu max 12hlö. Sisältää keittiön, wc.n, oleskelutilan, pukuhuoneen, pesuhuoneen ja saunan. Hintaan sisältyy loppusiivous. Omat ruoat ja juomat sallittu', 580, NULL, 1000, NULL, NULL, 6, 0)," +
                                "('Sauna', 'Saunan varaus 4h. Saunaan mahtuu max 12hlö. Sisältää keittiön, wc.n, oleskelutilan, pukuhuoneen, pesuhuoneen ja saunan. Hintaan sisältyy loppusiivous. Omat ruoat ja juomat sallittu', 580, NULL, 1000, NULL, NULL, 1, 0)";

                MySqlCommand cmd = new MySqlCommand(services, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Add devices to database
        private void AddDefaultDevices()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string devices = "INSERT INTO `device` (`device_name`, `device_description`, `device_price_hh`, `device_price_dd`, `device_price_ww`, `device_price_mm`, `office_id`, `device_is_inactive`) VALUES" +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 1, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 2, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 3, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 4, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 5, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 6, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 7, 0)," +
                                "('Siirrettävä valkotaulu', 'Siirrettävä pyörillä varustettu valkotaulu ja taulutussit', NULL, 10.00, 40.00, 80.00, 8, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 1, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 2, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 3, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 4, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 5, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 6, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 7, 0)," +
                                "('65\" näyttö', '65\" 4K näyttö, siirrettävä', NULL, 45.00, 150.00, 300.00, 8, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 1, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 2, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 3, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 4, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 5, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 6, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 7, 0)," +
                                "('Konferenssilaitteet', 'Konferenssikamera, mikrofonit ja kaiuttimet', NULL, 60.00, 200.00, 400.00, 8, 0)";

                MySqlCommand cmd = new MySqlCommand(devices, conn);
                cmd.ExecuteNonQuery();
            }
        }

        // Add  workspaces to database
        private void AddDefaultWorkspaces()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string workspaces = "INSERT INTO `workspace` (`ws_name`, `ws_description`, `ws_price_hh`, `ws_price_dd`, `ws_price_ww`, `ws_price_mm`, `office_id`, `ws_is_inactive`) VALUES" +
                                "('Hiki', 'Pieni kokoushuone max. 4 hlö.lle. Valkokangas löytyy seinältä', 18.00, 120, NULL, NULL, 1, 0)," +
                                "('Hautomo1', 'Kokoustila max. 8 henkilölle. Jaettu keittiö ja WC 3 muun toimitilan kanssa', 35.00, 250.00, NULL, NULL, 1, 0)," +
                                "('Hautomo2', 'Kokoustila max. 8 henkilölle. Jaettu keittiö ja WC 3 muun toimitilan kanssa', 35.00, 250.00, NULL, NULL, 1, 0)," +
                                "('Hautomo3', 'Kokoustila max. 8 henkilölle. Jaettu keittiö ja WC 3 muun toimitilan kanssa', 35.00, 250.00, NULL, NULL, 1, 0)," +
                                "('Hautomo4', 'Kokoustila max. 8 henkilölle. Jaettu keittiö ja WC 3 muun toimitilan kanssa', 35.00, 250.00, NULL, NULL, 1, 0)," +
                                "('Lokki', 'Toimistotilaa 300m2. Tilassa 4 kpl toimistohuoneita, keittiö, 2x WC. Lisäksi saatavilla runsaasti siirrettäviä sermejä, joilla tiloja voi muokata haluamaksensa', NULL, 250, NULL, 2950.00, 1, 0)," +
                                "('Pikku1', 'Neuvotteluhuone 4-6 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 15.00, 90, NULL, NULL, 2, 0)," +
                                "('Pikku2', 'Neuvotteluhuone 4-6 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 15.00, 90, NULL, NULL, 2, 0)," +
                                "('Pikku3', 'Neuvotteluhuone 4-6 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 15.00, 90, NULL, NULL, 2, 0)," +
                                "('Pikku4', 'Neuvotteluhuone 4-6 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 15.00, 90, NULL, NULL, 2, 0)," +
                                "('Keski1', 'Neuvotteluhuone 8-12 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 25.00, 150.00, 400.00, NULL, 2, 0)," +
                                "('Keski2', 'Neuvotteluhuone 8-12 hlö.lle. Jääkaappi, WC-tilat ja vesipiste yhteisessä tilassa 5 muun neukkarin kanssa.', 25.00, 150.00, 400.00, NULL, 2, 0)," +
                                "('Jätti', 'Neuvotteluhuone max 30 hengelle. Oma keittiö ja WC-tilat. Tilava eteinen, jossa riittävästi naulakkotilaa.  Esitystekniikka kuuluu tilan vuokraan.', NULL, 200, 800.00, 2200.00, 2, 0)," +
                                "('Koski1', 'Pieni (4-6 hlö) neuvotteluhuone Tiedepuiston 3. kerroksessa. Yhteiset sosiaalitilat muiden vastaavien tilojen kanssa. Valkokangas ja WiFi-yhteys kuuluu hintaan', 18.00, 115.00, 375.00, NULL, 3, 0)," +
                                "('Koski2', 'Pieni (4-6 hlö) neuvotteluhuone Tiedepuiston 3. kerroksessa. Yhteiset sosiaalitilat muiden vastaavien tilojen kanssa. Valkokangas ja WiFi-yhteys kuuluu hintaan', 18.00, 115.00, 375.00, NULL, 3, 0)," +
                                "('Koski3', 'Pieni (4-6 hlö) neuvotteluhuone Tiedepuiston 3. kerroksessa. Yhteiset sosiaalitilat muiden vastaavien tilojen kanssa. Valkokangas ja WiFi-yhteys kuuluu hintaan', 18.00, 115.00, 375.00, NULL, 3, 0)," +
                                "('Linnunlahti', 'Iso neuvotteluhuone (max. 20 hlö) Tiedepuiston 3. kerroksesta. Oma oleskelutila, keittiö, WCtilat sekä tilava aula. WiFi ja valkokangas kuuluu vuokraan', 45.00, 250.00, 650.00, NULL, 3, 0)," +
                                "('TiPu', '350m2 toimistotila omalla sisäänkäynnillä katutasossa. Tilavat sosiaalitilat sekä keittiö. Tilassa 4 kiinteää toimsitoa ja iso jaettava tila. Tiedepuiston kuntosalin vapaa käyttö kuuluu hintaan.', NULL, 300, 1200.00, 3000.00, 3, 0)," +
                                "('Savo', 'Suurjkokoinen toimistotila missä sosiaaljtilat ja pien keittiö.', NULL, 150, 600.00, 1600.00, 4, 0)," +
                                "('Torin Helemi', 'Suurj 600m2 toemistotila torin kuppeessa, keskellä mualiman napoo. Tästä o unelmat tehtynä. On sosiaalitilloo ja vaekka sun mitä. huonneet voep jakkoo, jos mielj tekköö. Kalustetta o jos mimmottista ja lissee tulloo jos myö nii sovitaan.', NULL, 375, NULL, 4500.00, 4, 0)";

                MySqlCommand cmd = new MySqlCommand(workspaces, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Add reservations to database
        private void AddDefaultReservations()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string reservations = "INSERT INTO `reservation` (`reservation_date`, `release_date`, `reservation_info`, `customer_id`, `office_id` , `is_billed`) VALUES" +
                                 "('2023-04-19', '2023-04-22', 'Pieni neukkari 3 vrk ja päiväkahvit joka päivälle 4hlö.lle', 1, 1, 1)," +
                                 "('2023-04-19', '2023-05-19', 'Toimistotila kuukaudeksi', 2, 3, 1)," +
                                 "('2023-04-19', '2023-05-03', 'Savo-toimistotila 2 viikoksi + näyttö ja konferenssilaitteet koko ajalle', 3, 4, 1)," +
                                 "('2023-04-19', '2023-04-19', 'Pikkukakkonen päiväksi + kahvit ja pullat 6.lle', 4, 2, 1)," +
                                 "('2023-04-19', '2023-04-20', 'Neukkari 2vrk + 20.4 Sauna klo 20 alkaen + siivous', 5, 3, 1)," +
                                 "('2023-04-21', '2023-04-21', 'Pikku1 varaus 21.4', 6, 2, 0)," +
                                 "('2023-04-22', '2023-04-22', 'Pikku1 varaus 22.4', 4, 2, 0)," +
                                 "('2023-04-23', '2023-04-23', 'Pikku1 varaus 23.4', 2, 2, 0)";

                MySqlCommand cmd = new MySqlCommand(reservations, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Add reserved objects to database
        private void AddDefaultReservedObjects()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string reservedObjects = "INSERT INTO `reserved_objects` (`reservation_id`, `service_id`, `device_id`, `workspace_id`, `qty`, `unit_price`) VALUES" +
                                "(1, 1, NULL, NULL, 3, 3)," +
                                "(1, NULL, NULL, 1, 3, 18)," +
                                "(2, NULL, NULL, 18, 1, 3000)," +
                                "(3, NULL, NULL, 19, 2, 600)," +
                                "(3, NULL, 12, NULL, 2, 150)," +
                                "(3, NULL, 20, NULL, 2, 200)," +
                                "(4, NULL, NULL, 8, 8, 15)," +
                                "(4, 10, NULL, NULL, 6, 7)," +
                                "(5, NULL, NULL, 17, 2, 250)," +
                                "(5, 35, NULL, NULL, 5, 50)," +
                                "(5, 46, NULL, NULL, 1, 580)," +
                                "(5, 19, NULL, NULL, 4, 3)," +
                                "(5, 27, NULL, NULL, 20, 1)," +
                                "(6, NULL, NULL, 7, 1, 90)," +
                                "(7, NULL, NULL, 7, 1, 90)," +
                                "(8, NULL, NULL, 7, 1, 90)";

                MySqlCommand cmd = new MySqlCommand(reservedObjects, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // Add invoices to database
        private void AddDefaultInvoicens()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string invoices = "INSERT INTO `invoice` (`billing_date`, `due_date`, `reference_num`, `total_price`, `additional_price`, `additional_info`, `customer_id`, `reservation_id`, `is_paid`) VALUES" +
                                "('2023-04-22', '2023-05-06', '1234 1234 1234', 63.00, 0, '', 1, 1, 1)," +
                                "('2023-05-19', '2023-06-02', '3214 32143 21', 3000.00, 0, '', 2, 2, 0)," +
                                "('2023-05-03', '2023-05-17', '12 1594 233', 1900.00, 0, '', 3, 3, 0)," +
                                "('2023-04-19', '2023-05-03', '6566 548', 162.00, 0, '', 4, 4, 1)," +
                                "('2023-04-19', '2023-05-03', '12345 12345', 1412.00, 250, 'Saunalta löytyi rikkoontuneita viinapulloja ja oksennusta. Laskuun lisätty ylimääräisiä siivouskuluja 250€', 5, 5, 0)";

                MySqlCommand cmd = new MySqlCommand(invoices, conn);
                cmd.ExecuteNonQuery();
            }
        }


        // run database creation scripts
        public void IniateDatabase()
        {
            CreateDatabase();
            CreateCustomerTable();
            CreateOfficeTable();
            CreateWorkSpaceTable();
            CreateServiceTable();
            CreateDeviceTable();
            CreateReservationTable();
            CreateReservedObjectsTable();
            CreateInvoiceTable();
            AddDefaultProducts();
            AddDefaultCustomers();
            AddDefaultServices();
            AddDefaultDevices();
            AddDefaultWorkspaces();
            AddDefaultReservations();
            AddDefaultReservedObjects();
            AddDefaultInvoicens();
        }

        public void IniateDatabaseAdmin()
        {
            FormatDatabase();
            CreateCustomerTable();
            CreateOfficeTable();
            CreateWorkSpaceTable();
            CreateServiceTable();
            CreateDeviceTable();
            CreateReservationTable();
            CreateReservedObjectsTable();
            CreateInvoiceTable();
            AddDefaultProducts();
            AddDefaultCustomers();
            AddDefaultServices();
            AddDefaultDevices();
            AddDefaultWorkspaces();
            AddDefaultReservations();
            AddDefaultReservedObjects();
            AddDefaultInvoicens();
        }
    }
}
