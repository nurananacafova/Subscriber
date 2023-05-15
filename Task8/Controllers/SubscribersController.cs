using Microsoft.AspNetCore.Mvc;
using SubscriberService.Models;
using SubscriberService.Services;

namespace SubscriberService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscribersController : ControllerBase
{
    private readonly ISubscriberService _subscriberService;

    public SubscribersController(ISubscriberService subscriberService)
    {
        _subscriberService = subscriberService;
    }


    [HttpGet("GetAllSubscribers")]
    public async Task<ActionResult<SubscriberModel>> GetSubscribers()
    {
        List<SubscriberModel> subscribers;
        try
        {
            subscribers = await _subscriberService.GetSubscribers();
            if (subscribers.Count == 0)
            {
                return Ok(null);
            }

            return Ok(subscribers);
        }
        catch (Exception e)
        {
            return new OkObjectResult(new ApiResponse<object>(500,
                $"Exception thrown via processing request. Method: {nameof(this.GetSubscribers)}, Error message: {e.Message}"));
        }
    }


    [HttpGet("GetSubscriber")]
    public async Task<ActionResult<SubscriberModel>> GetSubscriberById([FromQuery] int id)
    {
        try
        {
            SubscriberModel subscriber = await _subscriberService.GetSubscriberById(id);
            if (subscriber == null)
            {
                return Ok(null);
            }

            return Ok(subscriber);
        }
        catch (Exception e)
        {
            return new OkObjectResult(new ApiResponse<object>(500,
                $"Exception thrown via processing request. Method: {nameof(this.GetSubscriberById)}, Error message: {e.Message}"));
        }
    }


    [HttpGet("GetSubscriber/language")]
    public async Task<ActionResult<string>> GetLanguage([FromQuery] int id)
    {
        try
        {
            string language = await _subscriberService.GetLanguage(id);

            if (language == null)
            {
                return Ok(null);
            }

            return Ok(language);
        }
        catch (Exception e)
        {
            return new OkObjectResult(new ApiResponse<object>(500,
                $"Exception thrown via processing request. Method: {nameof(this.GetLanguage)}, Error message: {e.Message}"));
        }
    }


    [HttpPost("RegisterSubscriber")]
    public async Task<ActionResult<long>> PostSubscriber([FromBody] SubscriberModel subscriber)
    {
        try
        {
            long id = await _subscriberService.PostSubscriber(subscriber);
            if (id == 0)
            {
                return Ok(null);
            }

            return Ok(id);
        }
        catch (Exception e)
        {
            return new OkObjectResult(new ApiResponse<object>(500,
                $"Exception thrown via processing request. Method: {nameof(this.PostSubscriber)}, Error message: {e.Message}"));
        }
    }


    [HttpPut("UpdateSubscriber")]
    public async Task<ActionResult<long>> UpdateSubscriber([FromBody] SubscriberModel subscriberModel)
    {
        try
        {
            long id = await _subscriberService.UpdateSubscriber(subscriberModel);
            if (id == 0)
            {
                return Ok(null);
            }

            return Ok(id);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpDelete("RemoveSubscriber")]
    public async Task<IActionResult> DeleteSubscriber([FromQuery] int id)
    {
        try
        {
            await _subscriberService.DeleteSubscriber(id);
            if (id == 0)
            {
                return null;
            }

            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}