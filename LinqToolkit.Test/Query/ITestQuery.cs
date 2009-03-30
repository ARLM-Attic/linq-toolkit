using System;
using System.Linq;

namespace LinqToolkit.Test.Query {
    public interface ITestQuery {
        TestContextBase Context { get; }
        Type ElementType { get; }
        IQueryProvider Provider { get; }
    }
}
