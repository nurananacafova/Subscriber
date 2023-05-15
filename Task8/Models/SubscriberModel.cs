using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriberService.Models;

[Table("subscriber", Schema = "dbo")]
public class SubscriberModel
{
    [Key] public int id { get; set; }
    [Required] public string? language { get; set; }
    [Required] public string? email { get; set; }
    [Required] public DateTime registration_date { get; set; }
}