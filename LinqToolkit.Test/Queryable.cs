using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit.Test {
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
    }
}
