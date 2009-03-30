using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqToolkit.Test {
    using LinqToolkit.Test.Table;

    /// <summary>
    /// Summary description for TableTest
    /// </summary>
    [TestClass]
    public class TableTests {

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
        public void TableConstructor() {
            var result = new TestTable( new[] { new TestItem() }) ;
            Assert.AreEqual( 1, result.Count() );
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableConstructorArgumentNullException() {
            var result = new TestTable();
        }
        [TestMethod]
        public void TableInsert() {
            var result = new TestTable( new TestItem[] { } );
            result.Insert( new TestItem() );
            Assert.AreEqual( 1, result.Count() );
            Assert.AreEqual( 1, result.Inserted.Count() );
            Assert.AreEqual( 0, result.Updated.Count() );
            Assert.AreEqual( 0, result.Deleted.Count() );
        }
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void TableInsertItemDeletedArgumentException() {
            var result = new TestTable( new[] { new TestItem() } );
            var item = result.First();
            result.Delete( item );
            result.Insert( item );
        }
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void TableInsertItemExistsArgumentException() {
            var result = new TestTable( new TestItem[] { } );
            result.Insert( new TestItem() );
            result.Insert( result.First() );
        }
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void TableInsertArgumentNullException() {
            var result = new TestTable( new TestItem[] { } );
            result.Insert( null );
        }
        [TestMethod]
        public void TableUpdate() {
            var result = new TestTable( new[] { new TestItem() } );
            result.First().TestPropertySimple = "AAA";
            Assert.AreEqual( 1, result.Count() );
            Assert.AreEqual( 0, result.Inserted.Count() );
            Assert.AreEqual( 1, result.Updated.Count() );
            Assert.AreEqual( 0, result.Deleted.Count() );
        }
        [TestMethod]
        public void TableDelete() {
            var result = new TestTable( new[] { new TestItem() } );
            result.Delete( result.First() );
            Assert.AreEqual( 0, result.Count() );
            Assert.AreEqual( 0, result.Inserted.Count() );
            Assert.AreEqual( 0, result.Updated.Count() );
            Assert.AreEqual( 1, result.Deleted.Count() );
        }
        [TestMethod]
        public void TableDeleteInserted() {
            var result = new TestTable( new TestItem[] { } );
            result.Insert( new TestItem() );
            result.Delete( result.First() );
            Assert.AreEqual( 0, result.Count() );
            Assert.AreEqual( 0, result.Inserted.Count() );
            Assert.AreEqual( 0, result.Updated.Count() );
            Assert.AreEqual( 0, result.Deleted.Count() );
        }
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void TableDeleteArgumentNullException() {
            var result = new TestTable( new TestItem[] { } );
            result.Delete( null );
        }
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void TableDeleteArgumentException() {
            var result = new TestTable( new[] { new TestItem() } );
            var item = result.First();
            result.Delete( item );
            result.Delete( item );
        }
    }
}
