using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.Test {
    public abstract class TestContextBase: IQueryContext {

        public IQueryOptions Options { get; private set; }
        public TestOperator Operator { get; protected set; }

        public TestContextBase() {
            this.Options = new TestOptions();
        }

        public abstract IJoinOperation CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right );
        public abstract IBaseOperation CreateUnaryOperation( ExpressionType type, string propertyName );
        public abstract IBaseOperation CreateBinaryOperation( ExpressionType type, string propertyName, object value );
        public abstract IBaseOperation CreateMethodCallOperation( MethodInfo method, string propertyName, object[] arguments );
        public abstract bool BuildOperator( string operatorName );
        public abstract bool BuildOperator( string operatorName, string propertyName );
        public abstract bool BuildOperator( string operatorName, object value );
    }
}
