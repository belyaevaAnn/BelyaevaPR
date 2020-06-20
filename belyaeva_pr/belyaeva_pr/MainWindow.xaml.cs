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

namespace belyaeva_pr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void enterS(object sender, RoutedEventArgs e)
        {
            startScreen.Visibility = Visibility.Hidden;
            meneger.Visibility = Visibility.Visible;
            try
            {
                conn.Open();
                header.Text = "Да";
            }
            catch (Exception exc)
            {
                header.Text = "нет";
            }
        }
        
    }
}
