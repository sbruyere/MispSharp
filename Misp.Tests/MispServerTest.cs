// <copyright file="MispServerTest.cs">Copyright ©  2017</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misp;
using System.Collections.Generic;

namespace Misp.Tests
{
    /// <summary>This class contains parameterized unit tests for MispServer</summary>
    [PexClass(typeof(MispServer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public class MispServerTest
    {
        private MispServer NewServer()
        {
            MispServer server = new MispServer(TestHelper.MispInstance, TestHelper.MispKey);
            server.Init();
            return server;
        }



        /// <summary>Test stub for .ctor(String, String)</summary>
        [PexMethod]
        public MispServer ConstructorTest(string baseurl, string authKey)
        {
            MispServer target = new MispServer(baseurl, authKey);
            return target;
            // TODO: add assertions to method MispServerTest.ConstructorTest(String, String)
        }

        /// <summary>Test stub for Init()</summary>
        [PexMethod]
        public void InitTest([PexAssumeUnderTest]MispServer target)
        {
            target.Init();
        }

        /// <summary>Test stub for GetEvent(String)</summary>
        [PexMethod]
        public MispEvent GetEventTest([PexAssumeUnderTest]MispServer target, string id)
        {
            MispEvent result = target.GetEvent(id);
            return result;
            // TODO: add assertions to method MispServerTest.GetEventTest(MispServer, String)
        }

        /// <summary>Test stub for UpdateEvent(MispEvent)</summary>
        [PexMethod]
        public MispEvent UpdateEventTest([PexAssumeUnderTest]MispServer target, MispEvent evnt)
        {
            var e = target.UpdateEvent(evnt);
            MispEventTest.AreEqualFull(e, evnt);
            return e;
        }

        private void VerifyDeleted([PexAssumeUnderTest]MispServer target, String id)
        {
            try
            {
                var evnt = target.GetEvent(id);
                Assert.Fail("Found something that should have been deleted!");
            } catch (MispServerException e)
            {
                Assert.IsNotNull(e);
                Assert.AreEqual("Not Found", e.Message);
            }
        }

        /// <summary>Test stub for DeleteEvent(String)</summary>
        [PexMethod]
        public void DeleteEventTest([PexAssumeUnderTest]MispServer target, string id)
        {
            target.DeleteEvent(id);
            TestHelper.RemoveEvent(id);
            VerifyDeleted(target, id);
        }

        /// <summary>Test stub for DeleteEvent(MispEvent)</summary>
        [PexMethod]
        public void DeleteEventTest([PexAssumeUnderTest]MispServer target, MispEvent evnt)
        {
            target.DeleteEvent(evnt);
            TestHelper.RemoveEvent(evnt.Id);
            VerifyDeleted(target, evnt.Id);
        }

        /// <summary>Test stub for GetEvents()</summary>
        [PexMethod]
        public MispEvent[] GetEventsTest([PexAssumeUnderTest]MispServer target)
        {
            MispEvent[] result = target.GetEvents();
            return result;
            // TODO: add assertions to method MispServerTest.GetEventsTest(MispServer)
        }

        /// <summary>Test stub for AddEvent(MispEvent)</summary>
        [PexMethod]
        public MispEvent AddEventTest([PexAssumeUnderTest]MispServer target, MispEvent evnt)
        {
            var actual = target.AddEvent(evnt);
            TestHelper.AddEvent(actual.Id);
            MispEventTest.AreEqualMinimum(evnt, actual);
            return actual;
        }

        [TestMethod, ExpectedException(typeof(ArgumentException)), TestCategory("NoServer")]
        public void cTor_Empty() { this.ConstructorTest("", ""); }

        [TestMethod, ExpectedException(typeof(ArgumentException)), TestCategory("NoServer")]
        public void cTor_Null() { this.ConstructorTest(null, null); }

        [TestMethod, TestCategory("NoServer")]
        public void cTor_Basic() { this.ConstructorTest("http://testserver.local", TestHelper.RandomString(20)); }

        [TestMethod, TestCategory("NoServer")]
        public void cTor_BasicSSL() { this.ConstructorTest("https://testserver.local", TestHelper.RandomString(20)); }

        [TestMethod, TestCategory("NoServer")]
        public void cTor_BasicSSLtoIP() { this.ConstructorTest("https://127.0.0.1:8341/mispinstance/", TestHelper.RandomString(20)); }

        [TestMethod, TestCategory("Server")]
        public void Init_Basic()
        {
            MispServer server = new MispServer(TestHelper.MispInstance, TestHelper.MispKey);
            this.InitTest(server);
        }

        #region AddEvent
        [TestMethod, TestCategory("Server")]
        public void AddEvent_Basic()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Test Event " + TestHelper.RandomString(10);
            AddEventTest(server, evnt);
        }

        [TestMethod, TestCategory("Server")]
        public void AddEvent_SimpleAttributes()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Test Event with Attribute " + TestHelper.RandomString(10);
            List<Attribute> attrs = new List<Attribute>();
            Attribute a = new Attribute();
            a.Type = "comment";
            a.Category = server.GetDefaultCategoryForType(a.Type);
            a.Value = TestHelper.RandomSentance();
            a.ToIDS = false;
            attrs.Add(a);
            evnt.Attribute = attrs.ToArray();
            AddEventTest(server, evnt);
        }

        [TestMethod, TestCategory("Server")]
        public void AddEvent_WithTag_existing()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Test Event with existing Tag " + TestHelper.RandomString(10);
            List<Tag> list = new List<Tag>();
            Tag t = new Tag();
            t.Name = TestHelper.RandomFromList(server.GetAvailableTagNames());
            list.Add(t);
            evnt.Tag = list.ToArray();
            AddEventTest(server, evnt);
        }

        [TestMethod, TestCategory("Server")]
        public void AddEvent_WithTag_new()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Test Event with new Tag " + TestHelper.RandomString(10);
            List<Tag> list = new List<Tag>();
            Tag t = new Tag();
            t.Name = TestHelper.RandomString(5);
            t.Color = "#" + TestHelper.RandomHexString(6);
            t.Exportable = false;
            list.Add(t);
            evnt.Tag = list.ToArray();
            AddEventTest(server, evnt);
        }

