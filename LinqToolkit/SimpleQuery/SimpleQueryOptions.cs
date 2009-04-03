using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// This class is constructed inside the <see cref="SimpleQueryContext"/> class.
    /// Implements <see cref="IQueryOptions"/> interface and contains concrete query options including operators applied to a query.
    /// </summary>
    [XmlRoot( "Options")]
    public class SimpleQueryOptions: IQueryOptions {
        public string Source { get; set; }
        public SimpleQueryBaseOperation Filter { get; set; }
        [XmlArrayItem( "Property" )]
        public HashSet<string> PropertiesToRead { get; set; }
        [XmlArrayItem( "Operator" )]
        public List<SimpleQueryOperator> Operators { get; set; }

        public SimpleQueryOptions() {
            this.PropertiesToRead = new HashSet<string>();
            this.Operators = new List<SimpleQueryOperator>();
        }

        #region IQueryOptions Members
        IBaseOperation IQueryOptions.Filter {
            get { return this.Filter; }
            set { this.Filter = (SimpleQueryBaseOperation)value; }
        }
        #endregion
    }
}
