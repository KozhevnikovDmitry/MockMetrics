using System;
using System.ComponentModel;

namespace PostGrad.Core.DomainModel.MzOrder
{
    public interface IOrder : IPersistentObject, INotifyPropertyChanged
    {
        int Id { get; set; }
        DateTime Stamp { get; set; }
        string RegNumber { get; set; }
        string Name { get; }
    }
}