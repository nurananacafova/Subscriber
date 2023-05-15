using System.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using SubscriberService.Models;
using SubscriberServiceUnit.Tests.MockData;

namespace SubscriberServiceUnit.Tests.System;

public class TestSubscriberService
{
    [Fact]
    public async Task GetSubscribers_SubscribersExist_ReturnsOK()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(SubscribersMockData.AllSubscribers());

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);
        var result = await sut.GetSubscribers();
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        Assert.Equal(2, result.Count);
        Assert.Equal("az", result.ElementAt(0).language);
    }

    [Fact]
    public async Task GetSubscribers_SubscribersDoNotExist_ReturnsNull()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(SubscribersMockData.GetEmptySubscribers());

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);
        var result = await sut.GetSubscribers();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSubscriberById_SubscriberExists_ReturnsOK()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;

        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(SubscribersMockData.AllSubscribers());
        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await sut.GetSubscriberById(id);
        // Assert.IsType<OkObjectResult>(result);
        Assert.NotEmpty(new[] { result });
        // Assert.NotNull(result);
        Assert.Equal(1, result.id);
        Assert.Equal("az", result.language);
    }

    [Fact]
    public async Task GetSubscriberById_SubscriberDoesNotExist_ReturnsNull()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { new SubscriberModel() });
        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);
        var result = await sut.GetSubscriberById(id);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetLanguageById_SubscriberExists_ReturnsOk()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { SubscribersMockData.SubscriberById() });

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await sut.GetLanguage(id);
        Assert.Equal("az", result);
    }

    [Fact]
    public async Task GetLanguageById_SubscriberDoesNotExist_ReturnsNull()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { new SubscriberModel() });

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await sut.GetLanguage(id);
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterSubscriber_ReturnsOK()
    {
        var subscriber = new SubscriberModel()
        {
            id = 1,
            language = "az",
            email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2022-04-04")
        };
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { subscriber });
        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await sut.PostSubscriber(subscriber);

        Assert.Equal(1, result);
    }


    [Fact]
    public async Task DeleteSubscriber_SubscriberExists_ReturnsOk()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;
        var request = SubscribersMockData.SubscriberById();
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { request });
        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);
        await sut.DeleteSubscriber(id);
    }

    [Fact]
    public async Task DeleteSubscriber_SubscriberDoesNotExist_ThrowsException()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        int id = 1;
        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { new SubscriberModel() });
        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);
        var result = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.DeleteSubscriber(id));
        Assert.Equal("Subscriber not found", result.Message);
    }

    [Fact]
    public async Task UpdateSubscriber_SubscriberExists_ReturnsOK()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        var subscriber = SubscribersMockData.SubscriberById();
        var request = new SubscriberModel()
        {
            id = 1, language = "ru", email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2020-03-26")
        };

        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { subscriber });

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await sut.UpdateSubscriber(request);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateSubscriber_SubscriberDoesNotExist_ThrowsException()
    {
        var loggerMock = new Mock<ILogger<SubscriberService.Services.SubscriberService>>();
        var dbMock = new Mock<SubscriberDb>();
        var subscriber = SubscribersMockData.SubscriberById();
        var request = new SubscriberModel()
        {
            id = 111, language = "ru", email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2020-03-26")
        };

        dbMock.Setup(x => x.Subscribers).ReturnsDbSet(new[] { subscriber });

        var sut = new SubscriberService.Services.SubscriberService(loggerMock.Object, dbMock.Object);

        var result = await Assert.ThrowsAsync<InvalidConstraintException>(() => sut.UpdateSubscriber(request));

        Assert.Equal("Subscriber not found", result.Message);
    }
}