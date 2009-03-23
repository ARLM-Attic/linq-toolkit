using System;
using System.Diagnostics.CodeAnalysis;

namespace LinqToolkit.Test {
    public class TestItem: ITestItem {
        public bool TestField;
        public string TestPropertySimple { get; set; }
        [SourceProperty("TestSourceProperty")]
        public int TestPropertyWithSourceProperty { get; set; }
        [Ignore]
        public string TestPropertyWithIgnore { get; set; }
    }
}
