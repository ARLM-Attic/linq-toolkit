using System;
using System.Linq;
using System.Reflection;

namespace LinqToolkit.Test.Query {
    public class TestCallOperation: IBaseOperation, IEquatable<TestCallOperation> {

        public readonly MethodInfo Method;
        public readonly string PropertyName;
        public readonly object[] Arguments;

        public TestCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            this.Method = method;
            this.PropertyName = propertyName;
            this.Arguments = arguments;
        }
        #region Equals support
        public bool Equals( TestCallOperation other ) {
            return
                this.Method.Equals( other.Method ) && 
                this.PropertyName.Equals( other.PropertyName ) &&
                this.Arguments.Length==other.Arguments.Length &&
                !this.Arguments.Except( other.Arguments ).Any();
        }
        public override bool Equals( object obj ) {
            if ( obj is TestCallOperation ) {
                return this.Equals( (TestCallOperation)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return
                this.Method.GetHashCode() ^
                this.PropertyName.GetHashCode();
        }
        #endregion Equals support
    }
}
