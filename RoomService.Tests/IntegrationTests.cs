using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using RoomService.WebAPI;
using RoomService.WebAPI.Data;
using RoomService.WebAPI.SeedData;
using Xunit;

namespace RoomService.Tests
{
    public class IntegrationTests
    {
        private TestServer _server;

        public HttpClient Client { get; private set; }

        public IntegrationTests()
        {
            SetUpClient();
        }

        private async Task SeedData()
        {
            Client.DefaultRequestHeaders.Add("passwordKey", "passwordKey123456789");

            var createForm0 = GenerateCreateForm("Room Category 1", 523, 5, DateTime.Parse("02.01.2019"));
            var response0 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            var createForm1 = GenerateCreateForm("Room Category 2", 512, 5, DateTime.Parse("03.05.2020"));
            var response1 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            var createForm2 = GenerateCreateForm("Room Category 3", 332, 3, DateTime.Parse("12.04.2018"));
            var response2 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            var createForm3 = GenerateCreateForm("Room Category 4", 123, 1, DateTime.Parse("06.11.2019"));
            var response3 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));
       
            var createForm4 = GenerateCreateForm("Room Category 5", 573, 5, DateTime.Parse("03.01.2020"));
            var response4 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm4), Encoding.UTF8, "application/json"));
       
            var createForm5 = GenerateCreateForm("Room Category 6", 632, 6, DateTime.Parse("06.12.2018"));
            var response5 = await Client.PostAsync("/api/rooms", new StringContent(JsonConvert.SerializeObject(createForm5), Encoding.UTF8, "application/json"));
        }

        private RoomForm GenerateCreateForm(string category, int number, int floor, DateTime publishedDate)
        {
            return new RoomForm
            {
                Category = category,
                Floor = floor,
                Number = number,
                AddedDate = publishedDate
            };
        }

        [Fact]
        public async Task Test1()
        {
            await SeedData();

            var response0 = await Client.GetAsync("/api/rooms");
            response0.StatusCode.Should().BeEquivalentTo(200);
            var rooms = JsonConvert.DeserializeObject<IEnumerable<Room>>(response0.Content.ReadAsStringAsync().Result);
            rooms.Count().Should().Be(6);

            Client.DefaultRequestHeaders.Clear();
            var response1 = await Client.GetAsync("/api/rooms");
            response1.StatusCode.Should().BeEquivalentTo(403);
            var rooms2 = JsonConvert.DeserializeObject<IEnumerable<Room>>(response1.Content.ReadAsStringAsync().Result);
            rooms2.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Test2()
        {
            await SeedData();

            var response0 = await Client.GetAsync("/api/rooms/1");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var room = JsonConvert.DeserializeObject<Room>(response0.Content.ReadAsStringAsync().Result);
            room.Category.Should().Be("Room Category 1");
            room.Number.Should().Be(523);

            var response1 = await Client.GetAsync("/api/rooms/101");
            response1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);

            Client.DefaultRequestHeaders.Clear();
            var response2 = await Client.GetAsync("/api/rooms/101");
            response2.StatusCode.Should().BeEquivalentTo(403);
        }

        [Fact]
        public async Task Test3()
        {
            await SeedData();

            var response1 = await Client.GetAsync("/api/rooms?Floors=5&Floors=6");
            response1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status200OK);
            var filteredRooms = JsonConvert.DeserializeObject<IEnumerable<Room>>(response1.Content.ReadAsStringAsync().Result).ToArray();
            filteredRooms.Length.Should().Be(4);
            filteredRooms.Where(x => x.Floor == 5).ToArray().Length.Should().Be(3);
            filteredRooms.Where(x => x.Floor == 6).ToArray().Length.Should().Be(1);

            Client.DefaultRequestHeaders.Clear();
            var response2 = await Client.GetAsync("/api/rooms?Floors=5&Floors=6");
            response2.StatusCode.Should().BeEquivalentTo(403);
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var context = new RoomContext(new DbContextOptionsBuilder<RoomContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(RoomContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
