using System;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    public class CallOperation: BaseOperation {

        [XmlAttribute]
        public string MethodName { get; set; }
        [XmlAttribute]
        public string PropertyName { get; set; }
        [XmlArrayItem("Value")]
        public object[] Arguments { get; set; }
    }
}
