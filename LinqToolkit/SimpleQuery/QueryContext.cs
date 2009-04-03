using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// Implements <see cref="IQueryContext"/> interface and contains concrete query context.
    /// Contains instance of <see cref="SimpleQueryOptions"/> class.
    /// </summary>
    public class QueryContext: IQueryContext {

        public QueryOptions Options { get; private set; }

        public QueryContext() {
            this.Options =new QueryOptions();
        }

        #region IQueryContext Members
        IQueryOptions IQueryContext.Options {
            get { return this.Options; }
        }
        IJoinOperation IQueryContext.CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            return
                new JoinOperation() {
                    Type = type,
                    Left = (BaseOperation)left,
                    Right = (BaseOperation)right
                };
        }
        IBaseOperation IQueryContext.CreateUnaryOperation( ExpressionType type, string propertyName ) {
            return
                new UnaryOperation() {
                    Type = type,
                    PropertyName = propertyName
                };
        }
        IBaseOperation IQueryContext.CreateBinaryOperation( ExpressionType type, string propertyName, object value ) {
            return
                new BinaryOperation() {
                    Type = type,
                    PropertyName = propertyName,
                    Value = value
                };
        }
        IBaseOperation IQueryContext.CreateCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            return
                new CallOperation() {
                    MethodName = method.Name,
                    PropertyName = propertyName,
                    Arguments = arguments 
                };
        }
        bool IQueryContext.BuildOperator( string operatorName ) {
            this.Options.Operators.Add(
                new QueryOperator() {
                    OperatorName = operatorName,
                    PropertyName = null,
                    Value = null
                }
                );
            return true;
        }
        bool IQueryContext.BuildOperator( string operatorName, string propertyName ) {
            this.Options.Operators.Add(
                new QueryOperator() {
                    OperatorName = operatorName,
                    PropertyName = propertyName,
                    Value = null
                }
                );
            return true;
        }
        bool IQueryContext.BuildOperator( string operatorName, object value ) {
            this.Options.Operators.Add(
                new QueryOperator() {
                    OperatorName = operatorName,
                    PropertyName = null,
                    Value = value
                }
                );
            return true;
        }
        #endregion
    }
}
