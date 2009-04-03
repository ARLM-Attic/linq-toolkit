using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    public class UnaryOperation: BaseOperation {

        [XmlAttribute]
        public ExpressionType Type { get; set; }
        [XmlAttribute]
        public string PropertyName { get; set; }
    }
}
