using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    public class JoinOperation: BaseOperation, IJoinOperation {

        [XmlAttribute]
        public ExpressionType Type { get; set; }
        public BaseOperation Left { get; set; }
        public BaseOperation Right { get; set; }

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