        [TestMethod, TestCategory("Server")]
        public void AddEvent_SemiRealWorld()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Fake Malware " + TestHelper.RandomString(10);
            evnt.AnalysisLevel = AnalysisLevel.Complete;
            evnt.Distribution = "3"; //AllCommunities                        
            evnt.Tag = new Tag[] { new Tag("TestMalware", "#FF4500", true) };

            String testFile = @"c:\windows\system32\calc.exe";
            System.IO.FileInfo nfo = new System.IO.FileInfo(testFile);
            String md5 = TestHelper.GetHash(testFile, "md5");
            String sha1 = TestHelper.GetHash(testFile, "sha1");
            String sha256 = TestHelper.GetHash(testFile, "sha256");

            List<Attribute> attrs = new List<Attribute>();
            attrs.Add(new Attribute("md5", server.GetDefaultCategoryForType("md5"), md5, true));
            attrs.Add(new Attribute("sha1", server.GetDefaultCategoryForType("sha1"), sha1, true));
            attrs.Add(new Attribute("sha256", server.GetDefaultCategoryForType("sha256"), sha256, true));
            attrs.Add(new Attribute("filename", server.GetDefaultCategoryForType("filename"), nfo.Name));
            evnt.Attribute = attrs.ToArray();

            AddEventTest(server, evnt);
        }

        [TestMethod, TestCategory("Server")]
        public void AddEvent_TwoRelated()
        {
            String commonValue = TestHelper.RandomString(16);

            MispServer server = this.NewServer();
            MispEvent evnt1 = new MispEvent();
            evnt1.Date = DateTime.Today;
            evnt1.Info = "Test Event Related 1 " + TestHelper.RandomString(10);
            evnt1.Attribute = new Attribute[] { new Attribute("other", "Payload delivery", commonValue, true) };

            var real1 = AddEventTest(server, evnt1);

            MispEvent evnt2 = new MispEvent();
            evnt2.Date = DateTime.Today;
            evnt2.Info = "Test Event Related 2 " + TestHelper.RandomString(10);
            evnt2.Attribute = new Attribute[] { new Attribute("other", "Payload delivery", commonValue, true) };

            var real2 = AddEventTest(server, evnt2);

            Assert.AreEqual(1, real2.RelatedEvent.Length);
            Assert.AreEqual(real1.Id, real2.RelatedEvent[0].Event.Id);

        }
        #endregion AddEvent

        #region Edit Event

        [TestMethod, TestCategory("Server")]
        public void EditEvent_Simple()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.UtcNow;
            evnt.Timestamp = DateTime.UtcNow;
            evnt.Info = "Test Edit Event " + TestHelper.RandomString(10);
            evnt.AnalysisLevel = AnalysisLevel.Initial;
            evnt.Distribution = ((int)Distribution.YourOrganizationOnly).ToString();
            evnt = AddEventTest(server, evnt);

            Assert.AreEqual(AnalysisLevel.Initial, evnt.AnalysisLevel);

            System.Threading.Thread.Sleep(1000);

            evnt.Timestamp = DateTime.UtcNow;
            evnt.AnalysisLevel = AnalysisLevel.Ongoing;
            List<Attribute> attrs = new List<Attribute>();
            attrs.Add(new Attribute("url", "Network activity", TestHelper.RandomUrl(), false));
            attrs.Add(new Attribute("email-src", "Payload delivery", TestHelper.RandomEmail(), true));
            attrs.Add(new Attribute("malware-type", "Payload delivery", "fake", true));
            evnt.Attribute = attrs.ToArray();

            var evntNew = this.UpdateEventTest(server, evnt);
            MispEventTest.AreEqualFull(evnt, evntNew);            
        }

        [TestMethod, TestCategory("Server"), ExpectedException(typeof(Misp.MispServerException))]
        public void EditEvent_NoChanges()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Now;
            evnt.Info = "Test Edit Event " + TestHelper.RandomString(10);
            evnt.AnalysisLevel = AnalysisLevel.Initial;
            evnt.Distribution = ((int)Distribution.YourOrganizationOnly).ToString();
            evnt = AddEventTest(server, evnt);

            this.UpdateEventTest(server, evnt);
        }
        #endregion Edit Event

        #region Delete Event

        [TestMethod, TestCategory("Server")]
        public void DeleteEvent_Simple()
        {
            MispServer server = this.NewServer();
            MispEvent evnt = new MispEvent();
            evnt.Date = DateTime.Today;
            evnt.Info = "Test Delete Event " + TestHelper.RandomString(10);
            evnt.AnalysisLevel = AnalysisLevel.Initial;
            evnt.Distribution = ((int)Distribution.YourOrganizationOnly).ToString();
            evnt = AddEventTest(server, evnt);

            this.DeleteEventTest(server, evnt);
        }

        //[TestMethod, TestCategory("Server")]
        //public void DeleteEvent_BulkDelete()
        //{
        //    MispServer server = this.NewServer();
        //    //for (int i = 1; i < ??; i++)
        //    //{
        //    //    server.DeleteEvent(i.ToString())
        //    //}
        //    foreach(MispEvent evnt in server.GetEvents())
        //    {
        //       server.DeleteEvent(evnt);
        //    }

        //    MispEvent[] results = server.GetEvents();
        //    Assert.AreEqual(0, results.Length);
        //}
        #endregion Delete Event

    }
}
