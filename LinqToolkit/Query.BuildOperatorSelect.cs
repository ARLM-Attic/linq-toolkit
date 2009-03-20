using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToolkit {

    public partial class Query<TContext, TItem> {

        private bool BuildOperatorSelect( LambdaExpression expression, string methodName ) {
            if ( methodName!="Select" ) {
                return false;
            }

            //
            // Store projection information including the compiled lambda for subsequent execution
            // and a minimal set of properties to be retrieved (improves efficiency of queries).
            //
            this.project = expression.Compile();

            //
            // Original type is kept for reflection during querying.
            //
            this.originalType = expression.Parameters[0].Type;

            //
            // Support for projections, getting properties back from expression.
            //
            this.FindProperties( expression );

            //
            // If expression is not accesses to any property or field take them from resulting item type
            //
            if ( this.Context.Options.PropertiesToRead.Count==0 ) {
                Type itemType = typeof( TItem );
                var members =
                    itemType.GetFields( BindingFlags.Public | BindingFlags.Instance )
                    .Cast<MemberInfo>()
                    .Union( itemType.GetProperties( BindingFlags.Public | BindingFlags.Instance ) );
                foreach ( var member in members ) {
                    string propertyName = this.GetSourcePropertyName( member, false );
                    if ( propertyName==null ) {
                        continue;
                    }
                    this.Context.Options.PropertiesToRead.Add( propertyName );
                }
            }

            return true;
        }

        private void FindProperties( Expression expression ) {

            //
            // Record member accesses to properties or fields from the item.
            //
            if ( expression.NodeType==ExpressionType.MemberAccess ) {
                MemberExpression memberExpression = (MemberExpression)expression;
                string propertyName = this.GetSourcePropertyName( memberExpression.Member );
                this.Context.Options.PropertiesToRead.Add( propertyName );
            }
            else {
                if ( expression is BinaryExpression ) {
                    BinaryExpression b = expression as BinaryExpression;
                    this.FindProperties( b.Left );
                    this.FindProperties( b.Right );
                }
                else if ( expression is UnaryExpression ) {
                    UnaryExpression u = expression as UnaryExpression;
                    this.FindProperties( u.Operand );
                }
                else if ( expression is ConditionalExpression ) {
                    ConditionalExpression c = expression as ConditionalExpression;
                    this.FindProperties( c.IfFalse );
                    this.FindProperties( c.IfTrue );
                    this.FindProperties( c.Test );
                }
                else if ( expression is InvocationExpression ) {
                    InvocationExpression i = expression as InvocationExpression;
                    this.FindProperties( i.Expression );
                    foreach ( Expression ex in i.Arguments ) {
                        this.FindProperties( ex );
                    }
                }
                else if ( expression is LambdaExpression ) {
                    LambdaExpression l = expression as LambdaExpression;
                    this.FindProperties( l.Body );
                    foreach ( Expression ex in l.Parameters ) {
                        this.FindProperties( ex );
                    }
                }
                else if ( expression is ListInitExpression ) {
                    ListInitExpression li = expression as ListInitExpression;
                    this.FindProperties( li.NewExpression );
                    foreach ( ElementInit i in li.Initializers ) {
                        foreach ( var ex in i.Arguments ) {
                            this.FindProperties( ex );
                        }
                    }
                }
                else if ( expression is MemberInitExpression ) {
                    MemberInitExpression mi = expression as MemberInitExpression;
                    this.FindProperties( mi.NewExpression );
                    foreach ( MemberAssignment b in mi.Bindings ) {
                        this.FindProperties( b.Expression );
                    }
                }
                else if ( expression is MethodCallExpression ) {
                    MethodCallExpression mc = expression as MethodCallExpression;
                    this.FindProperties( mc.Object );
                    foreach ( Expression ex in mc.Arguments ) {
                        this.FindProperties( ex );
                    }
                }
                else if ( expression is NewExpression ) {
                    NewExpression n = expression as NewExpression;
                    foreach ( Expression ex in n.Arguments ) {
                        this.FindProperties( ex );
                    }
                }
                else if ( expression is NewArrayExpression ) {
                    NewArrayExpression na = expression as NewArrayExpression;
                    foreach ( Expression ex in na.Expressions ) {
                        this.FindProperties( ex );
                    }
                }
                else if ( expression is TypeBinaryExpression ) {
                    TypeBinaryExpression tb = expression as TypeBinaryExpression;
                    this.FindProperties( tb.Expression );
                }
            }
        }
    }
}