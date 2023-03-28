using session1.Session1_1DataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        public string log;
        public string idi;
        string id_users;
        string act;
        string FirstName;
        string Last_Name;
        string passwd;
        string ofis;
        string birth;
        string role;
        string email;
        public MenuAdmin(string log)
        {
            this.log = log;
            InitializeComponent();

            UsersTableAdapter adapter = new UsersTableAdapter();
            Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
            adapter.Fill(table);
            users.ItemsSource = table;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUser fm = new AddUser(log);
            fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            fm.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void DataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (users.SelectedItem as DataRowView != null)
            {
                idi = (users.SelectedItem as DataRowView).Row.ItemArray[4].ToString();
               
            }
        }

        private void users_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (users.SelectedItem as DataRowView != null)
            {
                EditUser fm = new EditUser(log, idi);
                fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                fm.Show();
                this.Hide();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
          
            if (users.SelectedItem as DataRowView != null)
            {
               string ConnectBD = "Data Source = LAPTOP-HV0RLJLE;Initial Catalog = Session1_1; Integrated Security = True;";
               SqlConnection conn = new SqlConnection(ConnectBD);
               conn.Open();
               SqlCommand command = new SqlCommand("SELECT * FROM [Users] WHERE [Email] = '" + idi + "'", conn);
               using (SqlDataReader reader = command.ExecuteReader())
               {
                    while (reader.Read())
                    {
                        id_users = reader["ID"].ToString();
                        act = reader["Active"].ToString();
                        FirstName = reader["FirstName"].ToString();
                        Last_Name = reader["LastName"].ToString();
                        passwd = reader["Password"].ToString();
                        ofis = reader["OfficeID"].ToString();
                        birth = reader["Birthdate"].ToString();
                        role = reader["RoleID"].ToString();
                        email = reader["Email"].ToString();
                    }
               }
               if (act == "False")
               {
                    new UsersTableAdapter().UpdateQuery(Convert.ToInt32(role), email, passwd, FirstName, Last_Name, Convert.ToInt32(ofis), birth, Convert.ToBoolean(true), Convert.ToInt32(id_users));
                    UsersTableAdapter adapter = new UsersTableAdapter();
                    Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                    adapter.Fill(table);
                    MessageBox.Show("Пользователь активен!");
               }
               else if (act == "True")
                {
                    new UsersTableAdapter().UpdateQuery(Convert.ToInt32(role), email, passwd, FirstName, Last_Name, Convert.ToInt32(ofis), birth, Convert.ToBoolean(false), Convert.ToInt32(id_users));
                    UsersTableAdapter adapter = new UsersTableAdapter();
                    Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                    adapter.Fill(table);
                    MessageBox.Show("Пользователь заблокирован!");
               }
                
            }
        }

        private void Change_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ch = ((ComboBoxItem)Change.SelectedItem).Content.ToString();        
            if (ch == "All offices")
            {
                UsersTableAdapter adapter = new UsersTableAdapter();
                Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                adapter.Fill(table);
                users.ItemsSource = table;
            }
            else
            {
                UsersTableAdapter adapter1 = new UsersTableAdapter();
                Session1_1DataSet.UsersDataTable table2 = new Session1_1DataSet.UsersDataTable();
                adapter1.FillBy(table2, ch);
                users.ItemsSource = table2;
            }
            
        }
    }
}
