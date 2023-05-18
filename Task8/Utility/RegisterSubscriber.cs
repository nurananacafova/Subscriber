using System.ComponentModel.DataAnnotations;

namespace SubscriberService.Utility;

public class RegisterSubscriber
{
    public string language { get; set; }
    public string email { get; set; }
    public DateTime registration_date { get; set; }
}