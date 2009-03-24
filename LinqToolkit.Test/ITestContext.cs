using System;

namespace LinqToolkit.Test {
    public interface ITestContext: IQueryContext {
        TestOperator Operator { get; }
    }
}
