using System;
using System.Collections.Generic;

namespace LinqToolkit.Test {

    public class TestQuery<TItem>: Query<TestContextBase, TItem>, ITestQuery {

        TestContextBase ITestQuery.Context { get { return base.Context; } }

        public TestQuery( TestContextBase context ) : base( context ) { }

        protected override Query<TestContextBase, T> Copy<T>() {
            return new TestQuery<T>( this.Context );
        }
        protected override IEnumerable<object> EnumerateQuery() {
            return new[] { new TestItem() };
        }
        protected override TResult ExecuteQuery<TResult>( string commandName ) {
            return (TResult)Convert.ChangeType( commandName, typeof( TResult ) );
        }
    }
}
