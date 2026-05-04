using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;
using MediatR;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
namespace Restaurant.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(IEnumerable<RestaurantDto>))]
    //[Authorize(Policy = PolicyNames.CreatedAtleast2Restaurants)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        if(restaurant is null)
            return NotFound();
        return Ok(restaurant);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
        if (isDeleted)
            return NoContent();
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand  restaurant)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        int id = await mediator.Send(restaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
    [HttpPatch("{idd}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int idd, UpdateRestaurantCommand restaurantCommand)
    {
        restaurantCommand.Id = idd;
        var isUpdated = await mediator.Send(restaurantCommand);
        if (isUpdated)
            return NoContent();
        return NotFound();
    }
}
