using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqToolkit.Test {
    using LinqToolkit.Test.Query;

    [TestClass]
    public class QueryTests {

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
        public void QuerySelectSimple() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                select item;

            Assert.AreEqual( "TestItem", testQuery.Context.Options.Source );
            Assert.AreEqual( typeof( TestItem ), testQuery.ElementType );
            Assert.IsInstanceOfType( testQuery.Provider, typeof( TestQuery<TestItem> ) );
            Assert.IsNull( testQuery.Context.Options.Filter );
            Assert.AreEqual( 4, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestField" ) );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestSourceProperty" ) );
        }
        [TestMethod]
        public void QuerySelectCast() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                select (ITestItem)item;

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewClass() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<ITestItem>( new TestContext() )
                select new TestItemNew( item.TestPropertySimple );

            Assert.AreEqual( "TestItem", testQuery.Context.Options.Source );
            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewClassInitialization() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                select new TestItemNew {
                    TestPropertySimple = item.TestPropertySimple
                };

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewAnonimousClass() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                select new { Field = item.TestField };

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestField" ) );
        }
        [TestMethod]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void QuerySelectIgnorePropertyInvalidOperationException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                select item.TestPropertyWithIgnore;

        }
        [TestMethod]
        public void QueryWhereBinaryOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                where item.TestPropertySimple=="123"
                select item;

            Assert.AreEqual(
                new TestBinaryOperation( ExpressionType.Equal, "TestPropertySimple", "123" ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereBinaryOperationRightOperandNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                where "123"==item.TestPropertySimple
                select item;
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereBinaryOperationNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where item.TestPropertySimple=="123"
                select item;
        }
        [TestMethod]
        public void QueryWhereUnaryOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                where !( item.TestField )
                select item;

            Assert.AreEqual(
                new TestUnaryOperation( ExpressionType.Not, "TestField" ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereUnaryOperationNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where !( item.TestPropertySimple!="AAA" )
                select item;
        }
        [TestMethod]
        public void QueryWhereCallOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                where item.TestPropertySimple.Contains( "123" )
                select item;

            Assert.AreEqual(
                new TestCallOperation( typeof( string ).GetMethod( "Contains" ), "TestPropertySimple", new object[] { "123" } ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereCallOperationNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where item.TestPropertySimple.Contains( "123" )
                select item;

        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereCallOperationNotEntityNotSupportedException() {
            TestItem dummy = new TestItem();

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where dummy.TestPropertySimple.Contains( "123" )
                select item;

        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereCallOperationNotMemberNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where "123".Contains( "123" )
                select item;

        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereCallOperationNotConstantNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContextEmpty() )
                where item.TestPropertySimple.Contains( item.TestPropertySimple )
                select item;

        }
        [TestMethod]
        public void QueryWhereJoinOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>( new TestContext() )
                where
                    item.TestPropertySimple=="123" &&
                    item.TestPropertyWithSourceProperty==123
                select item;

            Assert.AreEqual(
                new TestJoinOperation(
                    ExpressionType.AndAlso,
                    new TestBinaryOperation( ExpressionType.Equal, "TestPropertySimple", "123" ),
                    new TestBinaryOperation( ExpressionType.Equal, "TestSourceProperty", 123 )
                    ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        public void QueryBuildOperatorByName() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContext() ).Distinct();

            Assert.AreEqual(
                new TestOperator( "Distinct" ),
                testQuery.Context.Operator
                );
        }
        [TestMethod]
        public void QueryBuildOperatorByNameAndPropertyName() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContext() ).OrderBy( item => item.TestPropertySimple );

            Assert.AreEqual(
                new TestOperator( "OrderBy", "TestPropertySimple" ),
                testQuery.Context.Operator
                );
        }
        [TestMethod]
        public void QueryBuildOperatorByNameAndValue() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContext() ).Take( 10 );

            Assert.AreEqual(
                new TestOperator( "Take", 10 ),
                testQuery.Context.Operator
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryBuildOperatorByNameNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContextEmpty() ).Distinct();

        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryBuildOperatorByNameAndPropertyNameNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContextEmpty() ).OrderBy( item => 1 );

        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryBuildOperatorTooManyArgumentsNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>( new TestContextEmpty() ).TestOperator( 1, 2 );

        }
        [TestMethod]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void QueryCreateQueryNotSupported() {

            ITestQuery testQuery =
                (ITestQuery)
                ((IQueryProvider)new TestQuery<TestItem>( new TestContextEmpty() ))
                .CreateQuery<TestItem>( Expression.Constant( 10 ) );

        }
        [TestMethod]
        public void QuerySelectSimpleGetEnumerator() {
            foreach ( var item in new TestQuery<TestItem>( new TestContext() ) )
                ;
        }
        [TestMethod]
        public void QuerySelectCastGetEnumerator() {
            var testQuery =
                from item in new TestQuery<TestItem>( new TestContext() )
                select (ITestItem)item;
            foreach ( var item in testQuery )
                ;
        }
        [TestMethod]
        public void QuerySelectNewClassGetEnumerator() {
            var testQuery =
                from item in new TestQuery<TestItem>( new TestContext() )
                select new TestItemNew( item.TestPropertySimple );
            foreach ( var item in testQuery )
                ;
        }
        [TestMethod]
        public void QuerySelectNewClassInitializationGetEnumerator() {
            var testQuery =
                from item in new TestQuery<TestItem>( new TestContext() )
                select new TestItemNew {
                    TestPropertySimple = item.TestPropertySimple
                };
            foreach ( var item in testQuery )
                ;
        }
        [TestMethod]
        public void QuerySelectNewAnonimousClassGetEnumerator() {
            var testQuery =
                from item in new TestQuery<TestItem>( new TestContext() )
                select new { Field = item.TestField };
            foreach ( var item in testQuery )
                ;
        }
        [TestMethod]
        public void QueryExecute() {
            var result =
                (
                from item in new TestQuery<TestItem>( new TestContext() )
                select new { Field = item.TestField }
                ).TestExecuteOperator();
            Assert.AreEqual( "TestExecuteOperator", result );
        }
        [TestMethod]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void QueryExecuteInvalidOperationException() {
            ( (IQueryProvider)new TestQuery<TestItem>( new TestContext() ) ).Execute(
                Expression.Constant( 0 )
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryExecuteNotSupportedException() {
            new TestQuery<TestItem>( new TestContext() ).TestExecuteOperator( 0 );
        }
        [TestMethod]
        public void QueryFindProperties() {
            var testQuery =
                from item in new TestQuery<TestItem>( new TestContext() )
                select new {
                    BinaryExpression = item.TestPropertySimple=="123",
                    UnaryExpression = !item.TestField,
                    ConditionalExpression = item.TestField ? item.TestPropertySimple : item.TestPropertySimple,
                    InvocationExpression = item.TestFunc( item.TestPropertySimple ),
                    LambdaExpression = item.TestPropertySimple + item.TestPropertySimple + item.TestPropertySimple,
                    ListInitExpression = new List<string>() { item.TestPropertySimple },
                    MemberInitExpression = new { Field = item.TestPropertySimple },
                    MethodCallExpression = this.TestFunc( item.TestPropertySimple ),
                    NewExpression = new TestItem(),
                    NewArrayExpression = new[] { item.TestPropertySimple },
                    TypeBinaryExpression = item is TestItem
                };
            foreach ( var item in testQuery )
                ;
        }
        private object TestFunc( object argument ) {
            return argument;
        }
    }
}
