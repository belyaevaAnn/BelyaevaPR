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
using System.Data.SqlClient;
using System.Data;

namespace belyaeva_pr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        MySqlCommand com = new MySqlCommand();
        public static int user_id;
        public static bool privileg;
        string sql;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                conn.Open();
                com.Connection = conn;
                com.Parameters.Add("@analyse", MySqlDbType.Double);
                com.Parameters.Add("@setup", MySqlDbType.Double);
                com.Parameters.Add("@service", MySqlDbType.Double);
                com.Parameters.Add("@time", MySqlDbType.Double);
                com.Parameters.Add("@difficult", MySqlDbType.Double);
                com.Parameters.Add("@money", MySqlDbType.Double);
                com.Parameters.Add("@id", MySqlDbType.Int32);
            }
            catch
            {
                MessageBox.Show("Невозможно подключиться к базе");
            }
        }

        private void enterS(object sender, RoutedEventArgs e)
        {
            try
            {
                sql = $"select * from user where login = '{login_enter.Text}' and password = '{password_enter.Password.ToString()}'";
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
                            privileg = reader.GetBoolean(4);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль неверны");
                    }
                }

                if (privileg == true)
                {
                    add_task.IsEnabled = true;
                    coef_button.IsEnabled = true;
                    userFilter.IsEnabled = true;
                    userFilter.Items.Add("Без фильтра");
                    userFilter.Text = "Без фильтра";
                    sql = $"select name from user where Id in (select idPerf from subordinates where idMen = {user_id})";
                    com.CommandText = sql;
                    using (DbDataReader reader = com.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    userFilter.Items.Add(reader.GetString(i));
                                }
                            }
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
                    update_table($"SELECT task.id, user.name, title, complexity, status, natureWork, timeTask FROM task, user WHERE userID in (SELECT idPerf from subordinates where idMen = {user_id}) and task.userID = user.Id");
                }
                else
                {
                    add_task.IsEnabled = false;
                    coef_button.IsEnabled = false;
                    userFilter.IsEnabled = false;
                    update_table($"SELECT task.id, user.name, title, complexity, status, natureWork, timeTask FROM task, user WHERE userID = {user_id} and task.userID = user.Id");
                }
            }
            catch (Exception ex)
            {
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

        private void coef_button_Click(object sender, RoutedEventArgs e)
        {
            meneger.Visibility = Visibility.Hidden;
            coefic.Visibility = Visibility.Visible;
        }
        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            coefic.Visibility = Visibility.Hidden;
            meneger.Visibility = Visibility.Visible;
        }

        public void update_table (string sql)
        {
            task_table.ItemsSource = null;
            MySqlCommand com = new MySqlCommand();
            com.Connection = conn;
            com.CommandText = sql;
            using (DbDataReader readDB = com.ExecuteReader())
            {
                List<task_class> task_list = new List<task_class>();
                if (readDB.HasRows)
                {
                    while (readDB.Read())
                    {
                        for(int i=0; i<readDB.FieldCount; i+=7)
                        {
                            task_list.Add(new task_class() { id = readDB.GetInt32(i), id_user = readDB.GetString(i+1), name = readDB.GetString(i + 2), difficulty = readDB.GetInt32(i + 3), status = readDB.GetString(i + 4), natWork = readDB.GetString(i + 5), timeWork = readDB.GetInt32(i + 6) });
                        }
                    }
                    task_table.ItemsSource = task_list;
                }
            }
        }

        private void add_task_Click(object sender, RoutedEventArgs e)
        {
            Window1 addWind = new Window1();
            addWind.Show();
        }

        private void update_task_Click(object sender, RoutedEventArgs e)
        {
            task_class path = task_table.SelectedItem as task_class;
            UpdateTaskWin updateWin = new UpdateTaskWin(path.id, path.id_user, path.name, path.difficulty, path.status, path.natWork, path.timeWork); 
            updateWin.Show();
        }

        private void delete_task_Click(object sender, RoutedEventArgs e)
        {
            task_class path = task_table.SelectedItem as task_class;
            com.Parameters["@id"].Value = path.id;
            sql = $"delete from task where Id = @id";
            com.CommandText = sql;
            int doThing = com.ExecuteNonQuery();
            MessageBox.Show("Данные успешно удалены");
        }

        private void task_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            task_class tc = task_table.SelectedItem as task_class;
            if (tc != null && (tc.status == "Завершено" || tc.status == "Отменено"))
            {
                update_task.IsEnabled = false;
            }
            else
            {
                update_task.IsEnabled = true;
            }
        }

        private void update_table_button_Click(object sender, RoutedEventArgs e)
        {
            if (privileg)
            {
                if (userFilter.Text == "Без фильтра")
                {
                    update_table($"SELECT task.id, user.name, title, complexity, status, natureWork, timeTask FROM task, user WHERE userID in (SELECT idPerf from subordinates where idMen = {user_id}) and task.userID = user.Id");
                }
                else
                {
                    update_table($"SELECT task.id, user.name, title, complexity, status, natureWork, timeTask FROM task, user WHERE userID in (SELECT idPerf from subordinates where idMen = {user_id}) and task.userID = user.Id and user.name = '{userFilter.Text}'");
                }
            }
            else
            {
                update_table($"SELECT task.id, user.name, title, complexity, status, natureWork, timeTask FROM task, user WHERE userID = {user_id} and task.userID = user.Id");
            }
        }
    }

    public class task_class
    {
        public int id { get; set; }
        public string id_user { get; set; }
        public string name { get; set; }
        public int difficulty { get; set; }
        public string status { get; set; }
        public string natWork { get; set; }
        public int timeWork { get; set; }
    }
}