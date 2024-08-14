using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using vt_systems.CustomerData;
using vt_systems.DeviceData;
using vt_systems.OfficeData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.ReportData
{
    internal class ReportRepo
    {
        // Default database connection string
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";


        /// <summary>
        /// Hakee annetulta aikaväliltä toimitilat
        /// </summary>
        /// <returns>Listan(ObservableCollection) toimitiloista</returns>
        public ObservableCollection<Report> GetReportDataWorkspaces(Office office, DateTime startDate, DateTime endDate)
        {
            var reports = new ObservableCollection<Report>();
            var sql = "SELECT workspace.workspace_id, workspace.ws_name, workspace.ws_price_dd, workspace.ws_price_ww, workspace.ws_price_mm, reservation.reservation_date, reservation.release_date, customer.cust_first_name, customer.cust_last_name " +
                        "FROM reservation " +
                        "JOIN customer ON reservation.customer_id = customer.customer_id " +
                        "JOIN reserved_objects ON reservation.reservation_id = reserved_objects.reservation_id " +
                        "JOIN workspace ON reserved_objects.workspace_id = workspace.workspace_id " +
                        "WHERE reservation.office_id = @officeID " +
                        "AND reservation.reservation_date >= @startDate " +
                        "AND reservation.release_date <= @endDate;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var report = new Report();
                        var customer = new Customer();
                        var workspace = new Workspace();

                        workspace.WorkspaceID = dr.GetInt32(0);
                        workspace.WSName = dr.GetString(1);
                        workspace.PriceByDay = dr[2] == DBNull.Value ? 0.0 : (double)dr[2];
                        workspace.PriceByWeek = dr[3] == DBNull.Value ? 0.0 : (double)dr[3];
                        workspace.PriceByMonth = dr[4] == DBNull.Value ? 0.0 : (double)dr[4];

                        report.StartDate = dr.GetDateTime(5);
                        report.EndDate = dr.GetDateTime(6);

                        TimeSpan rentalPeriod = report.EndDate - report.StartDate;
                        int daysRented = rentalPeriod.Days + 1;
                        report.DaysRented = daysRented;
                        report.Sum = daysRented * workspace.PriceByDay;

                        customer.FirstName = dr.GetString(7);
                        customer.LastName = dr.GetString(8);

                        report.Workspace = workspace;
                        report.Customer = customer;

                        reports.Add(report);
                    }
                }
            }
            return reports;
        }

        /// <summary>
        /// Hakee annetulta aikaväliltä palvelut
        /// </summary>
        /// <returns>Listan(ObservableCollection) palveluista</returns>
        public ObservableCollection<Report> GetReportDataServices(Office office, DateTime startDate, DateTime endDate)
        {
            var reports = new ObservableCollection<Report>();
            var sqlService = "SELECT reservation.reservation_date, reservation.release_date, service.service_name, service.service_price_dd, reserved_objects.qty, customer.cust_first_name, customer.cust_last_name " +
                                "FROM reservation JOIN customer ON reservation.customer_id = customer.customer_id " +
                                "LEFT JOIN reserved_objects ON reservation.reservation_id = reserved_objects.reservation_id " +
                                "LEFT JOIN service ON reserved_objects.service_id = service.service_id " +
                                "WHERE reservation.office_id = @officeID " +
                                "AND reservation.reservation_date >= @startDate " +
                                "AND reservation.reservation_date <= @endDate " +
                                "AND reserved_objects.service_id IS NOT NULL;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Service
                MySqlCommand cmd = new MySqlCommand(sqlService, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var report = new Report();
                        var customer = new Customer();
                        var service = new Service();

                        service.ServiceName = dr.GetString(2);
                        service.ServicePrice  = dr[3] == DBNull.Value ? 0.0 : (double)dr[3];
                        service.Quantity = dr.GetInt32(4);
                        
                        customer.FirstName = dr.GetString(5);
                        customer.LastName = dr.GetString(6);

                        report.StartDate = dr.GetDateTime(0);
                        report.EndDate = dr.GetDateTime(1);

                        TimeSpan rentalPeriod = report.EndDate - report.StartDate;
                        int daysRented = rentalPeriod.Days + 1;
                        report.DaysRented = daysRented;
                        report.Sum = (double)(Convert.ToDouble(daysRented) * service.ServicePrice);
                        report.Sum = report.Sum * service.Quantity;

                        report.Customer = customer;
                        report.Service = service;

                        reports.Add(report);
                    }
                }
            }
            return reports;
        }


        /// <summary>
        /// Hakee annetulta aikaväliltä laitteet
        /// </summary>
        /// <returns>Listan(ObservableCollection) laitteista</returns>
        public ObservableCollection<Report> GetReportDataDevices(Office office, DateTime startDate, DateTime endDate)
        {
            var reports = new ObservableCollection<Report>();
            var sqlDevice = "SELECT reservation.reservation_date, reservation.release_date, device.device_name, device.device_price_hh, device.device_price_dd, device.device_price_ww, device.device_price_mm, reserved_objects.qty, customer.cust_first_name, customer.cust_last_name " +
                            "FROM reservation " +
                            "JOIN customer ON reservation.customer_id = customer.customer_id " +
                            "LEFT JOIN reserved_objects ON reservation.reservation_id = reserved_objects.reservation_id " +
                            "LEFT JOIN device ON reserved_objects.device_id = device.device_id " +
                            "WHERE  reservation.office_id = @officeID " +
                            "AND reservation.reservation_date >= @startDate " +
                            "AND reservation.reservation_date <= @endDate " +
                            "AND reserved_objects.device_id IS NOT NULL;";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Device
                MySqlCommand cmd = new MySqlCommand(sqlDevice, conn);
                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var report = new Report();
                        var customer = new Customer();
                        var device = new Device();

                        device.Name = dr.GetString(2);
                        device.PriceByDay = dr[3] == DBNull.Value ? 0.0 : (double)dr[3];
                        device.PriceByDay = dr[4] == DBNull.Value ? 0.0 : (double)dr[4];
                        device.PriceByWeek = dr[5] == DBNull.Value ? 0.0 : (double)dr[5];
                        device.PriceByMonth = dr[6] == DBNull.Value ? 0.0 : (double)dr[6];
                        device.Quantity = dr.GetInt32(7);

                        customer.FirstName = dr.GetString(8);
                        customer.LastName = dr.GetString(9);

                        report.StartDate = dr.GetDateTime(0);
                        report.EndDate = dr.GetDateTime(1);

                        TimeSpan rentalPeriod = report.EndDate - report.StartDate;
                        int daysRented = rentalPeriod.Days + 1;
                        report.DaysRented = daysRented;
                        report.Sum = (double)(Convert.ToDouble(daysRented) * device.PriceByDay);

                        report.Customer = customer;
                        report.Device = device;

                        reports.Add(report);
                    }
                }
            }
            return reports;
        }
    }
}
