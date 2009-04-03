using System;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryOperator {
        [XmlAttribute]
        public string OperatorName { get; set; }
        [XmlAttribute]
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
