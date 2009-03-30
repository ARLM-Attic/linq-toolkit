using System;
using System.Linq.Expressions;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryJoinOperation: SimpleQueryBaseOperation, IJoinOperation {

        public ExpressionType Type { get; private set; }
        public SimpleQueryBaseOperation Left { get; private set; }
        public SimpleQueryBaseOperation Right { get; private set; }

        internal SimpleQueryJoinOperation( ExpressionType type, SimpleQueryBaseOperation left, SimpleQueryBaseOperation right ) {
            this.Type = type;
            this.Left = left;
            this.Right = right;
        }

        #region IJoinOperation Members
        IBaseOperation IJoinOperation.Left {
            get { return this.Left; }
        }
        IBaseOperation IJoinOperation.Right {
            get { return this.Right; }
        }
        #endregion
    }
}
