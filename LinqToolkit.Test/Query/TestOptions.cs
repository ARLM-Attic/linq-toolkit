using System;
using System.Collections.Generic;

namespace LinqToolkit.Test.Query {
    public class TestOptions: IQueryOptions {
        public IBaseOperation Filter { get; set; }
        public HashSet<string> PropertiesToRead { get; private set; }
        public TestOptions() {
            this.PropertiesToRead = new HashSet<string>();
        }
    }
}
