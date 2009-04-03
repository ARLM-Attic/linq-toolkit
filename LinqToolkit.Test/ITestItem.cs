using System;
using System.ComponentModel;

namespace LinqToolkit.Test {
    [Source("TestItem")]
    public interface ITestItem: INotifyPropertyChanged {
        string TestPropertySimple { get; set; }
    }
}
