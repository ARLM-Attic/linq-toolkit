using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToolkit {

    public partial class Query<TContext, TEntity> {
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
            throw new NotSupportedException( "Unsupported query expression detected: " + expression.ToString() );
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
                string propertyName = this.GetFilterPropertyName( leftExpression.Member );
                return
                    this.Context.CreateBinaryOperation(
                        expression.NodeType,
                        propertyName,
                        Expression.Lambda( expression.Right ).Compile().DynamicInvoke()
                        );
            }
            throw new NotSupportedException( "A filtering expression should contain an entity member as the left operand: " + expression.ToString() );
        }
        private IBaseOperation ParseUnaryExpression( Expression expression ) {
            UnaryExpression typedExpression = expression as UnaryExpression;
            if ( typedExpression==null ) {
                return null;
            }
            return
                this.Context.CreateUnaryOperation(
                    typedExpression.NodeType,
                    this.ParseQuery( typedExpression.Operand )
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
                this.Context.CreateCallOperation(
                    typedExpression.Method,
                    this.GetFilterPropertyName( memberExpression.Member ),
                    arguments
                    );
        }
    }
}
