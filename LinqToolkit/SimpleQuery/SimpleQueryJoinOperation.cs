using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryJoinOperation: SimpleQueryBaseOperation, IJoinOperation {

        [XmlAttribute]
        public ExpressionType Type { get; set; }
        public SimpleQueryBaseOperation Left { get; set; }
        public SimpleQueryBaseOperation Right { get; set; }

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
