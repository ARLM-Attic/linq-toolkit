using System;
using System.ComponentModel;

namespace LinqToolkit.Test {
    public class TestItemNew: ITestItem {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TestPropertySimple { get; set; }
        public TestItemNew() {}
        public TestItemNew( string testPropertySimple ) {
            this.TestPropertySimple = testPropertySimple;
        }
    }
}
