using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misp;

namespace Misp.Tests
{
    /// <summary>This class contains parameterized unit tests for Attribute</summary>
    [TestClass]
    [PexClass(typeof(Attribute))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class AttributeTest
    {

        public static void AreEqualMinimum(Attribute expected, Attribute actual)
        {
            Assert.AreEqual(expected.Category, actual.Category);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(expected.ToIDS, actual.ToIDS);
        }

        public static void AreEqualFull(Attribute expected, Attribute actual)
        {
            AreEqualMinimum(expected, actual);
            Assert.AreEqual(expected.Comment, actual.Comment);
            Assert.AreEqual(expected.Data, actual.Data);
            Assert.AreEqual(expected.DisableCorrelation, actual.DisableCorrelation);
            Assert.AreEqual(expected.Distribution, actual.Distribution);
            Assert.AreEqual(expected.EventId, actual.EventId);
            //Assert.AreEqual(expected.RelatedAttribute, actual.RelatedAttribute);
            //Assert.AreEqual(expected.ShadowAttribute, actual.ShadowAttribute);
            //Assert.AreEqual(expected.ShadowGroup, actual.ShadowGroup);
            //Assert.AreEqual(expected.SharingGroupId, actual.SharingGroupId);
            Assert.AreEqual(expected.Timestamp, actual.Timestamp);
            Assert.AreEqual(expected.UUID, actual.UUID);
        }

        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public Attribute ConstructorTest()
        {
            Attribute target = new Attribute();

            Assert.IsNotNull(target);
            Assert.AreEqual("5", target.Distribution);

            return target;
        }

        [TestMethod, TestCategory("NoServer")]
        public void cTor_Basic()
        {
            this.ConstructorTest();
        }

        [TestMethod, TestCategory("NoServer")]
        public void Parser_RoundTrip_Simple()
        {
            Attribute expected = this.ConstructorTest();
            expected.Category = TestHelper.RandomString();
            expected.Type = TestHelper.RandomString();
            expected.Value = TestHelper.RandomString();
            expected.ToIDS = TestHelper.RandomBool();
            String expectedStr = expected.ToString();
            Attribute actual = Attribute.FromJson(expectedStr);
            AttributeTest.AreEqualMinimum(expected, actual);
        }

        [TestMethod, TestCategory("NoServer")]
        public void Parser_RoundTrip_Full()
        {
            Attribute expected = this.ConstructorTest();
            expected.Category = TestHelper.RandomString();
            expected.Type = TestHelper.RandomString();
            expected.Value = TestHelper.RandomString();
            expected.ToIDS = TestHelper.RandomBool();
            expected.Comment = TestHelper.RandomSentance();
            expected.DisableCorrelation = TestHelper.RandomBool();
            expected.Distribution = TestHelper.RandomInt(0, 5).ToString();
            expected.UUID = Guid.NewGuid();
            expected.Timestamp = Misp.Helper.UnixTimestampFromDateTime(DateTime.Now).ToString();
            //expected.RelatedAttribute
            //expected.ShadowAttribute
            //expected.ShadowGroup
            //expected.SharingGroupId

            String expectedStr = expected.ToString();
            Attribute actual = Attribute.FromJson(expectedStr);
            AttributeTest.AreEqualFull(expected, actual);
        }
    }
}
