using System;
using System.Linq.Expressions;

namespace LinqToolkit.Test.Query {

    public class TestJoinOperation: IJoinOperation, IEquatable<TestJoinOperation> {

        public readonly ExpressionType Type;
        public IBaseOperation Left { get; private set; }
        public IBaseOperation Right { get; private set; }

        public TestJoinOperation( ExpressionType type, IBaseOperation left, IBaseOperation right ) {
            this.Type = type;
            this.Left = left;
            this.Right = right;
        }

        #region Equals support
        public bool Equals( TestJoinOperation other ) {
            return
                this.Type.Equals( other.Type ) &&
                this.Left.Equals( other.Left ) && 
                this.Right.Equals( other.Right );
        }
        public override bool Equals( object obj ) {
            if ( obj is TestJoinOperation ) {
                return this.Equals( (TestJoinOperation)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return
                this.Type.GetHashCode() ^
                this.Left.GetHashCode() ^
                this.Right.GetHashCode();
        }
        #endregion Equals support
    }
}
