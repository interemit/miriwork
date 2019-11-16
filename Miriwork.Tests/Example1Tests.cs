using Example.Simple.Servicemodel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Miriwork.Tests
{
    public class Example1Tests : IClassFixture<WebApplicationFactory<Example.Simple.Startup>>
    {
        private readonly WebApplicationFactory<Example.Simple.Startup> factory;

        public Example1Tests(WebApplicationFactory<Example.Simple.Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/services/SimpleRequest?simplestring=Hello+World");

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(jsonData);
            Assert.Equal("Hello World", simpleResponse.SimpleString);
        }

        [Fact]
        public async Task Put_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            string json = JsonConvert.SerializeObject(new SimpleRequest
                {
                    SimpleString = "Hello World"
                });
            var response = await client.PutAsync("/services/SimpleRequest", new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(jsonData);
            Assert.Equal("Hello World", simpleResponse.SimpleString);
        }

        [Fact]
        public async Task Post_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            string json = JsonConvert.SerializeObject(new SimpleRequest
            {
                SimpleString = "Hello World"
            });
            var response = await client.PostAsync("/services/SimpleRequest", new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(jsonData);
            Assert.Equal("Hello World", simpleResponse.SimpleString);
        }

        [Fact]
        public async Task Delete_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            var response = await client.DeleteAsync("/services/SimpleRequest?simplestring=Hello+World");

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(jsonData);
            Assert.Equal("Hello World", simpleResponse.SimpleString);
        }
    }
}