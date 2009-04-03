using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    [XmlInclude( typeof( SimpleQueryBinaryOperation ) )]
    [XmlInclude( typeof( SimpleQueryUnaryOperation ) )]
    [XmlInclude( typeof( SimpleQueryCallOperation ) )]
    [XmlInclude( typeof( SimpleQueryJoinOperation ) )]
    public abstract class SimpleQueryBaseOperation: IBaseOperation {
    }
}
