using System;
using System.Collections.Generic;

namespace LinqToolkit.Test {

    public class TestQuery<TItem>: Query<TestContext, TItem>, ITestQuery {

        TestContext ITestQuery.Context { get { return base.Context; } }

        public TestQuery() : base( new TestContext() ) { }

        protected override Query<TestContext, T> Copy<T>() {
            return new TestQuery<T>();
        }
        protected override IEnumerable<object> ExecuteRequest() {
            throw new NotImplementedException();
        }
    }
}
