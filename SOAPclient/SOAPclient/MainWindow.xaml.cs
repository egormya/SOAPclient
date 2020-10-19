using System;
using System.Windows;
using WpfApp1.Controller;

namespace SOAPclient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CBRService cbr = new CBRService();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = cbr;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //textBox1.Text = Curs.ExtractCurrencyRate();
            cbr.AsyncGetCurrencyRateOnDate(DateTime.Now, "USD");
        }
    }
}
