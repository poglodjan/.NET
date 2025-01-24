using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
     public interface INotifyingCollection<T> : IEnumerable<T>
     where T : IAddressable,INotifyPropertyChanged
    {
         event EventHandler<CollectionChangedEventArgs<T>>? ElementAdded;
         event EventHandler<CollectionChangedEventArgs<T>>? ElementRemoved;
         event EventHandler<ElementPropertyChangedEventArgs<T>>? ElementPropertyChanged;

         bool Add(T element);
         bool Remove(T element);
     }

     public sealed class ElementPropertyChangedEventArgs<T>(T element, string? propertyName) : EventArgs
     {
         public T Element { get; } = element;
         public string? PropertyName { get; } = propertyName;
     }

     public sealed class CollectionChangedEventArgs<T>(T element) : EventArgs
     {
         public T Element { get; } = element;
     }
    }
