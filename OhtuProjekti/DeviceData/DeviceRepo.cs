using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using vt_systems.OfficeData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.DeviceData
{
    internal class DeviceRepo
    {
        // Default database connection string
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki laitteet
        /// </summary>
        /// <returns>Laitteet</returns>
        public ObservableCollection<Device> GetDevices()
        {
            var devices = new ObservableCollection<Device>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT device.*, office.office_city FROM device, office WHERE device.office_id = office.office_id ORDER BY device.device_id;", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("device_is_inactive") != true)
                    {
                        devices.Add(new Device
                        {
                            DeviceID = dr.GetInt32("device_id"),
                            Name = dr.GetString("device_name"),
                            Description = dr.GetString("device_description"),
                            PriceByHour = dr["device_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_hh"]),
                            PriceByDay = dr["device_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_dd"]),
                            PriceByWeek = dr["device_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_ww"]),
                            PriceByMonth = dr["device_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),
                            OfficeCity = dr.GetString("office_city")
                        });
                    }
                }
            }
            return devices;
        }


        /// <summary>
        /// Hakee kaikki laitteet office.id:n mukaan
        /// </summary>
        /// <returns>Laitteet</returns>
        public ObservableCollection<Device> GetDevices(Office office)
        {
            var devices = new ObservableCollection<Device>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM device WHERE office_id = @officeID", conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("device_is_inactive") != true)
                    {
                        devices.Add(new Device
                        {
                            DeviceID = dr.GetInt32("device_id"),
                            Name = dr.GetString("device_name"),
                            Description = dr.GetString("device_description"),
                            PriceByHour = dr["device_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_hh"]),
                            PriceByDay = dr["device_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_dd"]),
                            PriceByWeek = dr["device_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_ww"]),
                            PriceByMonth = dr["device_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),
                        });
                    }
                }
            }
            return devices;
        }


        /// <summary>
        /// Hakee kaikki ei aktiiviset laitteet
        /// </summary>
        /// <returns>Laitteet</returns>
        public ObservableCollection<Device> GetInactiveDevices()
        {
            var devices = new ObservableCollection<Device>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Device", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("device_is_inactive") != false)
                    {
                        devices.Add(new Device
                        {
                            DeviceID = dr.GetInt32("device_id"),
                            Name = dr.GetString("device_name"),
                            Description = dr.GetString("device_description"),
                            PriceByHour = dr["device_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_hh"]),
                            PriceByDay = dr["device_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_dd"]),
                            PriceByWeek = dr["device_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_ww"]),
                            PriceByMonth = dr["device_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_mm"]),
                            OfficeID = dr.GetInt32("office_id")
                        });
                    }
                }
            }
            return devices;
        }


        /// <summary>
        /// Hakee kaikki ei aktiiviset laitteet office.idn mukaan
        /// </summary>
        /// <returns>Laitteet</returns>
        public ObservableCollection<Device> GetInactiveDevices(Office office)
        {
            var devices = new ObservableCollection<Device>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Device WHERE office_id = @officeID", conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("device_is_inactive") != false)
                    {
                        devices.Add(new Device
                        {
                            DeviceID = dr.GetInt32("device_id"),
                            Name = dr.GetString("device_name"),
                            Description = dr.GetString("device_description"),
                            PriceByHour = dr["device_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_hh"]),
                            PriceByDay = dr["device_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_dd"]),
                            PriceByWeek = dr["device_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_ww"]),
                            PriceByMonth = dr["device_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["device_price_mm"]),
                            OfficeID = dr.GetInt32("office_id")
                        });
                    }
                }
            }
            return devices;
        }


        /// <summary>
        /// Hakee laitteen
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Device GetDevice(Device device)
        {
            int deviceID = device.DeviceID;
            string sql = "SELECT COUNT(*) FROM Device WHERE device_id = @DeviceID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DeviceID", deviceID);

                    // Tarkistetaan onko laite tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että laite löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Laite löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM Device WHERE device_id=@DeviceID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@deviceID", deviceID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                device.DeviceID = reader.GetInt32("device_id");
                                device.Name = reader.GetString("device_name");
                                device.Description = reader.GetString("device_description");

                                if (reader.IsDBNull("device_price_hh"))
                                {
                                    device.PriceByHour = 0;
                                }
                                else
                                {
                                    device.PriceByHour = reader.GetDouble("device_price_hh");
                                }

                                if (reader.IsDBNull("device_price_dd"))
                                {
                                    device.PriceByDay = 0;
                                }
                                else
                                {
                                    device.PriceByDay = reader.GetDouble("device_price_dd");
                                }

                                if (reader.IsDBNull("device_price_ww"))
                                {
                                    device.PriceByWeek = 0;
                                }
                                else
                                {
                                    device.PriceByWeek = reader.GetDouble("device_price_ww");
                                }

                                if (reader.IsDBNull("device_price_mm"))
                                {
                                    device.PriceByMonth = 0;
                                }
                                else
                                {
                                    device.PriceByMonth = reader.GetDouble("device_price_mm");
                                }
                                device.OfficeID = reader.GetInt32("office_id");
                                device.IsInActive = reader.GetBoolean("device_is_inactive");
                            }
                        }
                    }
                    else
                    {
                        // Laite ei löytynyt tietokannasta
                        MessageBox.Show("Laite ei löytynyt.", "Virhe!");

                        return null;
                    }
                }
            }
            return device;
        }


        /// <summary>
        /// Lisää uuden laitteen
        /// </summary>
        /// <param name="device"></param>
        /// <param name="office"></param>
        public void AddNewDevice(Device device, Office office)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Device(device_id, device_name, device_description, device_price_hh, device_price_dd, device_price_ww, device_price_mm, device_is_inactive, office_id )" +
                                                    "VALUES(@device_id, @device_name, @device_description, @device_price_hh, @device_price_dd, @device_price_ww, @device_price_mm, @device_is_inactive, @office_id);", conn);

                cmd.Parameters.AddWithValue("@device_id", device.DeviceID);
                cmd.Parameters.AddWithValue("@device_name", device.Name);
                cmd.Parameters.AddWithValue("@device_description", device.Description);
                cmd.Parameters.AddWithValue("@device_price_hh", device.PriceByHour);
                cmd.Parameters.AddWithValue("@device_price_dd", device.PriceByDay);
                cmd.Parameters.AddWithValue("@device_price_ww", device.PriceByWeek);
                cmd.Parameters.AddWithValue("@device_price_mm", device.PriceByMonth);
                cmd.Parameters.AddWithValue("@device_is_inactive", device.IsInActive);
                cmd.Parameters.AddWithValue("@office_id", office.OfficeID);

                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Muokkaa olemassa olevaa laitetta
        /// </summary>
        /// <param name="device"></param>
        public void UpdateDevice(Device device)
        {
            int deviceID = device.DeviceID;
            string sql = "UPDATE Device SET device_name=@device_name, device_description=@device_description," +
                          "device_price_hh=@device_price_hh, device_price_dd=@device_price_dd, device_price_ww=@device_price_ww, device_price_mm=@device_price_mm, " +
                          "device_is_inactive=@device_is_inactive WHERE device_id=@DeviceID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@deviceID", deviceID);
                cmd.Parameters.AddWithValue("@device_name", device.Name);
                cmd.Parameters.AddWithValue("@device_description", device.Description);
                cmd.Parameters.AddWithValue("@device_price_hh", device.PriceByHour);
                cmd.Parameters.AddWithValue("@device_price_dd", device.PriceByDay);
                cmd.Parameters.AddWithValue("@device_price_ww", device.PriceByWeek);
                cmd.Parameters.AddWithValue("@device_price_mm", device.PriceByMonth);
                cmd.Parameters.AddWithValue("@device_is_inactive", device.IsInActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Poistaa laitteen
        /// </summary>
        /// <param name="device"></param>
        public void DeleteDevice(Device device)
        {
            int deviceID = device.DeviceID;
            string sql = "SELECT COUNT(*) FROM Device WHERE device_id = @DeviceID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DeviceID", deviceID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimipiste löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Device WHERE device_id=@DeviceID", conn);
                        cmdDelete.Parameters.AddWithValue("@DeviceID", deviceID);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Laitteen poistaminen onnistui.");
                    }
                    else
                    {
                        // Toimipiste ei löytynyt tietokannasta
                        MessageBox.Show("Laitetteen numerolla: " + deviceID + " ei löytynyt yhtään laitetta.");
                    }
                }
            }
        }
    }
}
