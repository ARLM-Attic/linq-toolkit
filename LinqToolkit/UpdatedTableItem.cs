using System;
using System.Collections.Generic;

namespace LinqToolkit {
    public class UpdatedTableItem<TItem> {
        public TItem Item { get; private set; }
        public IEnumerable<string> Properties { get; private set; }
        internal UpdatedTableItem( TItem item, IEnumerable<string> properties ) {
            this.Item = item;
            this.Properties = properties;
        }
    }
}
