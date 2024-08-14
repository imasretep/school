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

namespace vt_systems.WorkspaceData
{
    internal class WorkSpaceRepo
    {
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=VTreservationsysAdmin; Pwd=VTreservationist01; Database=vtreservationsdb;";

        /// <summary>
        /// Hakee kaikki toimitilat
        /// </summary>
        /// <returns>Toimitilat</returns>
        public ObservableCollection<Workspace> GetWorkspaces()
        {
            var workspaces = new ObservableCollection<Workspace>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT workspace.*, office.office_city, office.office_id FROM workspace JOIN office ON office.office_id = workspace.office_id", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("ws_is_inactive") != true)
                    {
                        workspaces.Add(new Workspace
                        {
                            WorkspaceID = dr.GetInt32("workspace_id"),
                            WSName = dr.GetString("ws_name"),
                            WSDescription = dr.GetString("ws_description"),
                            PriceByHour = dr["ws_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_hh"]),
                            PriceByDay = dr["ws_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_dd"]),
                            PriceByWeek = dr["ws_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_ww"]),
                            PriceByMonth = dr["ws_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),
                            OfficeCity = dr.GetString("office_city")

                        });
                    }
                }
            }
            return workspaces;
        }

        /// <summary>
        /// Hakee kaikki ei aktiiviset toimitilat
        /// </summary>
        /// <returns>Toimitilat</returns>
        public ObservableCollection<Workspace> GetInactiveWorkspaces()
        {
            var workspaces = new ObservableCollection<Workspace>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Workspace", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetBoolean("ws_is_inactive") != false)
                    {
                        workspaces.Add(new Workspace
                        {
                            WorkspaceID = dr.GetInt32("workspace_id"),
                            WSName = dr.GetString("ws_name"),
                            WSDescription = dr.GetString("ws_description"),
                            PriceByHour = dr["ws_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_hh"]),
                            PriceByDay = dr["ws_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_dd"]),
                            PriceByWeek = dr["ws_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_ww"]),
                            PriceByMonth = dr["ws_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_mm"]),
                            OfficeID = dr.GetInt32("office_id"),

                        });
                    }
                }
            }
            return workspaces;
        }

        /// <summary>
        /// Hakee kaikki toimipisteen toimitilat.
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public ObservableCollection<Workspace> GetWorkspaces(Office office)
        {
            var workspaces = new ObservableCollection<Workspace>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Workspace WHERE office_id = @officeID ", conn);

                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr.GetBoolean("ws_is_inactive") != true)
                        {
                            workspaces.Add(new Workspace
                            {
                                WorkspaceID = dr.GetInt32("workspace_id"),
                                WSName = dr.GetString("ws_name"),
                                WSDescription = dr.GetString("ws_description"),
                                PriceByHour = dr["ws_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_hh"]),
                                PriceByDay = dr["ws_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_dd"]),
                                PriceByWeek = dr["ws_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_ww"]),
                                PriceByMonth = dr["ws_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_mm"]),
                                OfficeID = dr.GetInt32("office_id"),

                            });
                        }
                    }
                }
            }
            return workspaces;
        }

        /// <summary>
        /// Hakee työtilat, jotka on poistettu käytöstä
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public ObservableCollection<Workspace> GetInactiveWorkspaces(Office office)
        {
            var workspaces = new ObservableCollection<Workspace>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Workspace WHERE office_id = @officeID ", conn);

                cmd.Parameters.AddWithValue("@officeID", office.OfficeID);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr.GetBoolean("ws_is_inactive") != false)
                        {
                            workspaces.Add(new Workspace
                            {
                                WorkspaceID = dr.GetInt32("workspace_id"),
                                WSName = dr.GetString("ws_name"),
                                WSDescription = dr.GetString("ws_description"),
                                PriceByHour = dr["ws_price_hh"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_hh"]),
                                PriceByDay = dr["ws_price_dd"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_dd"]),
                                PriceByWeek = dr["ws_price_ww"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_ww"]),
                                PriceByMonth = dr["ws_price_mm"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["ws_price_mm"]),
                                OfficeID = dr.GetInt32("office_id"),

                            });
                        }
                    }
                }
            }
            return workspaces;
        }

        /// <summary>
        /// Hakee toimitilan
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public Workspace GetWorkspace(Workspace workspace)
        {
            int workspaceID = workspace.WorkspaceID;
            string sql = "SELECT COUNT(*) FROM Workspace WHERE workspace_id = @workspaceID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@workspaceID", workspaceID);

                    // Tarkistetaan onko toimitila tietokannassa.
                    // Jos count on enemmän kuin 0, tarkoittaa tämä sitä, että toimipiste löytyy tietokannasta.
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimitila löytyy tietokannasta
                        MySqlCommand cmdGetInfo = new MySqlCommand("SELECT * FROM Workspace WHERE workspace_id=@workspaceID", conn);
                        cmdGetInfo.Parameters.AddWithValue("@workspaceID", workspaceID);
                        using (MySqlDataReader reader = cmdGetInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                workspace.WorkspaceID = reader.GetInt32("workspace_id");
                                workspace.WSName = reader.GetString("ws_name");
                                workspace.WSDescription = reader.GetString("ws_description");

                                if (reader.IsDBNull("ws_price_hh"))
                                {
                                    workspace.PriceByHour = 0;
                                }
                                else
                                {
                                    workspace.PriceByHour = reader.GetDouble("ws_price_hh");
                                }


                                if (reader.IsDBNull("ws_price_dd"))
                                {
                                    workspace.PriceByDay = 0;
                                }
                                else
                                {
                                    workspace.PriceByDay = reader.GetDouble("ws_price_dd");
                                }

                                if (reader.IsDBNull("ws_price_ww"))
                                {
                                    workspace.PriceByWeek = 0;
                                }
                                else
                                {
                                    workspace.PriceByWeek = reader.GetDouble("ws_price_ww");
                                }

                                if (reader.IsDBNull("ws_price_mm"))
                                {
                                    workspace.PriceByMonth = 0;
                                }
                                else
                                {
                                    workspace.PriceByMonth = reader.GetDouble("ws_price_mm");
                                }

                                workspace.OfficeID = reader.GetInt32("office_id");
                                workspace.IsInActive = reader.GetBoolean("ws_is_inactive");

                            }
                        }
                    }
                    else
                    {
                        // Toimitila ei löytynyt tietokannasta
                        MessageBox.Show("Toimitila ei löytynyt.", "Virhe!");

                        return null;
                    }
                }
            }
            return workspace;
        }

        /// <summary>
        /// Lisää uuden toimitilan
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="office"></param>
        public void AddNewWorkspace(Workspace workspace, Office office)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Workspace(workspace_id, ws_name, ws_description, ws_price_hh, ws_price_dd, ws_price_ww, ws_price_mm, office_id, ws_is_inactive )" +
                                                    "VALUES(@workspace_id, @ws_name, @ws_description, @ws_price_hh, @ws_price_dd, @ws_price_ww, @ws_price_mm, @office_id, @ws_is_inactive);", conn);
                cmd.Parameters.AddWithValue("@workspace_id", workspace.WorkspaceID);
                cmd.Parameters.AddWithValue("@ws_name", workspace.WSName);
                cmd.Parameters.AddWithValue("@ws_description", workspace.WSDescription);
                cmd.Parameters.AddWithValue("@ws_price_hh", workspace.PriceByHour);
                cmd.Parameters.AddWithValue("@ws_price_dd", workspace.PriceByDay);
                cmd.Parameters.AddWithValue("@ws_price_ww", workspace.PriceByWeek);
                cmd.Parameters.AddWithValue("@ws_price_mm", workspace.PriceByMonth);
                cmd.Parameters.AddWithValue("@office_id", office.OfficeID);
                cmd.Parameters.AddWithValue("@ws_is_inactive", workspace.IsInActive);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Muokkaa olemassa olevaa toimitilaa
        /// </summary>
        /// <param name="workspace"></param>
        public void UpdateWorkspace(Workspace workspace)
        {
            int workspaceID = workspace.WorkspaceID;
            string sql = "UPDATE Workspace SET ws_name=@ws_name, ws_description=@ws_description," +
                          "ws_price_hh=@ws_price_hh, ws_price_dd=@ws_price_dd, ws_price_ww=@ws_price_ww, ws_price_mm=@ws_price_mm, ws_is_inactive=@ws_is_inactive " +
                          "WHERE workspace_id=@workspaceID";
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@workspaceID", workspaceID);
                cmd.Parameters.AddWithValue("@ws_name", workspace.WSName);
                cmd.Parameters.AddWithValue("@ws_description", workspace.WSDescription);
                cmd.Parameters.AddWithValue("@ws_price_hh", workspace.PriceByHour);
                cmd.Parameters.AddWithValue("@ws_price_dd", workspace.PriceByDay);
                cmd.Parameters.AddWithValue("@ws_price_ww", workspace.PriceByMonth);
                cmd.Parameters.AddWithValue("@ws_price_mm", workspace.PriceByWeek);
                cmd.Parameters.AddWithValue("@ws_is_inactive", workspace.IsInActive);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Poistaa toimitilan
        /// </summary>
        /// <param name="workspace"></param>
        public void DeleteWorkspace(Workspace workspace)
        {
            int workspaceID = workspace.WorkspaceID;
            string sql = "SELECT COUNT(*) FROM Workspace WHERE workspace_id = @workspaceID";

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@workspaceID", workspaceID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Toimipiste löytyy tietokannasta
                        MySqlCommand cmdDelete = new MySqlCommand("DELETE FROM Workspace WHERE workspace_id=@workspaceID", conn);
                        cmdDelete.Parameters.AddWithValue("@workspaceID", workspaceID);
                        cmdDelete.ExecuteNonQuery();
                        MessageBox.Show("Toimitilan poistaminen onnistui.");
                    }
                    else
                    {
                        // Toimipiste ei löytynyt tietokannasta
                        MessageBox.Show("Toimitilaa numerolla: " + workspaceID + " ei löytynyt yhtään toimitilaa.");

                    }
                }
            }
        }
    }
}
