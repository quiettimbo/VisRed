using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisRed.Utils;

namespace VisRed
{
    public class RedisModel : INotifyPropertyChanged
    {
        private ObservableCollection<RedisServer> _servers;

        public ObservableCollection<RedisServer> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
                RaisePropertyChanged("Servers");
            }
        }

        public ObservableDictionary<string, RedisVal> _entries;

        public ObservableDictionary<string, RedisVal> Entries
        {
            get
            {
                return _entries;
            }
            set
            {
                _entries = value;
                RaisePropertyChanged("Entries");
            }
        }

        public RedisModel()
        {
            Servers = new ObservableCollection<RedisServer>();
            Entries = new ObservableDictionary<string, RedisVal>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
