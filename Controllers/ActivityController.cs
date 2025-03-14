using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Services;

namespace tripAdvisorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ActivityController(ActivityService activityService) : ControllerBase
{
    private readonly ActivityService _activityService = activityService;

    // 🚀 GET: api/Activity
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ActivityDTORead>>> GetActivities()
    {
        return Ok(await _activityService.GetAllActivitiesAsync());
    }

    // 🚀 GET: api/Activity/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActivityById(int id)
    {
        var activity = await _activityService.GetActivityByIdAsync(id);
        return activity == null ? NotFound() : Ok(activity);
    }

    // 🚀 POST: api/Activity
    [HttpPost]
    public async Task<ActionResult> PostActivity(ActivityDTO activityDto)
    {
        await _activityService.CreateActivityAsync(activityDto);
        return CreatedAtAction(nameof(GetActivityById), new { id = activityDto.Name }, activityDto);
    }

    // 🚀 PUT: api/Activity/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActivity(int id, ActivityDTO activityDto)
    {
        var success = await _activityService.UpdateActivityAsync(id, activityDto);
        return success ? NoContent() : NotFound();
    }

    // 🚀 DELETE: api/Activity/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var success = await _activityService.DeleteActivityAsync(id);
        return success ? NoContent() : NotFound();
    }
}
