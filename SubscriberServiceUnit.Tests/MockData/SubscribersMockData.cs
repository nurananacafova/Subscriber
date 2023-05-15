using SubscriberService.Models;

namespace SubscriberServiceUnit.Tests.MockData;

public class SubscribersMockData
{
    public static List<SubscriberModel> AllSubscribers()
    {
        return new List<SubscriberModel>
        {
            new SubscriberModel()
            {
                id = 1,
                language = "az",
                email = "newmailfortests1@gmail.com",
                registration_date = Convert.ToDateTime("2020-03-26")
            },
            new SubscriberModel()
            {
                id = 2,
                language = "tr",
                email = "newmailfortests1@gmail.com",
                registration_date = Convert.ToDateTime("2021-04-1")
            },
        };
    }

    public static SubscriberModel SubscriberById()
    {
        return new SubscriberModel()
        {
            id = 1,
            language = "az",
            email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2020-03-26")
        };
    }

    public static List<SubscriberModel> GetEmptySubscribers()
    {
        return new List<SubscriberModel>();
    }

    public static SubscriberModel NewSubscriber()
    {
        return new SubscriberModel()
        {
            language = "az",
            email = "newmailfortests1@gmail.com",
            registration_date = Convert.ToDateTime("2020-03-26")
        };
    }
}