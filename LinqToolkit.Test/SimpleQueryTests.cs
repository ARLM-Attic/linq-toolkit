using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqToolkit.Test {
    using LinqToolkit.SimpleQuery;
    using LinqToolkit.Test.SimpleQuery;
    using System.Linq.Expressions;

    [TestClass]
    public class SimpleQueryTests {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void SimpleQuerySelectSimple() {
            var testQuery = (TestSimpleQuery<TestItem>)
                from item in new TestSimpleQuery<TestItem>()
                select item;
            Assert.IsFalse( testQuery.Options.Operators.Any() );
        }
        [TestMethod]
        public void SimpleQueryWhereBinaryOperation() {
            var testQuery = (TestSimpleQuery<TestItem>)
                from item in new TestSimpleQuery<TestItem>()
                where item.TestPropertySimple=="123"
                select item;
            Assert.IsInstanceOfType( testQuery.Options.Filter, typeof( BinaryOperation ) );
            var result = (BinaryOperation)testQuery.Options.Filter;
            Assert.AreEqual( "TestPropertySimple", result.PropertyName );
            Assert.AreEqual( "123", result.Value );
            Assert.AreEqual( ExpressionType.Equal, result.Type );
        }
        [TestMethod]
        public void SimpleQueryWhereUnaryOperation() {
            var testQuery = (TestSimpleQuery<TestItem>)
                from item in new TestSimpleQuery<TestItem>()
                where !( item.TestField )
                select item;
            Assert.IsInstanceOfType( testQuery.Options.Filter, typeof( UnaryOperation ) );
            var result = (UnaryOperation)testQuery.Options.Filter;
            Assert.AreEqual( "TestField", result.PropertyName );
            Assert.AreEqual( ExpressionType.Not, result.Type );
        }
        [TestMethod]
        public void SimpleQueryWhereCallOperation() {
            var testQuery = (TestSimpleQuery<TestItem>)
                from item in new TestSimpleQuery<TestItem>()
                where item.TestPropertySimple.Contains( "123" )
                select item;
            Assert.IsInstanceOfType( testQuery.Options.Filter, typeof( CallOperation ) );
            var result = (CallOperation)testQuery.Options.Filter;
            Assert.AreEqual( "TestPropertySimple", result.PropertyName );
            Assert.AreEqual( "Contains", result.MethodName );
            Assert.AreEqual( 1, result.Arguments.Length );
            Assert.AreEqual( "123", result.Arguments[0] );
        }
        [TestMethod]
        public void SimpleQueryWhereJoinOperation() {
            var testQuery = (TestSimpleQuery<TestItem>)
                from item in new TestSimpleQuery<TestItem>()
                where
                    item.TestPropertySimple=="123" &&
                    !(item.TestField)
                select item;
            var result = (JoinOperation)testQuery.Options.Filter;
            Assert.AreEqual( ExpressionType.AndAlso, result.Type );
            Assert.IsInstanceOfType( result.Left, typeof(BinaryOperation) );
            Assert.IsInstanceOfType( result.Right, typeof( UnaryOperation ) );
        }
        [TestMethod]
        public void SimpleQueryBuildOperatorByName() {
            var testQuery = (TestSimpleQuery<TestItem>)
                new TestSimpleQuery<TestItem>().Distinct();
            Assert.AreEqual( 1, testQuery.Options.Operators.Count );
            var result = testQuery.Options.Operators.First();
            Assert.AreEqual( "Distinct", result.OperatorName );
            Assert.IsNull( result.PropertyName );
            Assert.IsNull( result.Value );
        }
        [TestMethod]
        public void SimpleQueryBuildOperatorByNameAndPropertyName() {
            var testQuery = (TestSimpleQuery<TestItem>)
                new TestSimpleQuery<TestItem>().OrderBy( item => item.TestPropertySimple );
            Assert.AreEqual( 1, testQuery.Options.Operators.Count );
            var result = testQuery.Options.Operators.First();
            Assert.AreEqual( "OrderBy", result.OperatorName );
            Assert.AreEqual( "TestPropertySimple", result.PropertyName );
            Assert.IsNull( result.Value );
        }
        [TestMethod]
        public void SimpleQueryBuildOperatorByNameAndValue() {
            var testQuery = (TestSimpleQuery<TestItem>)
                new TestSimpleQuery<TestItem>().Take( 10 );
            Assert.AreEqual( 1, testQuery.Options.Operators.Count );
            var result = testQuery.Options.Operators.First();
            Assert.AreEqual( "Take", result.OperatorName );
            Assert.IsNull( result.PropertyName );
            Assert.AreEqual( 10, result.Value );
        }
    }
}
