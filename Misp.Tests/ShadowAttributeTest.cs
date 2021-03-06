using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misp;

namespace Misp.Tests
{
    /// <summary>This class contains parameterized unit tests for ShadowAttribute</summary>
    [TestClass]
    [PexClass(typeof(ShadowAttribute))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ShadowAttributeTest
    {

        public static void AreEqualMinimum(ShadowAttribute expected, ShadowAttribute actual)
        {
            AttributeTest.AreEqualMinimum(expected, actual);
        }

        public static void AreEqualFull(ShadowAttribute expected, ShadowAttribute actual)
        {
            AreEqualMinimum(expected, actual);
            AttributeTest.AreEqualFull(expected, actual);

            Assert.AreEqual(expected.OldId, actual.OldId);
            Assert.AreEqual(expected.OrgId, actual.OrgId);
            Assert.AreEqual(expected.ProposalToDelete, actual.ProposalToDelete);
            Assert.AreEqual(expected.Deleted, actual.Deleted);

            if (expected.Org != null)
            {
                Assert.IsNotNull(actual.Org);
                Assert.AreEqual(expected.Org.Id, actual.Org.Id);
                Assert.AreEqual(expected.Org.Name, actual.Org.Name);
                Assert.AreEqual(expected.Org.UUID, actual.Org.UUID);
            }
        }

        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public ShadowAttribute ConstructorTest()
        {
            ShadowAttribute target = new ShadowAttribute();

            Assert.IsNotNull(target);
            Assert.AreEqual("5", target.Distribution);
            return target;
        }

        [TestMethod, TestCategory("NoServer")]
        public void cTor_Basic() { this.ConstructorTest(); }

        [TestMethod, TestCategory("NoServer")]
        public void Parser_RoundTrip_Simple()
        {
            ShadowAttribute expected = this.ConstructorTest();
            expected.Category = TestHelper.RandomString();
            expected.Type = TestHelper.RandomString();
            expected.Value = TestHelper.RandomString();
            expected.ToIDS = TestHelper.RandomBool();
            String json = expected.ToString();
            ShadowAttribute actual = ShadowAttribute.FromJson(json);
            AreEqualMinimum(expected, actual);
        }
        [TestMethod, TestCategory("NoServer")]
        public void Parser_RoundTrip_Full()
        {
            ShadowAttribute expected = this.ConstructorTest();
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

            expected.OldId = TestHelper.RandomString();
            expected.OrgId = TestHelper.RandomString();
            expected.ProposalToDelete = TestHelper.RandomBool();
            expected.Deleted = TestHelper.RandomBool();
            expected.Org = new Org()
            {
                Id = expected.OrgId,
                Name = TestHelper.RandomString(),
                UUID = Guid.NewGuid()
            };

            String json = expected.ToString();
            ShadowAttribute actual = ShadowAttribute.FromJson(json);
            AreEqualMinimum(expected, actual);
        }
    }
}
