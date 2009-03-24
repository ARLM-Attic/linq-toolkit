using System;
using System.ComponentModel;

namespace LinqToolkit.Test {
    public interface ITestItem: INotifyPropertyChanged {
        string TestPropertySimple { get; set; }
    }
}
