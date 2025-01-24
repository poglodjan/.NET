using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    internal class ServerSystem : INotifyingCollection<Server>
    {
        private readonly Dictionary<string, Server> _servers = new();
        public IEnumerator<Server> GetEnumerator()
        {
             return _servers.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
         {
             return GetEnumerator();
         }

        public event EventHandler<CollectionChangedEventArgs<Server>>? ElementAdded;
        public event EventHandler<CollectionChangedEventArgs<Server>>? ElementRemoved;
        public event EventHandler<ElementPropertyChangedEventArgs<Server>>? ElementPropertyChanged;
        protected virtual void OnserverPropertyChanged(object? sender, PropertyChangedEventArgs e)
         {
             if (sender is Server _server)
             {
                ElementPropertyChanged?.Invoke(this, new ElementPropertyChangedEventArgs<Server>(_server, e.PropertyName));
             }
         }

        public bool Add(Server element)
     {
         if (_servers.TryGetValue(element.Address, out var server))
         {
             server.Status = element.Status;
             server.Load = element.Load;
             return false;
         }
         else
         {
             _servers.Add(element.Address, element);
             Console.WriteLine($"Added [{element.Address}]");
             element.Status = Status.Running;   
             element.Load = 23; //random number
             element.PropertyChanged += OnserverPropertyChanged;
             ServerAdded(element);
             return true;
         }
     }

     public bool Remove(Server element)
     {
         if (_servers.TryGetValue(element.Address, out var server))
         {
             server.PropertyChanged -= OnserverPropertyChanged;
             Console.WriteLine($"Removed [{element.Address}]");
             _servers.Remove(element.Address);
             ServerRemoved(server);
             return true;
         }

         return false;
     }

     protected virtual void ServerAdded(Server server)
     {
         ElementAdded?.Invoke(this, new CollectionChangedEventArgs<Server>(server));
     }

    protected virtual void ServerRemoved(Server server)
     {
         ElementRemoved?.Invoke(this, new CollectionChangedEventArgs<Server>(server));
     }

}
}
