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

namespace VisRed.Info
{
    /// <summary>
    /// Interaction logic for info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public IRedisContextProvider Context { get; set; }

        public Info()
        {
            InitializeComponent();
            Activated += Info_Initialized;

        }

        private void Info_Initialized(object sender, EventArgs e)
        {
            if (infoTabs.ItemsSource == null)
                PopulateModel();
        }

        private void PopulateModel()
        {
            if (Context == null || Context.RedisService == null) return;

            var rs = Context.RedisService.GetServer(Context.RedisService.GetEndPoints().FirstOrDefault());

            var info = rs.Info();
            infoTabs.ItemsSource = info; //.SelectMany(gp=> gp);
        }
    }
}
