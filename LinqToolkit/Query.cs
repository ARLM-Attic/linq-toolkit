using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit {

    /// <summary>
    /// Represents a generic data source. Allows for querying a data source via LINQ.
    /// This code is based on Bart De Smet's on-line blog:
    /// http://community.bartdesmet.net/blogs/bart/archive/2007/04/05/the-iqueryable-tales-linq-to-ldap-part-0.aspx
    /// The main feature is an abstraction of query filter built on Where operator.
    /// </summary>
    /// <typeparam name="TContext">Expected execution context type which should implements <see cref="IQueryContext"/> interface.</typeparam>
    /// <typeparam name="TEntity">Expected entity type in the underlying source.</typeparam>
    public abstract partial class Query<TContext, TEntity>: IOrderedQueryable<TEntity>, IQueryProvider
        where TContext: IQueryContext {
        #region private fields
        private Delegate project;
        private Type originalType = typeof( TEntity );
        #endregion private fields
        #region protected properties
        /// <summary>
        /// Gets current execution context object.
        /// </summary>
        protected TContext Context { get; private set; }
        #endregion protected properties
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TContext, TEntity}"/> class.
        /// </summary>
        /// <param name="context">An object which implements <see cref="IQueryContext"/> interface.</param>
        public Query( TContext context ) {
            this.Context = context;
        }
        #endregion constructors
        #region Abstract methods
        /// <summary>
        /// Creates a new <see cref="Query{TContext, TEntity}"/> object where <typeparamref name="TEntity"/> is of <typeparamref name="T"/> type and copies data from current object.
        /// </summary>
        /// <typeparam name="T">Entity type to convert from underlying <typeparamref name="TEntity"/> source type.</typeparam>
        /// <returns>Created instance of the <see cref="Query{TContext, TEntity}"/> object where <typeparamref name="TEntity"/> is of <typeparamref name="T"/> type.</returns>
        protected abstract Query<TContext, T> Copy<T>();
        /// <summary>
        /// Executes request to specific provider.
        /// </summary>
        /// <returns>Result of the request to specific provider.</returns>
        protected abstract IEnumerable<object> ExecuteRequest();
        #endregion Abstract methods
        #region IQueryable Members
        public Type ElementType {
            get { return typeof( TEntity ); }
        }
        public Expression Expression {
            get { return Expression.Constant( this ); }
        }
        public IQueryProvider Provider {
            get { return this; }
        }
        #endregion
        #region IQueryProvider Members
        public IQueryable CreateQuery( Expression expression ) {
            return CreateQuery<TEntity>( expression );
        }
        public IQueryable<T> CreateQuery<T>( Expression expression ) {

            MethodCallExpression call = expression as MethodCallExpression;
            if ( call==null ) {
                throw new NotSupportedException( "Expected MethodCallExpression: " + expression.ToString() );
            }

            Query<TContext, T> result = Copy<T>( this );

            if ( !result.BuildOperator( call ) ) {
                throw new NotSupportedException( "Unsupported query operator detected: " + call.ToString() );
            }

            return result;
        }
        public object Execute( Expression expression ) {
            throw new NotImplementedException();
        }
        public TResult Execute<TResult>( Expression expression ) {
            throw new NotImplementedException();
        }
        private static Query<TContext, T> Copy<T>( Query<TContext, TEntity> source ) {
            Query<TContext, T> result = source.Copy<T>();
            result.Context = source.Context;
            result.project = source.project;
            result.originalType = source.originalType;
            return result;
        }
        #endregion
        #region Enumeration support
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public IEnumerator<TEntity> GetEnumerator() {
            return GetResults().GetEnumerator();
        }
        private IEnumerable<TEntity> GetResults() {
            if ( this.Context.Options.PropertiesToRead.Count==0 ) {
                foreach ( var property in typeof( TEntity ).GetProperties() ) {
                    try {
                        string propertyName = this.GetSourcePropertyName( property );
                        this.Context.Options.PropertiesToRead.Add( propertyName );
                    }
                    catch ( InvalidOperationException ) {
                    }
                }
            }

            IEnumerable<object> results = this.ExecuteRequest();

            if ( project==null ) {
                return results.Cast<TEntity>();
            }

            return
                from result in results
                select (TEntity)project.DynamicInvoke( result );
        }
        #endregion
        #region Attributes support
        private string GetFilterPropertyName( MemberInfo member ) {
            return this.GetPropertyName<FilterPropertyAttribute>( member );
        }
        private string GetSourcePropertyName( MemberInfo member ) {
            return this.GetPropertyName<SourcePropertyAttribute>( member );
        }
        private string GetPropertyName<TAttribute>( MemberInfo member ) where TAttribute: Attribute, IPropertyName {

            if ( !member.DeclaringType.IsAssignableFrom( this.originalType ) ) {
                throw new InvalidOperationException( "Declaring type is not assignable from original: " + member.Name );
            }

            object[] customAttributes = member.GetCustomAttributes( false );

            if ( customAttributes.OfType<IgnoreAttribute>().Any() ) {
                throw new InvalidOperationException( "Property marked with IgnoreAttribute: " + member.Name );
            }

            IPropertyName sourceProperty = customAttributes.OfType<TAttribute>().FirstOrDefault();
            return
                sourceProperty!=null
                ? sourceProperty.Name
                : member.Name;
        }
        #endregion Attributes support
    }
}
