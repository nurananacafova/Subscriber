using Microsoft.AspNetCore.Mvc;
using SubscriberService.Models;

namespace SubscriberService.Services;

public interface ISubscriberService
{
    Task<List<SubscriberModel>> GetSubscribers();
    Task<SubscriberModel> GetSubscriberById([FromQuery] int id);
    Task<string> GetLanguage([FromQuery] int id);
    Task<long> PostSubscriber([FromQuery] SubscriberModel subscriber);
    Task<long> UpdateSubscriber([FromQuery] SubscriberModel subscriberModel);
    Task DeleteSubscriber([FromQuery] int id);
}