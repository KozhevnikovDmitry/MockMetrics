using System;
using System.ComponentModel;
using Common.DA.Interface;

namespace GU.MZ.DataModel.MzOrder
{
    public interface IOrder : IPersistentObject, INotifyPropertyChanged
    {
        int Id { get; set; }
        DateTime Stamp { get; set; }
        string RegNumber { get; set; }
        string Name { get; }
    }
}