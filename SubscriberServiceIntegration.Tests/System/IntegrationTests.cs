using System.Net;
using Newtonsoft.Json;
using SubscriberService.Models;
using WireMock.Server;

namespace SubscriberServiceIntegration.Tests.System;

public class IntegrationTests
{
    [Fact]
    public async Task GetAllSubscribers_ReturnsOk()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");

        var response = await httpClient.GetAsync("/api/Subscribers/GetAllSubscribers");
        var body = await response.Content.ReadAsStringAsync();
        var actualResult = JsonConvert.DeserializeObject<List<SubscriberModel>>(body);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSubscriberById_ReturnsOk()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 6;
        var response = await httpClient.GetAsync($"/api/Subscribers/GetSubscriber?id={subscriberId}");
        var body = await response.Content.ReadAsStringAsync();
        var actualResult = JsonConvert.DeserializeObject<SubscriberModel>(body);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(actualResult.language == "az");
    }

    [Fact]
    public async Task GetSubscriberById_SubscriberDoesNotExist_ReturnsNull()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 22222;
        var response = await httpClient.GetAsync($"/api/Subscribers/GetSubscriber?id={subscriberId}");
        var body = await response.Content.ReadAsStringAsync();
        var actualResult = JsonConvert.DeserializeObject<SubscriberModel>(body);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetLanguageById_ReturnsOk()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 6;
        var response = await httpClient.GetAsync($"/api/Subscribers/GetSubscriber/language?id={subscriberId}");
        var body = await response.Content.ReadAsStringAsync();
        // var actualResult = JsonConvert.DeserializeObject<string>(body);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("az", body);
    }

    [Fact]
    public async Task GetLanguageById_SubscriberDoesNotExist_ReturnsNull()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 6666666;
        var response = await httpClient.GetAsync($"/api/Subscribers/GetSubscriber/language?id={subscriberId}");
        var body = await response.Content.ReadAsStringAsync();
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ProvideSubscriber_ReturnsOk()
    {
        var subscriber = new SubscriberModel()
        {
            language = "az",
            email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2022 - 02 - 02")
        };

        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");

        var response =
            await httpClient.PostAsJsonAsync($"/api/Subscribers/RegisterSubscriber", subscriber);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSubscriber_ReturnsOk()
    {
        var request = new SubscriberModel()
        {
            id = 19,
            language = "az",
            email = "newmailfortests!@gmail.com",
            registration_date = Convert.ToDateTime("2010 - 02 - 02"),
        };

        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");

        var response = await httpClient.PutAsJsonAsync($"/api/Subscribers/UpdateSubscriber", request);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSubscriber_SubscriberDoesNotExist_ThrowsException()
    {
        var request = new SubscriberModel()
        {
            id = 222222222,
            language = "az",
            email = "newmailfortests!@gmail.com",
            registration_date = Convert.ToDateTime("2022 - 02 - 02"),
        };

        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");

        var response = await httpClient.PutAsJsonAsync($"/api/Subscribers/UpdateSubscriber", request);
        var body = await response.Content.ReadAsStringAsync();
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Subscriber not found", body);
    }

    [Fact]
    public async Task RemoveSubscriber_ReturnsOk()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 20;
        var response = await httpClient.DeleteAsync($"/api/Subscribers/RemoveSubscriber?id={subscriberId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RemoveSubscriber_SubscriberDoesNotExist_ThrowsException()
    {
        var server = WireMockServer.Start();
        var httpClient = server.CreateClient();
        httpClient.BaseAddress = new Uri($"http://localhost:{5000}");
        int subscriberId = 12345;

        var response = await httpClient.DeleteAsync($"/api/Subscribers/RemoveSubscriber?id={subscriberId}");
        var result = await response.Content.ReadAsStringAsync();
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Subscriber not found", result);
    }
}