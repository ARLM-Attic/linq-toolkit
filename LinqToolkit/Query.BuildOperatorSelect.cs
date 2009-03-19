using System;
using System.Linq.Expressions;

namespace LinqToolkit {

    public partial class Query<TContext, TEntity> {

        private bool BuildOperatorSelect( LambdaExpression expression, string methodName ) {
            if ( methodName!="Select" ) {
                return false;
            }

            //
            // Store projection information including the compiled lambda for subsequent execution
            // and a minimal set of properties to be retrieved (improves efficiency of queries).
            //
            project = expression.Compile();

            //
            // Original type is kept for reflection during querying.
            //
            originalType = expression.Parameters[0].Type;

            //
            // Support for projections, getting properties back from expression.
            //
            FindProperties( expression );

            return true;
        }

        private void FindProperties( Expression expression ) {

            //
            // Record member accesses to properties or fields from the entity.
            //
            if ( expression.NodeType==ExpressionType.MemberAccess ) {
                MemberExpression memberExpression = (MemberExpression)expression;
                string propertyName = this.GetSourcePropertyName( memberExpression.Member );
                this.Context.Options.PropertiesToRead.Add( propertyName );
            }
            else {
                if ( expression is BinaryExpression ) {
                    BinaryExpression b = expression as BinaryExpression;
                    FindProperties( b.Left );
                    FindProperties( b.Right );
                }
                else if ( expression is UnaryExpression ) {
                    UnaryExpression u = expression as UnaryExpression;
                    FindProperties( u.Operand );
                }
                else if ( expression is ConditionalExpression ) {
                    ConditionalExpression c = expression as ConditionalExpression;
                    FindProperties( c.IfFalse );
                    FindProperties( c.IfTrue );
                    FindProperties( c.Test );
                }
                else if ( expression is InvocationExpression ) {
                    InvocationExpression i = expression as InvocationExpression;
                    FindProperties( i.Expression );
                    foreach ( Expression ex in i.Arguments ) {
                        FindProperties( ex );
                    }
                }
                else if ( expression is LambdaExpression ) {
                    LambdaExpression l = expression as LambdaExpression;
                    FindProperties( l.Body );
                    foreach ( Expression ex in l.Parameters ) {
                        FindProperties( ex );
                    }
                }
                else if ( expression is ListInitExpression ) {
                    ListInitExpression li = expression as ListInitExpression;
                    FindProperties( li.NewExpression );
                    foreach ( ElementInit i in li.Initializers ) {
                        foreach ( var ex in i.Arguments ) {
                            FindProperties( ex );
                        }
                    }
                }
                else if ( expression is MemberInitExpression ) {
                    MemberInitExpression mi = expression as MemberInitExpression;
                    FindProperties( mi.NewExpression );
                    foreach ( MemberAssignment b in mi.Bindings ) {
                        FindProperties( b.Expression );
                    }
                }
                else if ( expression is MethodCallExpression ) {
                    MethodCallExpression mc = expression as MethodCallExpression;
                    FindProperties( mc.Object );
                    foreach ( Expression ex in mc.Arguments ) {
                        FindProperties( ex );
                    }
                }
                else if ( expression is NewExpression ) {
                    NewExpression n = expression as NewExpression;
                    foreach ( Expression ex in n.Arguments ) {
                        FindProperties( ex );
                    }
                }
                else if ( expression is NewArrayExpression ) {
                    NewArrayExpression na = expression as NewArrayExpression;
                    foreach ( Expression ex in na.Expressions ) {
                        FindProperties( ex );
                    }
                }
                else if ( expression is TypeBinaryExpression ) {
                    TypeBinaryExpression tb = expression as TypeBinaryExpression;
                    FindProperties( tb.Expression );
                }
            }
        }
    }
}