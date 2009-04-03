using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// Implements <see cref="IQueryContext"/> interface and contains concrete query context.
    /// Contains instance of <see cref="SimpleQueryOptions"/> class.
    /// </summary>
    public class SimpleQueryContext: IQueryContext {

        public SimpleQueryOptions Options { get; private set; }

        public SimpleQueryContext() {
            this.Options =new SimpleQueryOptions();
        }

        #region IQueryContext Members
        IQueryOptions IQueryContext.Options {
            get { return this.Options; }
        }
        IJoinOperation IQueryContext.CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            return
                new SimpleQueryJoinOperation() {
                    Type = type,
                    Left = (SimpleQueryBaseOperation)left,
                    Right = (SimpleQueryBaseOperation)right
                };
        }
        IBaseOperation IQueryContext.CreateUnaryOperation( ExpressionType type, string propertyName ) {
            return
                new SimpleQueryUnaryOperation() {
                    Type = type,
                    PropertyName = propertyName
                };
        }
        IBaseOperation IQueryContext.CreateBinaryOperation( ExpressionType type, string propertyName, object value ) {
            return
                new SimpleQueryBinaryOperation() {
                    Type = type,
                    PropertyName = propertyName,
                    Value = value
                };
        }
        IBaseOperation IQueryContext.CreateCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            return
                new SimpleQueryCallOperation() {
                    MethodName = method.Name,
                    PropertyName = propertyName,
                    Arguments = arguments 
                };
        }
        bool IQueryContext.BuildOperator( string operatorName ) {
            this.Options.Operators.Add(
                new SimpleQueryOperator() {
                    OperatorName = operatorName,
                    PropertyName = null,
                    Value = null
                }
                );
            return true;
        }
        bool IQueryContext.BuildOperator( string operatorName, string propertyName ) {
            this.Options.Operators.Add(
                new SimpleQueryOperator() {
                    OperatorName = operatorName,
                    PropertyName = propertyName,
                    Value = null
                }
                );
            return true;
        }
        bool IQueryContext.BuildOperator( string operatorName, object value ) {
            this.Options.Operators.Add(
                new SimpleQueryOperator() {
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
