using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqToolkit.Test.Table {
    public class TestTable: Table<ITestItem> {
        public TestTable(): base( null ) {}
        public TestTable( IEnumerable<TestItem> items )
            : base( items.Cast<ITestItem>() ) { }

        public override void SubmitChanges() {
        }
    }
}
