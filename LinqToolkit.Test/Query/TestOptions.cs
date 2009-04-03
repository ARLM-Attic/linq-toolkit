using System;
using System.Collections.Generic;

namespace LinqToolkit.Test.Query {
    public class TestOptions: IQueryOptions {
        public string Source { get; set; }
        public IBaseOperation Filter { get; set; }
        public HashSet<string> PropertiesToRead { get; private set; }
        public TestOptions() {
            this.PropertiesToRead = new HashSet<string>();
        }
    }
}
