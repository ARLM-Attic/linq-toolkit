using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.Test.Query {
    public class TestContext: TestContextBase {

        public override IJoinOperation CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            return new TestJoinOperation( type, left, right );
        }
        public override IBaseOperation CreateUnaryOperation( ExpressionType type, string propertyName ) {
            return new TestUnaryOperation( type, propertyName );
        }
        public override IBaseOperation CreateBinaryOperation( ExpressionType type, string propertyName, object value ) {
            return new TestBinaryOperation( type, propertyName, value );
        }
        public override IBaseOperation CreateCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            return new TestCallOperation( method, propertyName, arguments );
        }
        public override bool BuildOperator( string operatorName ) {
            this.Operator = new TestOperator( operatorName );
            return true;
        }
        public override bool BuildOperator( string operatorName, string propertyName ) {
            this.Operator = new TestOperator( operatorName, propertyName );
            return true;
        }
        public override bool BuildOperator( string operatorName, object value ) {
            this.Operator = new TestOperator( operatorName, value );
            return true;
        }
    }
}
