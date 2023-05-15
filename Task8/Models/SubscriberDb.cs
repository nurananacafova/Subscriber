using Microsoft.EntityFrameworkCore;

namespace SubscriberService.Models;

public class SubscriberDb : DbContext
{
    public SubscriberDb(DbContextOptions<SubscriberDb> options)
        : base(options)
    {
    }

    public SubscriberDb()
        : this(new DbContextOptions<SubscriberDb>())
    {
    }

    public virtual DbSet<SubscriberModel> Subscribers { get; set; }
}