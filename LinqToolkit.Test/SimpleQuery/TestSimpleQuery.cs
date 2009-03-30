using System;
using System.Collections.Generic;

namespace LinqToolkit.Test.SimpleQuery {
    using LinqToolkit.SimpleQuery;

    public class TestSimpleQuery<TItem>: Query<SimpleQueryContext, TItem> {

        public SimpleQueryOptions Options {
            get { return base.Context.Options; }
        }

        public TestSimpleQuery() : base( new SimpleQueryContext() ) { }
        public TestSimpleQuery( SimpleQueryContext context ) : base( context ) { }

        protected override Query<SimpleQueryContext, T> Copy<T>() {
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
