using System;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    [XmlInclude( typeof( BinaryOperation ) )]
    [XmlInclude( typeof( UnaryOperation ) )]
    [XmlInclude( typeof( CallOperation ) )]
    [XmlInclude( typeof( JoinOperation ) )]
    public abstract class BaseOperation: IBaseOperation {
    }
}
