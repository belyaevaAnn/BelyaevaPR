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
using System.Windows.Navigation;
using System.Windows.Shapes;
using belyaeva_pr.sqlConn;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace belyaeva_pr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        MySqlCommand com = new MySqlCommand();
        int user_id;
        string sql;

        public MainWindow()
        {
            InitializeComponent();
            com.Connection = conn;
            com.Parameters.Add("@analyse", MySqlDbType.Double);
            com.Parameters.Add("@setup", MySqlDbType.Double);
            com.Parameters.Add("@service", MySqlDbType.Double);
            com.Parameters.Add("@time", MySqlDbType.Double);
            com.Parameters.Add("@difficult", MySqlDbType.Double);
            com.Parameters.Add("@money", MySqlDbType.Double);
        }

        private void enterS(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                sql = $"select * from user where login = '{login_enter.Text}' and password = '{password_enter.Password.ToString()}'";
                com.Connection = conn;
                com.CommandText = sql;
                using (DbDataReader reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        startScreen.Visibility = Visibility.Hidden;
                        meneger.Visibility = Visibility.Visible;
                        while (reader.Read())
                        {
                            user_id = reader.GetInt32(0);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверные логин или пароль");
                    }
                }

                sql = $"select * from coefficient where Id = {user_id}";
                com.CommandText = sql;
                using (DbDataReader reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            analyse_C.Text = reader[1].ToString();
                            setup_C.Text = reader[2].ToString();
                            service_C.Text = reader[3].ToString();
                            time_C.Text = reader[4].ToString();
                            difficult_C.Text = reader[5].ToString();
                            money_C.Text = reader[6].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Подключение к базе отсутствует");
                MessageBox.Show(ex.Message);
            }
        }

        private void send_coef(object sender, RoutedEventArgs e)
        {
            try
            {
                com.Parameters["@analyse"].Value = analyse_C.Text;
                com.Parameters["@setup"].Value = setup_C.Text;
                com.Parameters["@service"].Value = service_C.Text;
                com.Parameters["@time"].Value = time_C.Text;
                com.Parameters["@difficult"].Value = difficult_C.Text;
                com.Parameters["@money"].Value = money_C.Text;
                sql = $"update coefficient set coefAnalysing = @analyse, " +
                    $"coefInstall = @setup, " +
                    $"coefService = @service, " +
                    $"coefTime = @time, " +
                    $"coefDifficulty = @difficult, " +
                    $"coefMoney = @money " +
                    $"where Id = {user_id}";
                com.CommandText = sql;
                int writeCom = com.ExecuteNonQuery();
                MessageBox.Show("Обновлено");
            }
            catch
            {
                MessageBox.Show("Ошибка обновления данных");
            }
        }
    }
}
