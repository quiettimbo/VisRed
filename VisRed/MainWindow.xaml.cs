using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            foreach (var item in ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .Where(cs => cs.ProviderName.Equals("Redis",StringComparison.CurrentCultureIgnoreCase )))
            {
                Model.Servers.Add(new RedisServer() { Url = item.ConnectionString, Name = item.Name });
            }
            comboBox.ItemsSource = Model.Servers;
            listView.ItemsSource = Model.Entries;

            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(e.AddedItems.Cast<RedisServer>().FirstOrDefault().Url);
        }

        private async Task Refresh(string url = null)
        {
            if (RedisService == null || 
                (url != null && !RedisService.Configuration.StartsWith(url)))
            {
                if (url == null)
                {
                    var selected = comboBox.SelectedItem as RedisServer;
                    if (selected == null)
                    {
                        return;
                    }
                    url = selected.Url;
                }
                RedisService = ConnectionMultiplexer.Connect(url);
            }

            var rs = RedisService.GetServer(RedisService.GetEndPoints().FirstOrDefault());
            Model.Entries.Clear();
            var db = RedisService.GetDatabase();
            var seq = rs.Keys(db.Database, searchBox.Text);
            foreach (var key in seq)
            {
                Model.Entries.Add(key, RedisVal.ValueFactory(db, key));
            }
            // Really want to do this async and just get enough to fill the page
            //IScanningCursor cursor = rs.Keys(db.Database, searchBox.Text, 10) as IScanningCursor;
            //do
            //{
            //    Model.Entries.AddRange(cursor.ToDictionary(k => k.ToString(), k => RedisServer.RedisFactory(db, k)));
            //    var sc = cursor as IScanningCursor;
            //    if (sc?.Cursor == 0) break;
            //    cursor = rs.Keys(db.Database, searchBox.Text, 10, sc.Cursor);
            //} while (true);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        private async void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await Refresh();
            }
        }

        private async void testgeneratebutton_Click(object sender, RoutedEventArgs e)
        {
            // want a two state so that we can delete 
            if (RedisService != null && RedisService.IsConnected)
            {
                var db = RedisService.GetDatabase();
                Task[] tarr = new Task[150];

                tarr[1] = db.StringSetAsync("Visred.Test.Key" ,"simple value");
                db.HashSet("Visred.Test.Hash", new HashEntry[] { new HashEntry( "hash1", "value1"),
                    new HashEntry("hash2", "value2"),
                    new HashEntry("hash3", "value3")});
                tarr[2] = db.SetAddAsync("Visred.Test.Set", new RedisValue[]
                {
                    "setvalue1", "setvalue2", "setvalue3"
                });
                tarr[3] = db.SortedSetAddAsync("Visred.Test.SortedSet", new SortedSetEntry[]
                {
                    new SortedSetEntry("setvalue1", 1),
                    new SortedSetEntry("setvalue2", 3),
                    new SortedSetEntry("setvalue3", 2)
                });
                tarr[0] = db.ListRightPushAsync("Visred.Test.List", new RedisValue[] { "Value1", "Value2" });

                for (int ii = 4; ii < tarr.Length; ii++)
                {
                    tarr[ii] = db.StringSetAsync($"Visred.Test.{ii}", $"Simple value {ii}");
                }

                await Task.WhenAll( tarr);
            }
        }
    }
}
