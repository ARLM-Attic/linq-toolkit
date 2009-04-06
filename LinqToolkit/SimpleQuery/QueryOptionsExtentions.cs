using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Linq.Expressions;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// Defines extension methods for SimpleQueryOptions class.
    /// </summary>
    public static class QueryOptionsExtentions {

        /// <summary>
        /// Transforms SimpleQueryOptions instance using xslt.
        /// </summary>
        /// <param name="source">Instance of <see cref="SimpleQueryOptions"/> class to transform.</param>
        /// <param name="transform">Instance of <see cref="XslCompiledTransform"/> class to apply on <paramref name="source"/>.</param>
        /// <returns>Returns result of transformation.</returns>
        public static string Transform(
            this QueryOptions source,
            XslCompiledTransform transform
            ) {
            return
                source.Transform(
                    transform,
                    new XmlWriterSettings() {
                        ConformanceLevel = ConformanceLevel.Fragment,
                        Indent = false
                    }
                    );
        }
        /// <summary>
        /// Transforms SimpleQueryOptions instance using xslt.
        /// </summary>
        /// <param name="source">Instance of <see cref="SimpleQueryOptions"/> class to transform.</param>
        /// <param name="transform">Instance of <see cref="XslCompiledTransform"/> class to apply on <paramref name="source"/>.</param>
        /// <param name="settings">Instance of <see cref="XmlWriterSettings" /> to be applied during transform.</param>
        /// <returns>Returns result of transformation.</returns>
        public static string Transform(
            this QueryOptions source,
            XslCompiledTransform transform,
            XmlWriterSettings settings
            ) {
            if ( source == null ) {
                throw new ArgumentNullException( "source" );
            }
            if ( transform == null ) {
                throw new ArgumentNullException( "transform" );
            }
            var serializer = new XmlSerializer( typeof( QueryOptions ) );
            var serializeWriter = new StringWriter();
            serializer.Serialize( serializeWriter, source );
            var resultWriter = new StringWriter();
            transform.Transform(
                XmlReader.Create( new StringReader( serializeWriter.ToString() ) ),
                XmlWriter.Create( resultWriter, settings )
                );
            return resultWriter.ToString();
        }
        public static IQueryable<TSource> Where<TSource>( this IQueryable<TSource> source, Expression<Func<TSource, BaseOperation>> predicate ) {
            return
                QueryOptionsToQueryable<TSource>.Filter(
                    source,
                    Expression.Lambda<Func<BaseOperation>>( predicate.Body ).Compile()()
                    );
        }
        private static class QueryOptionsToQueryable<TSource> {

            private static readonly ParameterExpression parameterExpression;
            private static readonly Type queryableType;
            private static readonly Type queryableSourceType;
            private static readonly Type sourceType;

            static QueryOptionsToQueryable() {
                parameterExpression = Expression.Parameter( typeof( TSource ), "item" );
                queryableType = typeof( Queryable );
                sourceType = typeof( TSource );
                queryableSourceType = typeof( IQueryable<TSource> );
            }

            public static IQueryable<TSource> Filter( IQueryable<TSource> source, BaseOperation operaton ) {
                if ( source == null ) {
                    throw new ArgumentNullException( "source" );
                }
                if ( operaton == null ) {
                    throw new ArgumentNullException( "operaton" );
                }
                Expression<Func<TSource, bool>> predicate =
                    Expression.Lambda<Func<TSource, bool>>(
                        BuildOperation( operaton ),
                        parameterExpression
                        );
                return source.Where( predicate );
            }
            private static Expression BuildOperation( BaseOperation operation ) {
                var handlers = new Func<BaseOperation, Expression>[] {
                    BuildJoinOperation,
                    BuildBinaryOperation,
                    BuildUnaryOperation,
                    BuildCallOperation,
                };
                var result =
                    handlers
                    .Select( handler => handler( operation ) )
                    .Where( item => item!=null )
                    .FirstOrDefault();
                if ( result==null ) {
                    throw new NotImplementedException();
                }
                return result;
            }
            private static Expression BuildJoinOperation( BaseOperation operation ) {
                var typedOperation = operation as JoinOperation;
                if ( typedOperation==null ) {
                    return null;
                }
                Func<Expression, Expression, Expression> call;
                switch ( typedOperation.Type ) {
                    case ExpressionType.AndAlso:
                        call = Expression.AndAlso;
                        break;
                    case ExpressionType.OrElse:
                        call = Expression.OrElse;
                        break;
                    default:
                        return null;
                }
                return
                    call(
                        BuildOperation( typedOperation.Left ),
                        BuildOperation( typedOperation.Right )
                        );
            }
            private static Expression BuildBinaryOperation( BaseOperation operation ) {
                var typedOperation = operation as BinaryOperation;
                if ( typedOperation==null ) {
                    return null;
                }
                Expression left =
                    Expression.MakeMemberAccess(
                        parameterExpression,
                        sourceType.GetMember( typedOperation.PropertyName ).First()
                        );
                Expression right = Expression.Constant( typedOperation.Value );
                return Expression.MakeBinary( typedOperation.Type, left, right );
            }
            private static Expression BuildUnaryOperation( BaseOperation operation ) {
                var typedOperation = operation as UnaryOperation;
                if ( typedOperation==null ) {
                    return null;
                }
                Expression expression =
                    Expression.MakeMemberAccess(
                        parameterExpression,
                        sourceType.GetMember( typedOperation.PropertyName ).First()
                        );
                return
                    typedOperation.Type==ExpressionType.MemberAccess
                    ? expression
                    : Expression.MakeUnary( typedOperation.Type, expression, null );
            }
            private static Expression BuildCallOperation( BaseOperation operation ) {
                var typedOperation = operation as CallOperation;
                if ( typedOperation==null ) {
                    return null;
                }
                var memberInfo = sourceType.GetMember( typedOperation.PropertyName ).First();
                Type memberType =
                    memberInfo is PropertyInfo
                    ? ( (PropertyInfo)memberInfo ).PropertyType
                    : ( (FieldInfo)memberInfo ).FieldType;

                if (typedOperation.Arguments.Any()) {
                    var argumentTypes =
                        from argument in typedOperation.Arguments
                        select argument.GetType();
                    var argumentExpressions =
                        from argument in typedOperation.Arguments
                        select Expression.Constant(argument);
                    return
                        Expression.Call(
                            Expression.MakeMemberAccess(
                                parameterExpression,
                                memberInfo
                                ),
                            memberType.GetMethod(
                                typedOperation.MethodName,
                                argumentTypes.ToArray()
                                ),
                            argumentExpressions.ToArray()
                        );
                } else {
                    return
                        Expression.Call(
                            Expression.MakeMemberAccess(
                                parameterExpression,
                                memberInfo
                                ),
                            memberType.GetMethod( typedOperation.MethodName )
                        );
                }
            }
        }
    }
}
