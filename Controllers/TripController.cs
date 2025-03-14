using Faker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tripAdvisorAPI.DTO.Trip;
using tripAdvisorAPI.Services;

namespace tripAdvisorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TripController(TripService tripService) : ControllerBase
{
    private readonly TripService _tripService = tripService;

    [HttpGet]
   [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TripDTORead>>> GetTrips()
    {
        var trips = await _tripService.GetAllTripsAsync();
        return Ok(trips);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        var trip = await _tripService.GetTripByIdAsync(id);
        return trip == null ? NotFound() : Ok(trip);
    }

    [HttpGet("/user/{UserId}")]
    public async Task <ActionResult<List<TripDTORead>>> GetTrips(int UserId){
        var trips = await _tripService.GetTripsAsyncByUserId(UserId);
        return Ok(trips);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var result = await _tripService.DeleteTripAsync(id);
        return result ? NoContent() : NotFound();
    }


    [HttpPost]
    public async Task<ActionResult<bool>> CreateTrip(TripDTOCreate tripDto)
    {
        var trip = await _tripService.CreateTripAsync(tripDto);
        if ((bool)!trip)
        {
            return false;
        }
        return true;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, TripDTOCreate tripDto)
    {
        var result = await _tripService.UpdateTripAsync(id, tripDto);
        return result ? NoContent() : NotFound();
    }
}
