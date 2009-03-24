using System;
using System.Linq;

namespace LinqToolkit.Test {
    public interface ITestQuery {
        TestContextBase Context { get; }
        Type ElementType { get; }
        IQueryProvider Provider { get; }
    }
}
