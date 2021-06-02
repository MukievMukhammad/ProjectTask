using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TaskTracker;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskTracker.DB;
using TaskTracker.Models;
using TaskTracker.Models.ViewModels;
using Xunit;
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
        private TaskTrackerDbContext _context;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory();
            _client = _factory.CreateClient();
            _context = new TaskTrackerDbContext();
        }

        private async Task<string> SendPost(string uri, ProjectViewModel project)
        {
            var queryString = new FormUrlEncodedContent(
                JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(project)));
            var result = await _client.PostAsync(uri, queryString);
            return await result.Content.ReadAsStringAsync();
        }

        [Test]
        public async Task CreateProject()
        {
            // Arrange
            var project = new ProjectViewModel {Name = "My Project"};
            
            // Act
            var response = await SendPost("/Project/Create", project);

            // Assert
            Assert.AreEqual("200", 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(response)["statusCode"]);
            Assert.IsNotNull(_context.Set<Project>().FirstOrDefault(p => p.Name == "My Project"));
        }
        
        [Test]
        public async Task CreateProjectWithNullName()
        {
            // Arrange
            var project = new ProjectViewModel();
            
            // Act
            var response = await SendPost("/Project/Create", project);
            
            // Assert
            var fieldInfo = JsonConvert
                .DeserializeObject<ICollection<JObject>>(response)
                .First(o => o["key"].ToString() == "Name");
            Assert.AreEqual("Invalid", fieldInfo["validationState"].ToString());
        }
        
        [Test]
        public async Task CreateProjectCompletedDateLessThanStart()
        {
            // Arrange
            var project = new ProjectViewModel() 
            {
                Name = "My Project", 
                StartDate = new DateTime(2021, 3, 3), 
                CompletionDate = new DateTime(2020, 2, 3)
            };
            
            // Act
            var response = await SendPost("/Project/Create", project);
            
            // Assert
            var fieldInfo = JsonConvert
                .DeserializeObject<ICollection<JObject>>(response)
                .First(o => o["key"].ToString() == "CompletionDate");
            Assert.AreEqual("Invalid", fieldInfo["validationState"].ToString());
        }
        
        [Test]
        public async Task CreateProjectWithNegativePriority()
        {
            // Arrange
            var project = new ProjectViewModel
            {
                Name = "My Project", 
                Priority = -10
            };
            
            // Act
            var response = await SendPost("/Project/Create", project);
            
            // Assert
            var fieldInfo = JsonConvert
                .DeserializeObject<ICollection<JObject>>(response)
                .First(o => o["key"].ToString() == "CompletionDate");
            Assert.AreEqual("Invalid", fieldInfo["validationState"].ToString());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}