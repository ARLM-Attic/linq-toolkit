using System;

namespace LinqToolkit.Test.Query {
    public interface ITestContext: IQueryContext {
        TestOperator Operator { get; }
    }
}
