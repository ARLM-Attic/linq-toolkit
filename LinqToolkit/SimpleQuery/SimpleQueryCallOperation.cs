using System;
using System.Reflection;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryCallOperation: SimpleQueryBaseOperation {

        public MethodInfo Method { get; private set; }
        public string PropertyName { get; private set; }
        public object[] Arguments { get; private set; }

        internal SimpleQueryCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            this.Method = method;
            this.PropertyName = propertyName;
            this.Arguments = arguments;
        }
    }
}
