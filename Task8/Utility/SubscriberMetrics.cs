using Prometheus;

namespace SubscriberService.Utility;

public class SubscriberMetrics
{
    public static readonly Counter GetCounts = Metrics
        .CreateCounter("get_counter", "Get method's counter");

    public static readonly Counter RequestCountByMethod = Metrics
        .CreateCounter("requests_total", "Number of requests received, by HTTP method.",
            labelNames: new[] { "method" });

    public static Histogram _responseTimeHistogram = Metrics.CreateHistogram("request_duration_seconds",
        "The duration in seconds between the response to a request.", new HistogramConfiguration
        {
            Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
            LabelNames = new[] { "status_code", "method" }
        });

    public static readonly Counter GetByIdCount = Metrics
        .CreateCounter("get_by_id_counter", "GetById method's counter");

    public static readonly Counter GetLanguageCount = Metrics
        .CreateCounter("get_language_counter", "Get method's counter for get language");

    public static readonly Counter InsertCounter = Metrics
        .CreateCounter("insert_counter", "Post method's counter");

    public static readonly Counter UpdateCounter = Metrics
        .CreateCounter("update_counter", "Put method's counter");

    public static readonly Counter DeleteCounter = Metrics
        .CreateCounter("delete_counter", "Delete method's counter");
}