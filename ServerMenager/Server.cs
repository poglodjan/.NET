using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{ 
    public enum Status
    {
        Failed,
        OverLoaded,
        Running,
        Stopped
    }

    public class Server : IAddressable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Status _status;
        private string _name;
        private double _load;
        public string Address { get; }

        public Server(string address, string name = "", Status status = Status.Stopped, double load = 0.0)
        {
            Address = address;
            _name = name;
            _status = status;
            _load = load;
        }

        public override string ToString()
        {
            return $"{Name} [{Address}]";
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public double Load
        {
            get => _load;
            set
            {
                if (_load != value)
                {
                    _load = value;
                    OnPropertyChanged();
                }
            }
        }
        public Status Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetPropertyValue(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return Name;
                case nameof(Status): return Status.ToString();
                case nameof(Load): return Load.ToString();
                default: return string.Empty;
            }
        }

    }
}
