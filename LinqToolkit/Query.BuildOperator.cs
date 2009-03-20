using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToolkit {

    public partial class Query<TContext, TItem> {

        private bool BuildOperator( MethodCallExpression call ) {
            switch ( call.Arguments.Count ) {
                case 1:
                    return this.Context.BuildOperator( call.Method.Name );
                case 2:
                    var handlers = new Func<Expression, string, bool>[] {
                        this.BuildOperatorWithConstantArgument,
                        this.BuildOperatorWithLambdaArgument,
                        };
                    return handlers.Any( handler => handler( call.Arguments[1], call.Method.Name ) );
                default:
                    return false;
            }
        }
        private bool BuildOperatorWithConstantArgument( Expression expression, string methodName ) {
            var argument = expression as ConstantExpression;
            if ( argument==null ) {
                return false;
            }
            return this.Context.BuildOperator( methodName, argument.Value );
        }
        private bool BuildOperatorWithLambdaArgument( Expression expression, string methodName ) {
            UnaryExpression argument = expression as UnaryExpression;
            if ( argument==null ) {
                return false;
            }
            var operand = argument.Operand as LambdaExpression;
            if ( operand==null ) {
                return false;
            }
            var handlers = new Func<LambdaExpression, string, bool>[] {
                        this.BuildOperatorSelect,
                        this.BuildOperatorWhere,
                        this.BuildOperatorWithMemberArgument
                        };
            return handlers.Any( handler => handler( operand, methodName ) );
        }
        private bool BuildOperatorWithMemberArgument( LambdaExpression expression, string methodName ) {
            var argument = expression.Body as MemberExpression;
            if ( argument==null ) {
                return false;
            }
            string propertyName = this.GetFilterPropertyName( argument.Member );
            return this.Context.BuildOperator( methodName, propertyName );
        }
    }
}