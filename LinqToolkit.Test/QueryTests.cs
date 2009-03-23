﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqToolkit.Test {

    /// <summary>
    /// Summary description for QueryTests
    /// </summary>
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
                from item in new TestQuery<TestItem>()
                select item;

            Assert.AreEqual( typeof( TestItem ), testQuery.ElementType );
            Assert.IsInstanceOfType( testQuery.Provider, typeof( TestQuery<TestItem> ) );
            Assert.IsNull( testQuery.Context.Options.Filter );
            Assert.AreEqual( 3, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestField" ) );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestSourceProperty" ) );
        }
        [TestMethod]
        public void QuerySelectCast() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                select (ITestItem)item;

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewClass() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                select new TestItemNew( item.TestPropertySimple );

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewClassInitialization() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                select new TestItemNew {
                    TestPropertySimple = item.TestPropertySimple
                };

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestPropertySimple" ) );
        }
        [TestMethod]
        public void QuerySelectNewAnonimousClass() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                select new { Field = item.TestField };

            Assert.AreEqual( 1, testQuery.Context.Options.PropertiesToRead.Count );
            Assert.IsTrue( testQuery.Context.Options.PropertiesToRead.Contains( "TestField" ) );
        }
        [TestMethod]
        public void QueryWhereBinaryOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                where item.TestPropertySimple=="123"
                select item;

            Assert.AreEqual(
                new TestBinaryOperation( ExpressionType.Equal, "TestPropertySimple", "123" ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void QueryWhereBinaryOperationNotSupportedException() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                where "123"==item.TestPropertySimple
                select item;
        }
        [TestMethod]
        public void QueryWhereUnaryOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                where !( item.TestField )
                select item;

            Assert.AreEqual(
                new TestUnaryOperation( ExpressionType.Not, "TestField" ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        public void QueryWhereMethodCallOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
                where item.TestPropertySimple.Contains( "123" )
                select item;

            Assert.AreEqual(
                new TestMethodCallOperation( typeof( string ).GetMethod( "Contains" ), "TestPropertySimple", new object[] { "123" } ),
                testQuery.Context.Options.Filter
                );
        }
        [TestMethod]
        public void QueryWhereJoinOperation() {

            ITestQuery testQuery = (ITestQuery)
                from item in new TestQuery<TestItem>()
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

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>().Distinct();

            Assert.AreEqual(
                new TestOperator( "Distinct" ),
                testQuery.Context.Operator
                );
        }
        [TestMethod]
        public void QueryBuildOperatorByNameAndPropertyName() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>().OrderBy( item => item.TestPropertySimple );

            Assert.AreEqual(
                new TestOperator( "OrderBy", "TestPropertySimple" ),
                testQuery.Context.Operator
                );
        }
        [TestMethod]
        public void QueryBuildOperatorByNameAndValue() {

            ITestQuery testQuery = (ITestQuery)new TestQuery<TestItem>().Take( 10 );

            Assert.AreEqual(
                new TestOperator( "Take", 10 ),
                testQuery.Context.Operator
                );
        }
    }
}