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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public string log;
        public string idi;
        public string role;
        public string passwd;
        public string birth;
        public string act;
        public string id_users;
        public EditUser(string log, string idi)
        {
            this.log = log;
            this.idi = idi;
            InitializeComponent();

            OfficesTableAdapter adapt = new OfficesTableAdapter();
            Session1_1DataSet.OfficesDataTable tabl = new Session1_1DataSet.OfficesDataTable();
            adapt.Fill(tabl);
            Change_Office.ItemsSource = tabl;
            Change_Office.DisplayMemberPath = "Title";
            Change_Office.SelectedValuePath = "ID";

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
                    FirstName.Text = reader["FirstName"].ToString();
                    Last_Name.Text = reader["LastName"].ToString();
                    passwd = reader["Password"].ToString();
                    Change_Office.SelectedValue = reader["OfficeID"].ToString();
                    birth = reader["Birthdate"].ToString();
                    if (reader["RoleID"].ToString() == "2")
                    {
                        UserCheck.IsChecked = true;
                    }
                    else AdminCheck.IsChecked = true;

                    Email.Text = reader["Email"].ToString();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuAdmin fm = new MenuAdmin(log);
            fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            fm.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (UserCheck.IsChecked == true)
            {
                new UsersTableAdapter().UpdateQuery(Convert.ToInt32("2"), Email.Text, passwd, FirstName.Text, Last_Name.Text, Convert.ToInt32(Change_Office.SelectedValue), birth, Convert.ToBoolean(true), Convert.ToInt32(id_users));
                UsersTableAdapter adapter = new UsersTableAdapter();
                Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                adapter.Fill(table);
                MessageBox.Show("Данные были успешно изменены!");
            }
            else
            {
                new UsersTableAdapter().UpdateQuery(Convert.ToInt32("1"), Email.Text, passwd, FirstName.Text, Last_Name.Text, Convert.ToInt32(Change_Office.SelectedValue), birth, Convert.ToBoolean(true), Convert.ToInt32(id_users));
                UsersTableAdapter adapter = new UsersTableAdapter();
                Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                adapter.Fill(table);
                MessageBox.Show("Данные были успешно изменены!");
            }
            
        }
    }
}
