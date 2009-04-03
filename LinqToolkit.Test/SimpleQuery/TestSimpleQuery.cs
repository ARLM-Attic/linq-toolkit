using System;
using System.Collections.Generic;

namespace LinqToolkit.Test.SimpleQuery {
    using LinqToolkit.SimpleQuery;

    public class TestSimpleQuery<TItem>: Query<QueryContext, TItem> {

        public QueryOptions Options {
            get { return base.Context.Options; }
        }

        public TestSimpleQuery() : base( new QueryContext() ) { }
        public TestSimpleQuery( QueryContext context ) : base( context ) { }

        protected override Query<QueryContext, T> Copy<T>() {
            return new TestSimpleQuery<T>( this.Context );
        }

        protected override IEnumerable<object> EnumerateQuery() {
            throw new NotImplementedException();
        }

        protected override TResult ExecuteQuery<TResult>( string commandName ) {
            throw new NotImplementedException();
        }
    }
}
