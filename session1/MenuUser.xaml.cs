using session1.Session1_1DataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace session1
{
    /// <summary>
    /// Логика взаимодействия для MenuUser.xaml
    /// </summary>
    public partial class MenuUser : Window
    {
        public string log;
        public string id_users;
        public string name;
        public MenuUser(string log)
        {
            this.log = log;
            InitializeComponent();

            string ConnectBD = "Data Source = LAPTOP-HV0RLJLE;Initial Catalog = Session1_1; Integrated Security = True;";
            SqlConnection conn = new SqlConnection(ConnectBD);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [Users] WHERE [Email] = '" + log + "'", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id_users = reader["ID"].ToString();
                    name = reader["FirstName"].ToString();                  
                }
            }

            imya.Content = name;

            TrackingTableAdapter adapter = new TrackingTableAdapter();
            Session1_1DataSet.TrackingDataTable table = new Session1_1DataSet.TrackingDataTable();
            adapter.Fill(table, Convert.ToInt32(id_users));
            infa.ItemsSource = table;
        }

        string data;
        string logtime;
        string idtrack;
        string n;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            string ConnectBD = "Data Source = LAPTOP-HV0RLJLE;Initial Catalog = Session1_1; Integrated Security = True;";
            SqlConnection conn = new SqlConnection(ConnectBD);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [Tracking] WHERE [Users_ID] = '" + Convert.ToInt32(id_users) + "'", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    data = reader["Data"].ToString();
                    logtime = reader["LoginTime"].ToString();
                }
            }
            conn.Close();

            Convert.ToDateTime(logtime);
            //SqlCommand command1 = new SqlCommand("SELECT FROM [Tracking] WHERE [Users_ID] ='" + Convert.ToInt32(id_users) + "' ORDER BY [ID_Tracking] DESC LIMIT 1", conn);    
            SqlCommand command1 = new SqlCommand("SELECT MAX(ID_Tracking) from [Tracking] WHERE [Users_ID] = '" + Convert.ToInt32(id_users) + "'", conn);
            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                idtrack = ((int)reader["ID_Tracking"]).ToString();
                if (reader.Read())
                    throw new Exception("too many rows has been readed!");
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            conn.Close();

            var b = Convert.ToDateTime(logtime);
            var a = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            var c = a.Subtract(b);
            n = c.TotalHours.ToString() + ":" + c.TotalMinutes.ToString();


            using (SqlConnection connn =
            new SqlConnection(ConnectBD))
            {
                connn.Open();
                using (SqlCommand cmd =
                    new SqlCommand("UPDATE Tracking SET Users_ID=@idusers, Data=@data, LoginTime=@logintime, LogoutTime=@logouttime, TimeSpent=@timespent, Reason=@reason" +
                        " WHERE ID_Tracking=@Id", connn))
                {
                    cmd.Parameters.AddWithValue("@Id", idtrack);
                    cmd.Parameters.AddWithValue("@idusers", id_users);
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.Parameters.AddWithValue("@logintime", b.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@logouttime", DateTime.Now.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@timespent", n);
                    cmd.Parameters.AddWithValue("@reason", " ");
                    int rows = cmd.ExecuteNonQuery();
                    connn.Close();
                }
            }
           
            Environment.Exit(1);
        }
    }
}
