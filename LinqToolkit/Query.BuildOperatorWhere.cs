using System;
using System.Linq;
using System.Linq.Expressions;
using LinqToolkit.Properties;

namespace LinqToolkit {

    public partial class Query<TContext, TItem> {
        private bool BuildOperatorWhere( LambdaExpression expression, string methodName ) {
            if ( methodName!="Where" ) {
                return false;
            }
            this.Context.Options.Filter = this.ParseQuery( expression.Body );
            return true;
        }
        private IBaseOperation ParseQuery( Expression expression ) {
            var handlers = new Func<Expression, IBaseOperation>[] {
                this.ParseBinaryExpression,
                this.ParseUnaryExpression,
                this.ParseMethodCallExpression
            };
            IBaseOperation result =
                handlers
                .Select( handler => handler( expression ) )
                .FirstOrDefault( filter => filter!=null );

            if ( result!=null ) {
                return result;
            }
            throw new NotSupportedException(
                string.Format(
                    Resources.ParseQueryNotSupportedFormat,
                    expression.ToString()
                    )
                );
        }
        private IBaseOperation ParseBinaryExpression( Expression expression ) {
            BinaryExpression typedExpression = expression as BinaryExpression;
            if ( typedExpression==null ) {
                return null;
            }
            switch ( typedExpression.NodeType ) {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    return
                        this.Context.CreateJoinOperation(
                            typedExpression.NodeType,
                            this.ParseQuery( typedExpression.Left ),
                            this.ParseQuery( typedExpression.Right )
                            );
                default:
                    return
                        this.ParseConditionExpression( typedExpression );
            }
        }
        private IBaseOperation ParseConditionExpression( BinaryExpression expression ) {
            var leftExpression = expression.Left as MemberExpression;
            if ( leftExpression!=null ) {
                string propertyName = this.GetSourcePropertyName( leftExpression.Member );
                return
                    this.Context.CreateBinaryOperation(
                        expression.NodeType,
                        propertyName,
                        Expression.Lambda( expression.Right ).Compile().DynamicInvoke()
                        );
            }
            throw
                new NotSupportedException(
                    string.Format(
                        Resources.ParseConditionExpressionNotSupportedFormat,
                        expression.ToString()
                        )
                    );
        }
        private IBaseOperation ParseUnaryExpression( Expression expression ) {
            UnaryExpression typedExpression = expression as UnaryExpression;
            if ( typedExpression==null ) {
                return null;
            }
            var operandExpression = typedExpression.Operand as MemberExpression;
            if ( operandExpression==null ) {
                throw
                    new NotSupportedException(
                        string.Format(
                            Resources.ParseUnaryExpressionNotSupportedFormat,
                            expression.ToString()
                            )
                        );
            }
            string propertyName = this.GetSourcePropertyName( operandExpression.Member );
            return
                this.Context.CreateUnaryOperation(
                    typedExpression.NodeType,
                    propertyName
                    );
        }
        private IBaseOperation ParseMethodCallExpression( Expression expression ) {
            MethodCallExpression typedExpression = expression as MethodCallExpression;
            if ( typedExpression==null ) {
                return null;
            }
            MemberExpression memberExpression = typedExpression.Object as MemberExpression;
            if ( memberExpression==null ) {
                return null;
            }
            if ( typedExpression.Arguments.Any( item => !( item is ConstantExpression ) ) ) {
                return null;
            }
            object[] arguments =
                typedExpression.Arguments
                .Cast<ConstantExpression>()
                .Select( argument => argument.Value )
                .ToArray();
            return
                this.Context.CreateMethodCallOperation(
                    typedExpression.Method,
                    this.GetSourcePropertyName( memberExpression.Member ),
                    arguments
                    );
        }
    }
}
