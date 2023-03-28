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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace session1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            TimerSec.Interval = TimeSpan.FromSeconds(1);
            TimerSec.Tick += Reklama_Tick;
        }

        DispatcherTimer TimerSec = new DispatcherTimer();
        int attempt = 0;
        int seconds = 10;

        private void Reklama_Tick(object sender, EventArgs e)
        {

            if (seconds == 0)
            {
                seconds = 10;
                login_btn.IsEnabled = true;
                lblTime.Visibility = Visibility.Hidden;
                TimerSec.Stop();
            }
            lblTime.Content = seconds;
            seconds--;

        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            attempt += 1;
            if (attempt <= 3)
            {
                if (UserName.Text.Trim() != "" && Password.Password.ToString() != "")
                {

                    string ConnectBD = "Data Source = LAPTOP-HV0RLJLE;Initial Catalog = Session1_1; Integrated Security = True;";
                    SqlConnection conn = new SqlConnection(ConnectBD);
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM [Users] WHERE [Email] = '" + UserName.Text + "' AND [Password] = '" + Password.Password.ToString() + "'", conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Пользователь с таким логином и паролем не найден!. Удостоверьтесь в корректности введенных данных.", "Оповещение системы");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                if (reader["Active"].ToString() == "True")
                                {
                                    if (reader["RoleID"].ToString() == "1")
                                    {
                                        MenuAdmin fm = new MenuAdmin(UserName.Text);
                                        fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                                        fm.Show();
                                        this.Hide();
                                    }
                                    if (reader["RoleID"].ToString() == "2")
                                    {
                                        string id = reader["ID"].ToString();
                                        string b = "";
                                        new TrackingTableAdapter().InsertQuery(Convert.ToInt32(id), DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                                        MenuUser fm = new MenuUser(UserName.Text);
                                        fm.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                                        fm.Show();
                                        this.Hide();
                                    }
                                }
                                else MessageBox.Show("Вы были заблокированы!");
                                    
                            }
                        }
   
                    }

                }
                else MessageBox.Show("Пожалуйста, заполните все поля!");
            }
            else 
            {
                MessageBox.Show("Подождите 10 секунд до следующей попытки!");
                attempt = 0;
                lblTime.Visibility = Visibility.Visible;
                login_btn.IsEnabled = false;
                TimerSec.Start();
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
