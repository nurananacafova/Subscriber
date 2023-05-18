using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace SubscriberService.Models;

[Table("subscriber", Schema = "dbo")]
public class SubscriberModel
{
    [SwaggerSchema(ReadOnly = true)]
    [Key] public int id { get; set; }
    [Required] public string? language { get; set; }
    [Required] public string? email { get; set; }
    [Required] public DateTime registration_date { get; set; }
}