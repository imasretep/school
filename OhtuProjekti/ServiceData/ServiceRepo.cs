using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using vt_systems.DeviceData;
using vt_systems.OfficeData;

namespace vt_systems.ServiceData
{
    internal class ServiceRepo
    {
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki toimipisteet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Service> GetServices()
        {
            var services = new ObservableCollection<Service>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT service.*, office.office_city FROM service, office WHERE service.office_id = office.office_id ORDER BY service.service_id;", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("service_is_inactive") != true)
                    {
                        services.Add(new Service
                        {
                            ServiceID = dr.GetInt32("service_id"),
                            ServiceName = dr.GetString("service_name"),
                            ServiceDescription = dr.GetString("service_description"),
                            ServicePrice = dr["service_price"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price"]),
                            PriceByHour = dr["service_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_hh"]),
                            PriceByDay = dr["service_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_dd"]),
                            PriceByWeek = dr["service_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_ww"]),
                            PriceByMonth = dr["service_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),
                            OfficeCity = dr.GetString("office_city")
                        });
                    }
                }
            }
            return services;
        }

        /// <summary>
        /// Hakee kaikki toimipisteet office.idn mukaan
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Service> GetServices(Office office)
        {
            var services = new ObservableCollection<Service>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM service WHERE office_id = @officeID;", conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("service_is_inactive") != true)
                    {
                        services.Add(new Service
                        {
                            ServiceID = dr.GetInt32("service_id"),
                            ServiceName = dr.GetString("service_name"),
                            ServiceDescription = dr.GetString("service_description"),
                            ServicePrice = dr["service_price"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price"]),
                            PriceByHour = dr["service_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_hh"]),
                            PriceByDay = dr["service_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_dd"]),
                            PriceByWeek = dr["service_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_ww"]),
                            PriceByMonth = dr["service_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),
                        });
                    }
                }
            }
            return services;
        }

        /// <summary>
        /// Hakee kaikki ei aktiiviset toimipisteet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Service> GetInactiveServices()
        {
            var services = new ObservableCollection<Service>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Service", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("service_is_inactive") != false)
                    {
                        services.Add(new Service
                        {
                            ServiceID = dr.GetInt32("service_id"),
                            ServiceName = dr.GetString("service_name"),
                            ServiceDescription = dr.GetString("service_description"),
                            ServicePrice = dr["service_price"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price"]),
                            PriceByHour = dr["service_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_hh"]),
                            PriceByDay = dr["service_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_dd"]),
                            PriceByWeek = dr["service_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_ww"]),
                            PriceByMonth = dr["service_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_mm"]),
                            OfficeID = dr.GetInt32("office_id")
                        });
                    }
                }
            }
            return services;
        }

        /// <summary>
        /// Hakee kaikki ei aktiiviset toimipisteet office.idn mukaan
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Service> GetInactiveServices(Office office)
        {
            var services = new ObservableCollection<Service>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Service WHERE office_id = @officeID", conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("service_is_inactive") != false)
                    {
                        services.Add(new Service
                        {
                            ServiceID = dr.GetInt32("service_id"),
                            ServiceName = dr.GetString("service_name"),
                            ServiceDescription = dr.GetString("service_description"),
                            ServicePrice = dr["service_price"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price"]),
                            PriceByHour = dr["service_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_hh"]),
                            PriceByDay = dr["service_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_dd"]),
                            PriceByWeek = dr["service_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_ww"]),
                            PriceByMonth = dr["service_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["service_price_mm"]),
                            OfficeID = dr.GetInt32("office_id")
                        });
                    }
                }
            }
            return services;
        }
        /// <summary>
        /// Hakee tietokannasta palvelun
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Service GetService(Service service)
        {
            int serviceID = service.ServiceID;
            string sql = "SELECT COUNT(*) FROM Service WHERE service_id = @ServiceID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceID", serviceID);

                    // Tarkistetaan onko palvelu tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että palvelu löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Palvelu löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM service WHERE service_id=@ServiceID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@ServiceID", serviceID);

                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                service.ServiceID = reader.GetInt32("service_id");
                                service.ServiceName = reader.GetString("service_name");
                                service.ServiceDescription = reader.GetString("service_description");

                                if (reader.IsDBNull("service_price"))
                                {
                                    service.ServicePrice = 0;
                                }
                                else
                                {
                                    service.ServicePrice = reader.GetDouble("service_price");
                                }

                                if (reader.IsDBNull("service_price_hh"))
                                {
                                    service.PriceByHour = 0;
                                }
                                else
                                {
                                    service.PriceByHour = reader.GetDouble("service_price_hh");
                                }


                                if (reader.IsDBNull("service_price_dd"))
                                {
                                    service.PriceByDay = 0;
                                }
                                else
                                {
                                    service.PriceByDay = reader.GetDouble("service_price_dd");
                                }

                                if (reader.IsDBNull("service_price_ww"))
                                {
                                    service.PriceByWeek = 0;
                                }
                                else
                                {
                                    service.PriceByWeek = reader.GetDouble("service_price_ww");
                                }

                                if (reader.IsDBNull("service_price_mm"))
                                {
                                    service.PriceByMonth = 0;
                                }
                                else
                                {
                                    service.PriceByMonth = reader.GetDouble("service_price_mm");
                                }

                                service.OfficeID = reader.GetInt32("office_id");
                                service.IsInActive = reader.GetBoolean("service_is_inactive");
                            }
                        }
                    }
                    else
                    {
                        // Palvelua ei löytynyt tietokannasta
                        MessageBox.Show("Palvelua ei löytynyt.", "Virhe!", MessageBoxButton.OK, MessageBoxImage.Information);

                        return null;
                    }
                }
            }
            return service;
        }

        /// <summary>
        /// Lisää uuden palvelun
        /// </summary>
        /// <param name="service"></param>
        /// <param name="office"></param>
        public void AddNewService(Service service, Office office)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO service (service_id, service_name, service_description, service_price, service_price_hh, service_price_dd, service_price_ww, service_price_mm, office_id, service_is_inactive)" +
                                                    "VALUES(@service_id, @service_name, @service_description, @service_price, @service_price_hh, @service_price_dd, @service_price_ww, @service_price_mm, @office_id, @service_is_inactive)", conn);

                cmd.Parameters.AddWithValue("@service_id", service.ServiceID);
                cmd.Parameters.AddWithValue("@service_name", service.ServiceName);
                cmd.Parameters.AddWithValue("@service_description", service.ServiceDescription);
                cmd.Parameters.AddWithValue("@service_price", service.ServicePrice);
                cmd.Parameters.AddWithValue("@service_price_hh", service.PriceByHour);
                cmd.Parameters.AddWithValue("@service_price_dd", service.PriceByDay);
                cmd.Parameters.AddWithValue("@service_price_ww", service.PriceByWeek);
                cmd.Parameters.AddWithValue("@service_price_mm", service.PriceByMonth);
                cmd.Parameters.AddWithValue("@office_id", office.OfficeID);
                cmd.Parameters.AddWithValue("@service_is_inactive", service.IsInActive);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Päivittää palvelun tiedot.
        /// </summary>
        /// <param name="service"></param>
        public void UpdateService(Service service)
        {
            int serviceID = service.ServiceID;
            string sql = "UPDATE service SET service_name = @Service_Name, service_description = @service_description, service_price = @service_price, " +
                "service_price_hh =@service_price_hh, service_price_dd = @service_price_dd, service_price_ww = @service_price_ww, service_price_mm = @service_price_mm, service_is_inactive=@service_is_inactive " +
                "WHERE service_id=@service_id";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))

            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@service_id", serviceID);
                cmd.Parameters.AddWithValue("@service_name", service.ServiceName);
                cmd.Parameters.AddWithValue("@service_description", service.ServiceDescription);
                cmd.Parameters.AddWithValue("@service_price", service.ServicePrice);
                cmd.Parameters.AddWithValue("@service_price_hh", service.PriceByHour);
                cmd.Parameters.AddWithValue("@service_price_dd", service.PriceByDay);
                cmd.Parameters.AddWithValue("@service_price_ww", service.PriceByWeek);
                cmd.Parameters.AddWithValue("@service_price_mm", service.PriceByMonth);
                cmd.Parameters.AddWithValue("@service_is_inactive", service.IsInActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa valitun palvelun
        /// </summary>
        /// <param name="service"></param>
        public void DeleteService(Service service)
        {
            int serviceID = service.ServiceID;
            string sql = "SELECT COUNT(*) FROM Service WHERE service_id = @ServiceID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceID", serviceID);

                    // Tarkistetaan onko palvelu tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että palvelu löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Palvelu löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM service WHERE service_id=@ServiceID", conn);
                        cmdDelete.Parameters.AddWithValue("@ServiceID", serviceID);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Palvelun poistaminen onnistui.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Palvelu ei löytynyt tietokannasta
                        MessageBox.Show("Palvelua numerolla: " + serviceID + " ei löytynyt.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
