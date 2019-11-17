using Example.Webhosting.Servicemodel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Miriwork.Tests
{
    public class Example2Tests : IClassFixture<WebApplicationFactory<Example.BoundedContext.Bar.BarStartup>>
    {
        private readonly WebApplicationFactory<Example.BoundedContext.Bar.BarStartup> factory;

        public Example2Tests(WebApplicationFactory<Example.BoundedContext.Bar.BarStartup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/services/BarRequest?stringvalue=Hello+World&intvalue=1");

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var barResponse = JsonConvert.DeserializeObject<BarResponse>(jsonData);
            Assert.Equal("Hello World", barResponse.StringValue);
            Assert.Equal(1, barResponse.IntValue);
        }

        [Fact]
        public async Task Put_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            string json = JsonConvert.SerializeObject(new BarRequest
                {
                    StringValue = "Hello World",
                    IntValue = 1
                });
            var response = await client.PutAsync("/services/BarRequest", new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var barResponse = JsonConvert.DeserializeObject<BarResponse>(jsonData);
            Assert.Equal("Hello World", barResponse.StringValue);
            Assert.Equal(1, barResponse.IntValue);
        }

        [Fact]
        public async Task Post_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            string json = JsonConvert.SerializeObject(new BarRequest
            {
                StringValue = "Hello World",
                IntValue = 1
            });
            var response = await client.PostAsync("/services/BarRequest", new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var barResponse = JsonConvert.DeserializeObject<BarResponse>(jsonData);
            Assert.Equal("Hello World", barResponse.StringValue);
            Assert.Equal(1, barResponse.IntValue);
        }

        [Fact]
        public async Task Delete_ReturnsCorrectResponse()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/services/BarRequest?stringvalue=Hello+World&intvalue=1");

            response.EnsureSuccessStatusCode();
            string jsonData = await response.Content.ReadAsStringAsync();
            var barResponse = JsonConvert.DeserializeObject<BarResponse>(jsonData);
            Assert.Equal("Hello World", barResponse.StringValue);
            Assert.Equal(1, barResponse.IntValue);
        }
    }
}