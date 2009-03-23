using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.Test {
    public class TestContext: IQueryContext {

        public IQueryOptions Options { get; private set; }
        public TestOperator Operator { get; private set; }

        public TestContext() {
            this.Options = new TestOptions();
        }

        public IJoinOperation CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            return new TestJoinOperation( type, left, right );
        }

        public IBaseOperation CreateUnaryOperation( ExpressionType type, string propertyName ) {
            return new TestUnaryOperation( type, propertyName );
        }

        public IBaseOperation CreateBinaryOperation( ExpressionType type, string propertyName, object value ) {
            return new TestBinaryOperation( type, propertyName, value );
        }

        public IBaseOperation CreateMethodCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            return new TestMethodCallOperation( method, propertyName, arguments );
        }

        public bool BuildOperator( string operatorName ) {
            this.Operator = new TestOperator( operatorName );
            return true;
        }

        public bool BuildOperator( string operatorName, string propertyName ) {
            this.Operator = new TestOperator( operatorName, propertyName );
            return true;
        }

        public bool BuildOperator( string operatorName, object value ) {
            this.Operator = new TestOperator( operatorName, value );
            return true;
        }
    }
}
