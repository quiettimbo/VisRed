using StackExchange.Redis;
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

namespace VisRed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RedisModel Model { get; set; }
        public ConnectionMultiplexer RedisService { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Model = new RedisModel();
            Model.Servers.Add(new RedisServer() { Url = "whcinnsamres01q.corp.web" });
            Model.Servers.Add(new RedisServer() { Url = "whcinnsamres01s.corp.web" });
            comboBox.ItemsSource = Model.Servers;
            listView.ItemsSource = Model.Entries;

            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(e.AddedItems.Cast<RedisServer>().FirstOrDefault().Url);
        }

        private void Refresh(string Url)
        {
            RedisService = ConnectionMultiplexer.Connect(Url);

            var rs = RedisService.GetServer(RedisService.GetEndPoints().FirstOrDefault());
            Model.Entries.Clear();
            var db = RedisService.GetDatabase();
            Model.Entries.AddRange(rs.Keys().ToDictionary(k => k.ToString(), k => RedisServer.RedisFactory(db, k)));

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Refresh(comboBox.Items.Cast<RedisServer>().FirstOrDefault().Url);
        }
    }
}
