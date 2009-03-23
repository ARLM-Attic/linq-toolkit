using System;

namespace LinqToolkit.Test {
    public class TestItemNew: ITestItem {
        public string TestPropertySimple { get; set; }
        public TestItemNew() {}
        public TestItemNew( string testPropertySimple ) {
            this.TestPropertySimple = testPropertySimple;
        }
    }
}
