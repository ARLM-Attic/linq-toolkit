using System;
using System.Collections.Generic;

namespace LinqToolkit.SimpleQuery {

    /// <summary>
    /// This class is constructed inside the <see cref="SimpleQueryContext"/> class.
    /// Implements <see cref="IQueryOptions"/> interface and contains concrete query options including operators applied to a query.
    /// </summary>
    public class SimpleQueryOptions: IQueryOptions {

        public SimpleQueryBaseOperation Filter { get; set; }
        public HashSet<string> PropertiesToRead { get; private set; }
        public List<SimpleQueryOperator> Operators { get; private set; }

        internal SimpleQueryOptions() {
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
