using System;
using System.Linq;
using System.Xml.Xsl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqToolkit.Test {
    using LinqToolkit.SimpleQuery;
    using LinqToolkit.Test.SimpleQuery;
    using System.Xml;
    using System.IO;
    using System.Reflection;

    [TestClass]
    public class SimpleQueryContextExtensionsTests {

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
        public void SimpleQueryOptionsTranslateXslTransform() {
            var testQuery = (TestSimpleQuery<TestItem>)
                (
                from item in new TestSimpleQuery<TestItem>()
                where item.TestPropertySimple=="123" && item.TestPropertySimple.Contains( "123" ) || !item.TestField
                orderby item.TestPropertySimple
                select new TestItem() {
                    TestPropertySimple = item.TestPropertySimple,
                    TestField = item.TestField
                }
                )
                .Distinct()
                .Skip( 10 );
            XslCompiledTransform transform = new XslCompiledTransform();
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "LinqToolkit.Test.Extensions.Sample.xslt" );
            transform.Load( XmlReader.Create( stream ) );
            var result = testQuery.Options.Transform( transform );
            Assert.AreEqual(
                "SELECT DISTINCT TestPropertySimple, TestField\r\nFROM TestItem\r\nORDER BY TestPropertySimple\r\nWHERE (((TestPropertySimpleEqual\"123\")AndAlso(TestPropertySimple.Contains(\"123\")))OrElse(NotTestField))",
                result
                );
        }
    }
}
