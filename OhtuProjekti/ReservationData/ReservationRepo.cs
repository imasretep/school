using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using vt_systems.CustomerData;
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.ReservationData
{
    internal class ReservationRepo
    {
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki varaukset.
        /// </summary>
        /// <returns>OBSERVABLECOLLECTION</returns>
        public ObservableCollection<Reservation> GetReservations()
        {
            var reservations = new ObservableCollection<Reservation>();
            var reserverObjects = new ObservableCollection<ReservationObject>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Reservation", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    reservations.Add(new Reservation
                    {
                        ReservationId = dr.GetInt32("reservation_id"),
                        ReservationDate = dr.GetDateTime("reservation_date"),
                        ReleaseDate = dr.GetDateTime("release_date"),
                        CustomerID = dr.GetInt32("customer_id"),
                        OfficeID = dr.GetInt32("office_id"),
                        IsBilled = dr.GetBoolean("is_billed")
                    });
                }
            }
            return reservations;
        }

        /// <summary>
        /// Hakee tietyn varauksen.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public Reservation GetReservation(Reservation reservation)
        {
            int reservationID = reservation.ReservationId;
            string sql = "SELECT COUNT(*) FROM Reservation WHERE reservation_id = @reservationID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@reservationID", reservationID);

                    // Tarkistetaan onko toimipiste tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että toimipiste löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimipiste löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM reservation WHERE reservation_id=@reservationID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@reservationID", reservationID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reservation.ReservationId = reader.GetInt32("reservation_id");
                                reservation.ReservationDate = reader.GetDateTime("reservation_date");
                                reservation.ReleaseDate = reader.GetDateTime("release_date");
                                reservation.CustomerID = reader.GetInt32("customer_id");
                                reservation.OfficeID = reader.GetInt32("office_id");

                            }
                        }
                    }
                    else
                    {
                        // Varausta ei löytynyt tietokannasta
                        MessageBox.Show("Varausta ei löytynyt.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                        return null;
                    }
                }
            }
            return reservation;
        }


        /// <summary>
        /// Hakee tietyn toimitilan varatut päivät.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public ObservableCollection<Reservation> GetBookedDates(Workspace workspace)
        {
            var reservations = new ObservableCollection<Reservation>();
            var sql = "SELECT workspace_id, reservation_date, release_date " +
                        "FROM reservation, reserved_objects WHERE reservation.reservation_id = reserved_objects.reservation_id " +
                        "AND reserved_objects.workspace_id = @workspaceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@workspaceID", workspace.WorkspaceID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            ReservationDate = dr.GetDateTime("reservation_date"),
                            ReleaseDate = dr.GetDateTime("release_date"),
                        });
                    }
                }
            }
            return reservations;
        }

        /// <summary>
        /// Hakee tietyn varauksen ja sen datan.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public Reservation GetReservationWithData(Reservation reservation)
        {
            var sql = "SELECT reservation.reservation_id, reservation.reservation_date, reservation.release_date, reservation.customer_id, reservation.office_id, reservation_info, " +
                          "reserved_objects.qty, reserved_objects.unit_price, " +
                          "workspace.workspace_id, workspace.ws_name, workspace.ws_price_hh, workspace.ws_price_dd, workspace.ws_price_ww, workspace.ws_price_mm, " +
                          "device.device_id, device.device_name, device.device_price_hh, device.device_price_dd, device.device_price_ww, device.device_price_mm, " +
                          "service.service_id, service.service_name, service.service_description, service.service_price_hh, service.service_price_dd, service.service_price_ww, service.service_price_mm " +
                          "FROM reservation " +
                          "LEFT JOIN reserved_objects ON reservation.reservation_id = reserved_objects.reservation_id " +
                          "LEFT JOIN workspace ON reserved_objects.workspace_id = workspace.workspace_id " +
                          "LEFT JOIN device ON reserved_objects.device_id = device.device_id " +
                          "LEFT JOIN service ON reserved_objects.service_id = service.service_id " +
                          "WHERE reservation.reservation_id = @reservationID;";


            var newReservation = new Reservation();
            ObservableCollection<ReservationObject> newReservedObjects = new ObservableCollection<ReservationObject>();
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ReservationObject reservationObject = new ReservationObject();
                    Service service = new Service();
                    Device device = new Device();
                    Workspace workspace = new Workspace();

                    newReservation.ReservationId = dr.GetInt32("reservation_id");
                    newReservation.ReservationDate = dr.GetDateTime("reservation_date");
                    newReservation.ReleaseDate = dr.GetDateTime("release_date");
                    newReservation.ReservationInfo = dr.GetString("reservation_info");
                    newReservation.CustomerID = dr.GetInt32("customer_id");
                    newReservation.OfficeID = dr.GetInt32("office_id");

                    reservationObject.ReservationID = newReservation.ReservationId;

                    // UUSI TYÖTILA
                    if (!dr.IsDBNull("workspace_id"))
                    {
                        workspace.WorkspaceID = dr.GetInt32("workspace_id");
                        workspace.WSName = dr.GetString("ws_name");
                        workspace.OfficeID = dr.GetInt32("office_id");
                        if (!dr.IsDBNull("ws_price_hh"))
                        {
                            workspace.PriceByHour = dr.GetDouble("ws_price_hh");
                        }
                        if (!dr.IsDBNull("ws_price_dd"))
                        {
                            workspace.PriceByDay = dr.GetDouble("ws_price_dd");
                        }
                        if (!dr.IsDBNull("ws_price_ww"))
                        {
                            workspace.PriceByWeek = dr.GetDouble("ws_price_ww");
                        }
                        if (!dr.IsDBNull("ws_price_mm"))
                        {
                            workspace.PriceByMonth = dr.GetDouble("ws_price_mm");
                        }
                        reservationObject.Qty = dr.GetInt32("qty");
                        reservationObject.Price = dr.GetDouble("unit_price");
                        workspace.UnitPrice = dr.GetDouble("unit_price");
                        reservationObject.Workspace = workspace;
                        newReservedObjects.Add(reservationObject);
                    }

                    // UUSI PALVELU
                    if (!dr.IsDBNull("service_id"))
                    {
                        if (!dr.IsDBNull("service_id"))
                        {
                            service.ServiceID = dr.GetInt32("service_id");
                        }
                        if (!dr.IsDBNull("service_name"))
                        {
                            service.ServiceName = dr.GetString("service_name");
                        }
                        if (!dr.IsDBNull("service_description"))
                        {
                            service.ServiceDescription = dr.GetString("service_description");
                        }
                        if (!dr.IsDBNull("service_price_hh"))
                        {
                            service.PriceByHour = dr.GetDouble("service_price_hh");
                        }
                        if (!dr.IsDBNull("service_price_dd"))
                        {
                            service.PriceByDay = dr.GetDouble("service_price_dd");
                        }
                        if (!dr.IsDBNull("service_price_ww"))
                        {
                            service.PriceByWeek = dr.GetDouble("service_price_ww");
                        }
                        if (!dr.IsDBNull("service_price_mm"))
                        {
                            service.PriceByMonth = dr.GetDouble("service_price_mm");
                        }
                        if (!dr.IsDBNull("qty"))
                        {
                            service.Quantity = dr.GetInt32("qty");
                        }
                        if (!dr.IsDBNull("unit_price"))
                        {
                            service.UnitPrice = dr.GetDouble("unit_price");
                        }
                        reservationObject.Service = service;
                        newReservedObjects.Add(reservationObject);
                    }

                    // UUSI LAITE
                    if (!dr.IsDBNull("device_id"))
                    {
                        if (!dr.IsDBNull("device_id"))
                        {
                            device.DeviceID = dr.GetInt32("device_id");
                        }
                        if (!dr.IsDBNull("device_name"))
                        {
                            device.Name = dr.GetString("device_name");
                        }
                        if (!dr.IsDBNull("device_price_hh"))
                        {
                            device.PriceByHour = dr.GetDouble("device_price_hh");
                        }
                        if (!dr.IsDBNull("device_price_dd"))
                        {
                            device.PriceByDay = dr.GetDouble("device_price_dd");
                        }
                        if (!dr.IsDBNull("device_price_ww"))
                        {
                            device.PriceByWeek = dr.GetDouble("device_price_ww");
                        }
                        if (!dr.IsDBNull("device_price_mm"))
                        {
                            device.PriceByMonth = dr.GetDouble("device_price_mm");
                        }
                        if (!dr.IsDBNull("qty"))
                        {
                            device.Quantity = dr.GetInt32("qty");
                        }
                        if (!dr.IsDBNull("unit_price"))
                        {
                            device.UnitPrice = dr.GetDouble("unit_price");
                        }
                        reservationObject.Device = device;
                        newReservedObjects.Add(reservationObject);
                    }
                }

                newReservation.ReservedObjects = newReservedObjects;
            }
            return newReservation;
        }

        /// <summary>
        /// Poistaa valitun varauksen
        /// </summary>
        /// <param name="reservation"></param>
        public void DeleteReservation(Reservation reservation)
        {
            int reservationID = reservation.ReservationId;
            string sql = "SELECT COUNT(*) FROM Reservation WHERE reservation_id = @reservationID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@reservationID", reservationID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Varaus löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Reservation WHERE reservation_id=@reservationID", conn);
                        cmdDelete.Parameters.AddWithValue("@reservationID", reservationID);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Varauksen poistaminen onnistui.");
                    }
                    else
                    {
                        // Varaus ei löytynyt tietokannasta
                        MessageBox.Show("Varausta numerolla: " + reservationID + " ei löytynyt.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }
        }

        /// <summary>
        /// Lisää varauksen tietokantaan
        /// </summary>
        /// <param name="reservation"></param>
        public void AddNewReservation(Reservation reservation)
        {
            var sqlReservation = "INSERT INTO reservation(reservation_date, release_date, reservation_info, customer_id, office_id, is_billed )" +
                                                    "VALUES(@reservation_date, @release_date, @reservation_info, @customer_id,  @office_id, @is_billed);";



            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlReservation, conn);

                cmd.Parameters.AddWithValue("@reservation_date", reservation.ReservationDate);
                cmd.Parameters.AddWithValue("@release_date", reservation.ReleaseDate);
                cmd.Parameters.AddWithValue("@reservation_info", reservation.ReservationInfo);
                cmd.Parameters.AddWithValue("@customer_id", reservation.Customer.Id);
                cmd.Parameters.AddWithValue("@office_id", reservation.OfficeID);
                cmd.Parameters.AddWithValue("@is_billed", reservation.IsBilled);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Hakee varauksen tiedot IDllä
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public Reservation GetReservationId(Reservation reservation, Workspace workspace)
        {
            var sql = "SELECT reservation.reservation_id FROM reservation " +
                        "JOIN workspace ON reservation.office_id = workspace.office_id " +
                        "WHERE reservation.reservation_date = @reservationDate " +
                        "AND reservation.release_date = @releaseDate " +
                        "AND workspace.workspace_id = @workspaceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmdGetInfo = new MySqlCommand(sql, conn);
                cmdGetInfo.Parameters.AddWithValue("@reservationDate", reservation.ReservationDate);
                cmdGetInfo.Parameters.AddWithValue("@releaseDate", reservation.ReleaseDate);
                cmdGetInfo.Parameters.AddWithValue("@workspaceID", workspace.WorkspaceID);
                using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation.ReservationId = reader.GetInt32("reservation_id");
                    }
                }
            }
            return reservation;
        }

        /// <summary>
        /// Lisää rivitiedon varaukselle (laite, palvelu tai toimitila)
        /// </summary>
        /// <param name="reservation"></param>
        public void AddNewReservedObject(Reservation reservation)
        {
            var sqlReservation = "INSERT INTO reserved_objects(reservation_id, service_id, device_id, workspace_id, qty, unit_price )" +
                                        "VALUES(@reservation_id, @service_id, @device_id, @workspace_id, @qty, @unit_price);";

            TimeSpan rentalPeriod = reservation.ReleaseDate - reservation.ReservationDate;
            int daysRented = rentalPeriod.Days + 1;

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sqlReservation, conn);
                foreach (var item in reservation.ReservedObjects)
                {
                    cmd.Parameters.AddWithValue("@reservation_id", reservation.ReservationId);

                    if (item.Service != null)
                    {
                        cmd.Parameters.AddWithValue("@service_id", item.Service.ServiceID);
                        cmd.Parameters.AddWithValue("@qty", item.Service.Quantity);
                        cmd.Parameters.AddWithValue("@unit_price", item.Service.UnitPrice);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@service_id", DBNull.Value);
                    }

                    if (item.Device != null)
                    {
                        cmd.Parameters.AddWithValue("@device_id", item.Device.DeviceID);
                        cmd.Parameters.AddWithValue("@qty", item.Device.Quantity);
                        cmd.Parameters.AddWithValue("@unit_price", item.Device.UnitPrice);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@device_id", DBNull.Value);
                    }

                    if (item.Workspace != null)
                    {
                        cmd.Parameters.AddWithValue("@workspace_id", item.Workspace.WorkspaceID);
                        cmd.Parameters.AddWithValue("@qty", daysRented);
                        cmd.Parameters.AddWithValue("@unit_price", item.Price);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@workspace_id", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Päivittää tietokantaan varauksen
        /// </summary>
        /// <param name="reservation"></param>
        public void UpdateReservation(Reservation reservation)
        {

            string sqlUpdateReservation = "Update reservation SET reservation_date = @reservationDate, release_date = @releaseDate, reservation_info = @reservationInfo WHERE reservation_id = @reservationID;";

            string sqlUpdateDevice = "Update reserved_objects SET qty = @quantity WHERE reservation_id = @reservationID AND device_id = @deviceID;";
            string sqlUpdateService = "Update reserved_objects SET qty = @quantity WHERE reservation_id = @reservationID AND service_id = @serviceID;";

            var sqlInsertDevice = "INSERT INTO reserved_objects(reservation_id, device_id, qty, unit_price )" +
                                    "VALUES(@reservation_id, @deviceID, @qty, @unit_price);";

            var sqlInsertService = "INSERT INTO reserved_objects(reservation_id, service_id, qty, unit_price )" +
                                    "VALUES(@reservation_id, @serviceID, @qty, @unit_price);";


            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdateReservation, conn);
                cmdUpdate.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdUpdate.Parameters.AddWithValue("@reservationDate", reservation.ReservationDate);
                cmdUpdate.Parameters.AddWithValue("@releaseDate", reservation.ReleaseDate);
                cmdUpdate.Parameters.AddWithValue("@reservationInfo", reservation.ReservationInfo);
                cmdUpdate.ExecuteNonQuery();

                foreach (var item in reservation.ReservedObjects)
                {
                    if (item.Device != null)
                    {
                        cmdUpdate = new MySqlCommand(sqlUpdateDevice, conn);
                        cmdUpdate.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                        cmdUpdate.Parameters.AddWithValue("@deviceID", item.Device.DeviceID);
                        cmdUpdate.Parameters.AddWithValue("@quantity", item.Device.Quantity);
                        int rowsUpdated = cmdUpdate.ExecuteNonQuery();

                        if (rowsUpdated == 0)
                        {
                            MySqlCommand cmdInsert = new MySqlCommand(sqlInsertDevice, conn);
                            cmdInsert.Parameters.AddWithValue("@reservation_id", reservation.ReservationId);
                            cmdInsert.Parameters.AddWithValue("@deviceID", item.Device.DeviceID);
                            cmdInsert.Parameters.AddWithValue("@qty", item.Device.Quantity);
                            cmdInsert.Parameters.AddWithValue("@unit_price", item.Device.UnitPrice);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                    else if (item.Service != null)
                    {

                        // Service
                        cmdUpdate = new MySqlCommand(sqlUpdateService, conn);
                        cmdUpdate.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                        cmdUpdate.Parameters.AddWithValue("@serviceID", item.Service.ServiceID);
                        cmdUpdate.Parameters.AddWithValue("@quantity", item.Service.Quantity);
                        int rowsUpdated = cmdUpdate.ExecuteNonQuery();

                        if (rowsUpdated == 0)
                        {
                            MySqlCommand cmdInsert = new MySqlCommand(sqlInsertService, conn);
                            cmdInsert.Parameters.AddWithValue("@reservation_id", reservation.ReservationId);
                            cmdInsert.Parameters.AddWithValue("@serviceID", item.Service.ServiceID);
                            cmdInsert.Parameters.AddWithValue("@qty", item.Service.Quantity);
                            cmdInsert.Parameters.AddWithValue("@unit_price", item.Service.UnitPrice);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                    else if (item.Workspace != null)
                    {
                        // Workspace --> tähän tulee koodia, jos varausta muokatessa voidaan myös vaihtaa toimitila.      
                    }
                }
            }
        }

        /// <summary>
        /// Poistaa palvelun varaukselta (reserved_objects).
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="service"></param>
        public void RemoveServiceFromReservation(Reservation reservation, Service service)
        {
            string sql = "DELETE FROM reserved_objects WHERE reservation_id = @reservationID AND service_id = @serviceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdRemove = new MySqlCommand(sql, conn);
                cmdRemove.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdRemove.Parameters.AddWithValue("@serviceID", service.ServiceID);
                cmdRemove.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa laitteen varaukselta (reserved_objects).
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="device"></param>
        public void RemoveDeviceFromReservation(Reservation reservation, Device device)
        {
            string sql = "DELETE FROM reserved_objects WHERE reservation_id = @reservationID AND device_id = @deviceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdRemove = new MySqlCommand(sql, conn);
                cmdRemove.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdRemove.Parameters.AddWithValue("@deviceID", device.DeviceID);
                cmdRemove.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa toimitilan varaukselta (reserved_objects).
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="workspace"></param>
        public void RemoveWorkspaceFromReservation(Reservation reservation, Workspace workspace)
        {
            string sql = "DELETE FROM reserved_objects WHERE reservation_id = @reservationID AND workspace_id = @workspaceID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdRemove = new MySqlCommand(sql, conn);
                cmdRemove.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdRemove.Parameters.AddWithValue("@workspaceID", workspace.WorkspaceID);
                cmdRemove.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa asiakkaan varaukselta.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="customer"></param>
        public void RemoveCustomerFromReservation(Reservation reservation, int customerID)
        {
            string sql = "DELETE FROM reservation WHERE reservation_id = @reservationID AND customer_id = @customerID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdRemove = new MySqlCommand(sql, conn);
                cmdRemove.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdRemove.Parameters.AddWithValue("@customerID", customerID);
                cmdRemove.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa Toimipisteen varaukselta.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="customer"></param>
        public void RemoveOfficeFromReservation(Reservation reservation, int officeID)
        {
            string sql = "DELETE FROM reservation WHERE reservation_id = @reservationID AND office_id = @officeID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmdRemove = new MySqlCommand(sql, conn);
                cmdRemove.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdRemove.Parameters.AddWithValue("@officeID", officeID);
                cmdRemove.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee asiakkaan IDt laskukokoelmaan, mistä tarkistetaan onko asiakkaalla varauksia ( asiakkaan poisto )
        /// </summary>
        /// <param name="id">Asiakkaan id</param>
        /// <returns>Asiakkaan tiedot Customer luokan ilmentymänä.</returns>

        public ObservableCollection<Reservation> GetReservationLines(Customer customer)
        {
            var reservationLines = new ObservableCollection<Reservation>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT customer_id FROM reservation WHERE customer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", customer.Id);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Reservation reservation = new Reservation();
                    {
                        reservation.CustomerID = dr.GetInt32("customer_id");
                    }
                    reservationLines.Add(reservation);
                }
            }
            return reservationLines;
        }


        /// <summary>
        /// Hakee varaukseen varatut laitteet
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public ObservableCollection<ReservationObject> GetReservedDevices(Device device)
        {
            var reservedDevices = new ObservableCollection<ReservationObject>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT device_id FROM reserved_objects WHERE device_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", device.DeviceID);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReservationObject reservationObject = new ReservationObject();
                    {
                        reservationObject.DeviceID = dr.GetInt32("device_id");
                    }
                    reservedDevices.Add(reservationObject);
                }

            }
            return reservedDevices;
        }

        /// <summary>
        /// Hakee varaukseen varatut palvelut
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public ObservableCollection<ReservationObject> GetReservedServices(Service service)
        {
            var reservedServices = new ObservableCollection<ReservationObject>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT service_id FROM reserved_objects WHERE service_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", service.ServiceID);
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReservationObject reservationObject = new ReservationObject();
                    {
                        reservationObject.SeriveID = dr.GetInt32("service_id");
                    }
                    reservedServices.Add(reservationObject);
                }

            }
            return reservedServices;
        }

        /// <summary>
        /// Päivittää varauksen tilan is_billed true/false 1/0
        /// </summary>
        /// <param name="reservation"></param>
        public void UpdateReservationStatus(Reservation reservation)
        {

            string sqlUpdateReservation = "Update reservation SET is_billed = @is_billed WHERE reservation_id = @reservationID;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdateReservation, conn);
                cmdUpdate.Parameters.AddWithValue("@is_billed", reservation.IsBilled);
                cmdUpdate.Parameters.AddWithValue("@reservationID", reservation.ReservationId);
                cmdUpdate.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee halutun toimipisteen kaikki varatut toimitilat.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public ObservableCollection<Reservation> GetBookedWorkspaces(Office office)
        {
            var reservations = new ObservableCollection<Reservation>();
            var sql = "SELECT workspace.workspace_id, workspace.ws_name, reservation.reservation_date, reservation.release_date, customer.cust_first_name, customer.cust_last_name, reservation.reservation_id " +
                "FROM reservation JOIN customer ON reservation.customer_id = customer.customer_id " +
                "JOIN reserved_objects ON reservation.reservation_id = reserved_objects.reservation_id " +
                "JOIN workspace ON reserved_objects.workspace_id = workspace.workspace_id " +
                "WHERE reservation.office_id = @officeID;";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var reservation = new Reservation();
                        var customer = new Customer();
                        var workspace = new Workspace();
                        var reservedObject = new ReservationObject();
                        workspace.WorkspaceID = dr.GetInt32(0);
                        workspace.WSName = dr.GetString(1);
                        reservation.ReservationDate = dr.GetDateTime(2);
                        reservation.ReleaseDate = dr.GetDateTime(3);
                        customer.FirstName = dr.GetString(4);
                        customer.LastName = dr.GetString(5);
                        reservation.ReservationId = dr.GetInt32(6);
                        reservedObject.Workspace = workspace;
                        reservation.ReservedObjects.Add(reservedObject);
                        reservation.Customer = customer;
                        reservations.Add(reservation);
                    }
                }
            }
            return reservations;
        }
    }
}
