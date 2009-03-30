using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.Test.Query {
    public class TestContextEmpty: TestContextBase {

        public override IJoinOperation CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            return null;
        }
        public override IBaseOperation CreateUnaryOperation( ExpressionType type, string propertyName ) {
            return null;
        }
        public override IBaseOperation CreateBinaryOperation( ExpressionType type, string propertyName, object value ) {
            return null;
        }
        public override IBaseOperation CreateCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            return null;
        }
        public override bool BuildOperator( string operatorName ) {
            return false;
        }
        public override bool BuildOperator( string operatorName, string propertyName ) {
            return false;
        }
        public override bool BuildOperator( string operatorName, object value ) {
            return false;
        }
    }
}
