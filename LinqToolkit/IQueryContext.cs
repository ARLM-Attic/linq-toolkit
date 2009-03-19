using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit {
    /// <summary>
    /// Defines an interface for query context.
    /// </summary>
    public interface IQueryContext {
        /// <summary>
        /// Gets <see cref="IQueryOptions"/> interface to provide an access to query options.
        /// </summary>
        IQueryOptions Options { get; }
        /// <summary>Creates join operation.</summary>
        /// <param name="type">Type of operation to create</param>
        /// <param name="left">Left operand to join</param>
        /// <param name="right">Right operand to join</param>
        /// <returns>Created join operation</returns>
        IJoinOperation CreateJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right );
        /// <summary>Creates unary operation.</summary>
        /// <param name="type">Type of operation to create</param>
        /// <param name="left">Operand for unary operation</param>
        /// <returns>Created unary operation</returns>
        IBaseOperation CreateUnaryOperation( ExpressionType type, IBaseOperation operand );
        /// <summary>Creates binary operation.</summary>
        /// <param name="type">Type of operation to create</param>
        /// <param name="propertyName">Left operand property name</param>
        /// <param name="value">Right operand value</param>
        /// <returns>Created binary operation</returns>
        IBaseOperation CreateBinaryOperation( ExpressionType type, string propertyName, object value );
        /// <summary>Creates method call operation.</summary>
        /// <param name="method">Method to call for operation</param>
        /// <param name="propertyName">Property name where method is defined</param>
        /// <param name="arguments">Arguments for method call</param>
        /// <returns>Created method call operation</returns>
        IBaseOperation CreateCallOperation( MethodInfo method, string propertyName, object[] arguments );
        /// <summary>Builds an operator without parameters</summary>
        /// <param name="operatorName">Operator name to create</param>
        /// <returns>True if operator created, otherwise False</returns>
        bool BuildOperator( string operatorName );
        /// <summary>Builds an operator with property as a parameter</summary>
        /// <param name="operatorName">Operator name to create</param>
        /// <param name="propertyName">Property name to pass as a parameter</param>
        /// <returns>True if operator created, otherwise False</returns>
        bool BuildOperator( string operatorName, string propertyName );
        /// <summary>Builds an operator with property as a parameter</summary>
        /// <param name="operatorName">Operator name to create</param>
        /// <param name="value">Value to pass as a parameter</param>
        /// <returns>True if operator created, otherwise False</returns>
        bool BuildOperator( string operatorName, object value );
    }
}
