using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// This class is constructed inside the <see cref="SimpleQueryContext"/> class.
    /// Implements <see cref="IQueryOptions"/> interface and contains concrete query options including operators applied to a query.
    /// </summary>
    [XmlRoot( "Options")]
    public class QueryOptions: IQueryOptions {
        public string Source { get; set; }
        public BaseOperation Filter { get; set; }
        [XmlArrayItem( "Property" )]
        public HashSet<string> PropertiesToRead { get; set; }
        [XmlArrayItem( "Operator" )]
        public List<QueryOperator> Operators { get; set; }

        public QueryOptions() {
            this.PropertiesToRead = new HashSet<string>();
            this.Operators = new List<QueryOperator>();
        }

        #region IQueryOptions Members
        IBaseOperation IQueryOptions.Filter {
            get { return this.Filter; }
            set { this.Filter = (BaseOperation)value; }
        }
        #endregion
    }
}
