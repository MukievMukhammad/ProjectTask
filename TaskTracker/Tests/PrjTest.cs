using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TaskTracker;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TaskTracker.Models;
using Task = System.Threading.Tasks.Task;

namespace Tests
{
    public class APIWebApplicationFactory : WebApplicationFactory<Startup>
    {
    }
    
    public class PrjTest
    {
        private APIWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory();
            _client = _factory.CreateClient();
        }
        
        [Test]
        public async Task CreateProject()
        {
            var project = new Project {Name = "My Project"};
            var stringContent = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/Project/Create", stringContent);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}