using System;
using System.Linq;

namespace LinqToolkit.Test {
    public interface ITestQuery {
        TestContext Context { get; }
        Type ElementType { get; }
        IQueryProvider Provider { get; }
    }
}
