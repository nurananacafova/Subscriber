using System.Data;
using Microsoft.EntityFrameworkCore;
using SubscriberService.Models;
using SubscriberService.Utility;

namespace SubscriberService.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ILogger<SubscriberService> _logger;
    private readonly SubscriberDb _subscriberDb;


    public SubscriberService(ILogger<SubscriberService> logger, SubscriberDb subscriberDb)
    {
        _logger = logger;
        _subscriberDb = subscriberDb;
    }

    public async Task<List<SubscriberModel>> GetSubscribers()
    {
        List<SubscriberModel> subscribers;
        _logger.LogInformation("request received: {methodName}", "Get All Subscribers");
        try
        {
            if (_subscriberDb.Subscribers == null)
            {
                _logger.LogInformation("response replied: {methodName}", "Data not found ");
                return null;
            }

            SubscriberMetrics.GetCounts.Inc();
            SubscriberMetrics.RequestCountByMethod.WithLabels("GET").Inc();
            subscribers = await _subscriberDb.Subscribers.ToListAsync();
            foreach (var s in subscribers)
            {
                _logger.LogInformation("response replied: {message} {id} {language} {email} {registrationDate}",
                    "Subscribers successfully get:",
                    $"subscriber id:{s.id}", $"language:{s.language}", $"email:{s.email}",
                    $"registration date:{s.registration_date}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        return subscribers;
    }

    public async Task<SubscriberModel> GetSubscriberById(int id)
    {
        SubscriberModel subscriber;
        try
        {
            _logger.LogInformation("request received: {methodName} {id}", "Get Subscriber By Id:",
                $"subscriber id:{id}");
            if (_subscriberDb.Subscribers == null)
            {
                _logger.LogInformation("response replied: {methodName}", "Data not found ");
                return null;
            }

            subscriber = await _subscriberDb.Subscribers.FirstOrDefaultAsync(x => x.id == id);
            if (subscriber == null)
            {
                return null;
            }

            SubscriberMetrics.GetByIdCount.Inc();
            _logger.LogInformation("response replied: {message} {id} {language} {email} {registrationDate}",
                "Subscriber successfully get:",
                $"subscriber id:{subscriber.id}", $"language:{subscriber.language}", $"email:{subscriber.email}",
                $"registration date:{subscriber.registration_date}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }


        return subscriber;
    }

    public async Task<string> GetLanguage(int id)
    {
        string language;
        try
        {
            _logger.LogInformation("request received: {methodName} {id}", "Get Language By Id:",
                $"subscriber id:{id}");
            if (_subscriberDb.Subscribers == null)
            {
                _logger.LogInformation("response replied: {methodName}", "Data not found ");
                return null;
            }

            var subscriber = await _subscriberDb.Subscribers.FirstOrDefaultAsync(x => x.id == id);
            if (subscriber == null)
            {
                return null;
            }

            language = subscriber.language;


            SubscriberMetrics.GetLanguageCount.Inc();
            _logger.LogInformation("response replied: {message} {language}", "Language successfully get:",
                $"language:{language}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        return (language);
    }

    public async Task<long> PostSubscriber(SubscriberModel subscriber)
    {
        int subscriberId = 0;
        try
        {
            _logger.LogInformation("request received: {methodName} {language} {email} {registrationDate}",
                "Create Subscriber:",
                $"language:{subscriber.language}", $"email:{subscriber.email}",
                $"registration date:{subscriber.registration_date}");
            if (subscriber == null)
            {
                return 0;
            }

            await _subscriberDb.Subscribers.AddAsync(subscriber);
            await _subscriberDb.SaveChangesAsync();
            subscriber = _subscriberDb.Subscribers.OrderBy(x => x.id).LastOrDefault();
            subscriberId = subscriber.id;
            // OrderValueHistogram.Observe(subscriber.TotalValueUsd);
            SubscriberMetrics.InsertCounter.Inc();
            _logger.LogInformation("response replied: {message} {subscriberId} {language} {email} {registrationDate}",
                "Subscriber created:", $"subscriber id:{subscriber.id}",
                $"language:{subscriber.language}", $"email:{subscriber.email}",
                $"registration date:{subscriber.registration_date}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        return subscriberId;
    }

    public async Task<long> UpdateSubscriber(SubscriberModel subscriberModel)
    {
        long id = 0;
        _logger.LogInformation("request received: {methodName} {language} {email}", "Update Subscriber:",
            $"language:{subscriberModel.language}", $"email:{subscriberModel.email}");
        if (!SubscriberAvailable(subscriberModel.id))
        {
            throw new InvalidConstraintException("Subscriber not found");
        }

        _subscriberDb.Subscribers.Update(subscriberModel);
        try
        {
            await _subscriberDb.SaveChangesAsync();
            id = subscriberModel.id;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError(e.Message);
        }

        _logger.LogInformation("response replied: {message} {id} {language} {email} {registrationDate}",
            "Subscriber updated:", $"subscriber id:{subscriberModel.id}", $"language:{subscriberModel.language}",
            $"email:{subscriberModel.email}", $"registration date:{subscriberModel.registration_date}");
        return id;
    }

    private bool SubscriberAvailable(int id)
    {
        return (_subscriberDb.Subscribers?.Any(x => x.id == id)).GetValueOrDefault();
    }

    public async Task DeleteSubscriber(int id)
    {
        try
        {
            _logger.LogInformation("request received: {methodName} {id}", "Delete Subscriber:",
                $"subscriber id:{id}");

            var subscriber = await _subscriberDb.Subscribers.FirstOrDefaultAsync(x => x.id == id);
            if (subscriber == null)
            {
                _logger.LogInformation("response replied: {message}", "Data not found");
                throw new InvalidOperationException("Subscriber not found");
            }

            _subscriberDb.Subscribers.Remove(subscriber);
            await _subscriberDb.SaveChangesAsync();
            SubscriberMetrics.DeleteCounter.Inc();
            _logger.LogInformation("request response: {message}", $"Subscriber {id} successfully deleted");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}