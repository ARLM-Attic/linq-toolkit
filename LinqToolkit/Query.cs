using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqToolkit.Properties;

namespace LinqToolkit {

    /// <summary>
    /// Represents a generic data source. Allows for querying a data source via LINQ.
    /// This code is based on Bart De Smet's on-line blog:
    /// http://community.bartdesmet.net/blogs/bart/archive/2007/04/05/the-iqueryable-tales-linq-to-ldap-part-0.aspx
    /// The main feature is an abstraction of query filter built on Where operator.
    /// </summary>
    /// <typeparam name="TContext">Expected execution context type which should implements <see cref="IQueryContext"/> interface.</typeparam>
    /// <typeparam name="TItem">Expected entity type in the underlying source.</typeparam>
    public abstract partial class Query<TContext, TItem>: IOrderedQueryable<TItem>, IQueryProvider
        where TContext: IQueryContext {
        #region static fields
        private static Type ItemType;
        #endregion static fields
        #region static constructor
        static Query() {
            ItemType = typeof( TItem );
        }
        #endregion static constructor
        #region private fields
        private Delegate project;
        private Type originalType;
        #endregion private fields
        #region protected properties
        /// <summary>
        /// Gets current execution context object.
        /// </summary>
        protected TContext Context { get; private set; }
        #endregion protected properties
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TContext, TItem}"/> class.
        /// </summary>
        /// <param name="context">An object which implements <see cref="IQueryContext"/> interface.</param>
        public Query( TContext context ) {
            originalType = ItemType;
            this.Context = context;
        }
        #endregion constructors
        #region Abstract methods
        /// <summary>
        /// Creates a new <see cref="Query{TContext, TItem}"/> object where <typeparamref name="TItem"/> is of <typeparamref name="T"/> type and copies data from current object.
        /// </summary>
        /// <typeparam name="T">Entity type to convert from underlying <typeparamref name="TItem"/> source type.</typeparam>
        /// <returns>Created instance of the <see cref="Query{TContext, TItem}"/> object where <typeparamref name="TItem"/> is of <typeparamref name="T"/> type.</returns>
        protected abstract Query<TContext, T> Copy<T>();
        /// <summary>
        /// Executes request to specific provider.
        /// </summary>
        /// <returns>Result of the request to specific provider.</returns>
        protected abstract IEnumerable<object> EnumerateQuery();
        protected abstract TResult ExecuteQuery<TResult>( string commandName );
        #endregion Abstract methods
        #region IQueryable Members
        public Type ElementType {
            get { return ItemType; }
        }
        public Expression Expression {
            get { return Expression.Constant( this ); }
        }
        public IQueryProvider Provider {
            get { return this; }
        }
        #endregion
        #region IQueryProvider Members
        IQueryable IQueryProvider.CreateQuery( Expression expression ) {
            return ((IQueryProvider)this).CreateQuery<TItem>( expression );
        }
        IQueryable<T> IQueryProvider.CreateQuery<T>( Expression expression ) {

            MethodCallExpression call = expression as MethodCallExpression;
            if ( call==null ) {
                throw
                    new InvalidOperationException(
                        string.Format(
                            Resources.CreateQueryInvalidOperationFormat,
                            expression.ToString()
                            )
                        );
            }

            Query<TContext, T> result = Copy<T>( this );

            if ( !result.BuildOperator( call ) ) {
                throw
                    new NotSupportedException(
                        string.Format(
                            Resources.CreateQueryNotSupportedFormat,
                            call.ToString()
                            )
                        );
            }

            return result;
        }
        object IQueryProvider.Execute( Expression expression ) {
            return ((IQueryProvider)this).Execute<object>( expression );
        }
        TResult IQueryProvider.Execute<TResult>( Expression expression ) {
            MethodCallExpression call = expression as MethodCallExpression;
            if ( call==null ) {
                throw
                    new InvalidOperationException(
                        string.Format(
                            Resources.ExecuteInvalidOperationFormat,
                            expression.ToString()
                            )
                        );
            }
            if (call.Arguments.Count>1) {
                throw
                    new NotSupportedException(
                        string.Format(
                            Resources.ExecuteArgumentsNotSupportedFormat,
                            expression.ToString()
                            )
                        );
            }
            return this.ExecuteQuery<TResult>( call.Method.Name );
        }
        private static Query<TContext, T> Copy<T>( Query<TContext, TItem> source ) {
            Query<TContext, T> result = source.Copy<T>();
            result.Context = source.Context;
            result.project = source.project;
            result.originalType = source.originalType;
            return result;
        }
        #endregion
        #region Enumeration support
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        public IEnumerator<TItem> GetEnumerator() {
            return this.GetResults().GetEnumerator();
        }
        private IEnumerable<TItem> GetResults() {

            IEnumerable<object> results = this.EnumerateQuery();

            if ( project==null ) {
                return results.Cast<TItem>();
            }

            return
                from result in results
                select (TItem)project.DynamicInvoke( result );
        }
        #endregion
        #region Attributes support
        protected string GetSourcePropertyName( MemberExpression expression ) {
            if ( !( expression.Expression is ParameterExpression ) ) {
                throw
                    new NotSupportedException(
                        string.Format(
                            "Accepted only entity members: {0}",
                            expression.ToString()
                            )
                        );
            }
            return this.GetSourcePropertyName( expression.Member, true );
        }
        protected string GetSourcePropertyName( MemberInfo member, bool throwOnError ) {
            if ( !member.DeclaringType.IsAssignableFrom( this.originalType ) ) {
                if ( !throwOnError ) { return null; }
                throw
                    new InvalidOperationException(
                        string.Format(
                            Resources.GetPropertyNameNotAssignableFormat,
                            member.Name
                            )
                        );
            }

            object[] customAttributes = member.GetCustomAttributes( false );

            if ( customAttributes.OfType<IgnoreAttribute>().Any() ) {
                if ( !throwOnError ) { return null; }
                throw
                    new InvalidOperationException(
                        string.Format(
                            Resources.GetPropertyNameIgnoreAttributeFormat,
                            member.Name
                            )
                        );
            }

            SourcePropertyAttribute sourceProperty =
                customAttributes
                .OfType<SourcePropertyAttribute>()
                .FirstOrDefault();

            return
                sourceProperty!=null
                ? sourceProperty.Name
                : member.Name;
        }
        #endregion Attributes support
    }
}
