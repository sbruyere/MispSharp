using System.Drawing;
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misp;

namespace Misp.Tests
{
    /// <summary>This class contains parameterized unit tests for Tag</summary>
    [TestClass]
    [PexClass(typeof(Tag))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public class TagTest
    {
        public static void AreEqualMin(Tag expected, Tag actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Exportable, actual.Exportable);
        }

        public static void AreEqualFull(Tag expected, Tag actual)
        {
            AreEqualMin(expected, actual);
            Assert.AreEqual(expected.AttributeCount, actual.AttributeCount);
            Assert.AreEqual(expected.EventCount, actual.EventCount);
            Assert.AreEqual(expected.Hidden, actual.Hidden);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.IsFavorite, actual.IsFavorite);
            Assert.AreEqual(expected.OrgOnly, actual.OrgOnly);            
        }

        /// <summary>Test stub for .ctor(String)</summary>
        [PexMethod]
        public Tag ConstructorTest()
        {
            Tag target = new Tag();
            Assert.IsNotNull(target);
            return target;
        }
        
        /// <summary>Test stub for .ctor(String)</summary>
        [PexMethod]
        public Tag ConstructorTest(string name)
        {
            Tag target = new Tag(name);
            Assert.IsNotNull(target);
            Assert.AreEqual(name, target.Name);
            return target;
        }
        
        /// <summary>Test stub for .ctor(String, String, Boolean)</summary>
        [PexMethod]
        public Tag ConstructorTest(string name, string color, bool isExportable)
        {
            Tag target = new Tag(name, color, isExportable);
            Assert.IsNotNull(target);
            Assert.AreEqual(name, target.Name);
            Assert.AreEqual(color, target.Color);
            Assert.AreEqual(isExportable, target.Exportable);
            return target;
        }

        /// <summary>Test stub for .ctor(String, Color, Boolean)</summary>
        [PexMethod]
        public Tag ConstructorTest(string name, Color color, bool isExportable)
        {
            Tag target = new Tag(name, color, isExportable);
            Assert.IsNotNull(target);
            Assert.AreEqual(name, target.Name);
            Assert.AreEqual(Misp.Drawing.ColorTranslator.ToHtml(color), target.Color);
            Assert.AreEqual(isExportable, target.Exportable);
            return target;
        }

        [TestMethod, TestCategory("NoServer")] public void cTor_Empty() { this.ConstructorTest(); }
        [TestMethod, TestCategory("NoServer")] public void cTor_Name() { this.ConstructorTest(TestHelper.RandomString()); }
        [TestMethod, TestCategory("NoServer")] public void cTor_NameColorExport() { this.ConstructorTest(TestHelper.RandomString(), TestHelper.RandomString(), TestHelper.RandomBool()); }
        [TestMethod, TestCategory("NoServer")] public void cTor_NameColor2Export() { this.ConstructorTest(TestHelper.RandomString(), Color.FromArgb(TestHelper.RandomInt(0, Int32.MaxValue)) , TestHelper.RandomBool()); }


        [TestMethod, TestCategory("NoServer")]
        public void Parser_RoundTrip_Simple()
        {
            Tag expected = this.ConstructorTest(TestHelper.RandomString(), TestHelper.RandomString(), TestHelper.RandomBool());
            String json = expected.ToString();
            Tag actual = Tag.FromJson(json);
            AreEqualMin(expected, actual);
        }


        [TestMethod, TestCategory("NoServer")]
        public void WrapperParser_RoundTrip_Simple()
        {
            TagWrapper expected = new TagWrapper(new Tag[] { this.ConstructorTest(TestHelper.RandomString(), TestHelper.RandomString(), TestHelper.RandomBool()) });
            String json = expected.ToString();
            TagWrapper actual = TagWrapper.FromJson(json);
            Assert.AreEqual(expected.Tags.Length, actual.Tags.Length);
            AreEqualMin(expected.Tags[0], actual.Tags[0]);
        }


    }
}
