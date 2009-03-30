using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace LinqToolkit.Test.Query {
    public static class Queryable {
        public static IQueryable<TSource> TestOperator<TSource>( this IQueryable<TSource> source, int parameter1, int parameter2 ) {
            Expression expression =
                Expression.Call(
                    null,
                    ( (MethodInfo)MethodBase.GetCurrentMethod() )
                    .MakeGenericMethod( new Type[] { typeof( TSource ) } ),
                    new Expression[] {
                        source.Expression,
                        Expression.Constant( parameter1 ),
                        Expression.Constant( parameter1 ),
                    }
                );
            return (IQueryable<TSource>)source.Provider.CreateQuery( expression );
        }
        public static string TestExecuteOperator<TSource>( this IQueryable<TSource> source ) {
            Expression expression =
                Expression.Call(
                    null,
                    ( (MethodInfo)MethodBase.GetCurrentMethod() )
                    .MakeGenericMethod( new Type[] { typeof( TSource ) } ),
                    new Expression[] { source.Expression }
                );
            return (string)source.Provider.Execute( expression );
        }
        public static int TestExecuteOperator<TSource>( this IQueryable<TSource> source, int parameter ) {
            Expression expression =
                Expression.Call(
                    null,
                    ( (MethodInfo)MethodBase.GetCurrentMethod() )
                    .MakeGenericMethod( new Type[] { typeof( TSource ) } ),
                    new Expression[] {
                        source.Expression,
                        Expression.Constant( parameter )
                    }
                );
            return (int)source.Provider.Execute( expression );
        }
    }
}
