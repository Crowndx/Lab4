using Lab4;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public UnitTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [TestMethod]
        public void TestMethod1()
        {

        }
        [TestMethod]
        public void TestMethod2()
        {

        }
        [TestMethod]
        public void TestMethod3()
        {

        }
        [TestMethod]
        public void TestMethod4()
        {

        }
    }
}
