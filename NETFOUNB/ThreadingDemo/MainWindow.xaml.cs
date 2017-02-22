using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ThreadingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        object _lock = new object();
        private int i;

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            int i = await FunctieAsync();
        }

        private async Task<int> FunctieAsync()
        {
            await Task.Run(() => DureBerekening());
            label.Content = "Thread is klaar";

            return 3;
        }

        private void Methode(int i)
        {
            Monitor.Enter(_lock);

            lock(_lock)
            {


            }

            Monitor.Exit(_lock);
        }

        private void DureBerekening()
        {
            lock (_lock)
            {
                i++;
            }

            Thread.Sleep(5000);
        }
    }
}
