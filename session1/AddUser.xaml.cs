using session1.Session1_1DataSetTableAdapters;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public string log;
        public AddUser(string log)
        {
            this.log = log;
            InitializeComponent();

            OfficesTableAdapter adapt = new OfficesTableAdapter();
            Session1_1DataSet.OfficesDataTable tabl = new Session1_1DataSet.OfficesDataTable();
            adapt.Fill(tabl);
            Change_Office.ItemsSource = tabl;
            Change_Office.DisplayMemberPath = "Title";
            Change_Office.SelectedValuePath = "ID";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Email.Text.Trim() != "" && Password.Text.Trim() != "" && FirstName.Text.Trim() != "" && Last_Name.Text.Trim() != "" && Birthdate.Text.Trim() != "")
                {
                    new UsersTableAdapter().InsertQuery(Convert.ToInt32("2"), Email.Text, Password.Text, FirstName.Text, Last_Name.Text, Convert.ToInt32(Change_Office.SelectedValue), Birthdate.Text, Convert.ToBoolean(true));
                    UsersTableAdapter adapter = new UsersTableAdapter();
                    Session1_1DataSet.UsersDataTable table = new Session1_1DataSet.UsersDataTable();
                    adapter.Fill(table);
                    MessageBox.Show("Данные были успешно добавлены!");
                }
                else { MessageBox.Show("Заполните все поля!"); }
            }
            catch { MessageBox.Show("Проверьте корректность введенных данных!"); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MenuAdmin fm = new MenuAdmin(log);
            fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            fm.Show();
            this.Hide();
        }
    }
}
