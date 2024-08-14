using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using vt_systems.DeviceData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.OfficeData
{
    /// <summary>
    /// Sisältää kaikki Office luokalle tarkoitetut metodit.
    /// </summary>
    class OfficeRepo
    {
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki toimipisteet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Office> GetOffices()
        {
            var offices = new ObservableCollection<Office>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM office;", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("office_is_inactive") != true)
                    {
                        offices.Add(new Office
                        {
                            OfficeID = dr.GetInt32("office_id"),
                            StreetAddress = dr.GetString("office_street"),
                            PostalCode = dr.GetString("office_postal"),
                            City = dr.GetString("office_city"),
                            Phone = dr.GetString("office_phone"),
                            Email = dr.GetString("office_email"),

                        });
                    }
                }
            }
            return offices;
        }

        /// <summary>
        /// Hakee kaikki ei aktiiviset toimipisteet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Office> GetInactiveOffices()
        {
            var offices = new ObservableCollection<Office>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM office;", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("office_is_inactive") != false)
                    {
                        offices.Add(new Office
                        {
                            OfficeID = dr.GetInt32("office_id"),
                            StreetAddress = dr.GetString("office_street"),
                            PostalCode = dr.GetString("office_postal"),
                            City = dr.GetString("office_city"),
                            Phone = dr.GetString("office_phone"),
                            Email = dr.GetString("office_email"),

                        });
                    }
                }
            }
            return offices;
        }


        /// <summary>
        /// Hakee tietokannasta toimipisteen
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public Office GetOffice(Office office)
        {
            int officeID = office.OfficeID;
            string sql = "SELECT COUNT(*) FROM Office WHERE office_id = @officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@officeID", officeID);

                    // Tarkistetaan onko toimipiste tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että toimipiste löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimipiste löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM office WHERE office_id=@officeID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@officeID", officeID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                office.OfficeID = reader.GetInt32("office_id");
                                office.StreetAddress = reader.GetString("office_street");
                                office.PostalCode = reader.GetString("office_postal");
                                office.City = reader.GetString("office_city");
                                office.Phone = reader.GetString("office_phone");
                                office.Email = reader.GetString("office_email");
                                office.IsInActive = reader.GetBoolean("office_is_inactive");

                            }
                        }
                    }
                    else
                    {
                        // Toimipiste ei löytynyt tietokannasta
                        MessageBox.Show("Toimipistettä ei löytynyt.", "Virhe!");
                        return null;
                    }
                }
            }
            return office;
        }


        /// <summary>
        /// Lisää uuden toimipisteen
        /// </summary>
        /// <param name="office"></param>
        public void AddNewOffice(Office office)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Office(office_id, office_street, office_postal, office_city, office_phone, office_email, office_is_inactive)" +
                                                    "VALUES(@office_id, @office_street, @office_postal, @office_city, @office_phone, @office_email, @office_is_inactive);", conn);
                cmd.Parameters.AddWithValue("@office_id", office.OfficeID);
                cmd.Parameters.AddWithValue("@office_street", office.StreetAddress);
                cmd.Parameters.AddWithValue("@office_postal", office.PostalCode);
                cmd.Parameters.AddWithValue("@office_city", office.City);
                cmd.Parameters.AddWithValue("@office_phone", office.Phone);
                cmd.Parameters.AddWithValue("@office_email", office.Email);
                cmd.Parameters.AddWithValue("@office_is_inactive", office.IsInActive);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Päivittää toimipisteen tiedot.
        /// </summary>
        /// <param name="office"></param>
        public void UpdateOffice(Office office)
        {
            int officeID = office.OfficeID;
            string sql = "UPDATE Office SET office_street=@office_street, office_postal=@office_postal," +
                          "office_city=@office_city, office_phone=@office_phone, office_email=@office_email, office_is_inactive=@office_is_inactive " +
                          "WHERE office_id=@officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                cmd.Parameters.AddWithValue("@office_street", office.StreetAddress);
                cmd.Parameters.AddWithValue("@office_postal", office.PostalCode);
                cmd.Parameters.AddWithValue("@office_city", office.City);
                cmd.Parameters.AddWithValue("@office_phone", office.Phone);
                cmd.Parameters.AddWithValue("@office_email", office.Email);
                cmd.Parameters.AddWithValue("@office_is_inactive", office.IsInActive);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee kaikki halutun toimipisteen palvelut.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public ObservableCollection<Service> GetOfficeServices(Office office)
        {
            ObservableCollection<Service> services = new ObservableCollection<Service>();
            var sql = "SELECT * FROM service WHERE office_id = @officeID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        services.Add(new Service
                        {
                            ServiceID = dr.GetInt32("service_id"),
                            ServiceName = dr.GetString("service_name"),
                            ServiceDescription = dr.GetString("service_description"),
                            UnitPrice = dr["service_price"] == DBNull.Value ? 0.0 : (double)dr["service_price"],
                            PriceByHour = dr["service_price_hh"] == DBNull.Value ? 0.0 : (double)dr["service_price_hh"],
                            PriceByDay = dr["service_price_dd"] == DBNull.Value ? 0.0 : (double)dr["service_price_dd"],
                            PriceByWeek = dr["service_price_ww"] == DBNull.Value ? 0.0 : (double)dr["service_price_ww"],
                            PriceByMonth = dr["service_price_mm"] == DBNull.Value ? 0.0 : (double)dr["service_price_mm"],
                        });
                    }
                }
            }
            return services;
        }

        /// <summary>
        /// Hakee valitun toimipisteen kaikki laitteet
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>

        public ObservableCollection<Device> GetOfficeDevices(Office office)
        {
            ObservableCollection<Device> devices = new ObservableCollection<Device>();
            var sql = "SELECT * FROM device WHERE office_id = @officeID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        devices.Add(new Device
                        {
                            DeviceID = dr.GetInt32("device_id"),
                            Name = dr.GetString("device_name"),
                            Description = dr.GetString("device_description"),
                            PriceByHour = dr["device_price_hh"] == DBNull.Value ? 0.0 : (double)dr["device_price_hh"],
                            PriceByDay = dr["device_price_dd"] == DBNull.Value ? 0.0 : (double)dr["device_price_dd"],
                            PriceByWeek = dr["device_price_ww"] == DBNull.Value ? 0.0 : (double)dr["device_price_ww"],
                            PriceByMonth = dr["device_price_mm"] == DBNull.Value ? 0.0 : (double)dr["device_price_mm"],
                        });
                    }
                }
            }
            return devices;
        }



        /// <summary>
        /// Hakee kuinka monta laitetta kyseisessä toimipisteessä on.
        /// Käytetään varmistamaan, että toimipiste voidaan poistaa.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public bool GetOfficeDevicesAmount(Office office)
        {
            string sql = "SELECT COUNT(*) FROM Device WHERE office_id = @officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Hakee kuinka monta palvelua kyseisessä toimipisteessä on
        /// Käytetään varmistamaan, että toimipiste voidaan poistaa.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public bool GetOfficeServicesAmount(Office office)
        {
            string sql = "SELECT COUNT(*) FROM Service WHERE office_id = @officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Hakee kuinka monta varausta toimipisteellä on
        /// Käytetään varmistamaan, että toimipiste voidaan poistaa.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public bool GetOfficeReservationsAmount(Office office)
        {
            string sql = "SELECT COUNT(*) FROM Reservation WHERE office_id = @officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Hakee kuinka monta toimitilaa toimipisteellä on.
        /// Käytetään varmistamaan, että toimipiste voidaan poistaa.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public bool GetOfficeWorkspaceAmount(Office office)
        {
            string sql = "SELECT COUNT(*) FROM Workspace WHERE office_id = @officeID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Poistaa toimipisteen tietokannasta
        /// </summary>
        /// <param name="office"></param>
        public void DeleteOffice(Office office)
        {
            int officeID = office.OfficeID;
            string sql = "SELECT COUNT(*) FROM Office WHERE office_id = @officeID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@officeID", officeID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimipiste löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Office WHERE office_id=@officeID", conn);
                        cmdDelete.Parameters.AddWithValue("@officeID", office.OfficeID);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Toimipisteen poistaminen onnistui.");
                    }
                    else
                    {
                        // Toimipiste ei löytynyt tietokannasta
                        MessageBox.Show("Toimipiste numerolla: " + officeID + " ei löytynyt yhtään toimipistettä.");

                    }
                }
            }
        }
    }
}
