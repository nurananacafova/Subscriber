using Microsoft.AspNetCore.Mvc;
using SubscriberService.Models;
using SubscriberService.Utility;

namespace SubscriberService.Services;

public interface ISubscriberService
{
    Task<List<SubscriberModel>> GetSubscribers();
    Task<SubscriberModel> GetSubscriberById(int id);
    Task<string> GetLanguage(int id);
    Task<long> PostSubscriber(RegisterSubscriber subscriber);
    Task<long> UpdateSubscriber(SubscriberModel subscriberModel);
    Task DeleteSubscriber(int id);
}