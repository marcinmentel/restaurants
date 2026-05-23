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
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
namespace Restaurant.API.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(IEnumerable<RestaurantDto>))]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }
    [HttpGet("{id}")]
    //[Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
    {
        //var userId = User.Claims.FirstOrDefault(c => c.Type == "<id claim type>")!.Value;
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        
        return Ok(restaurant);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
    [HttpPost]
    [Authorize(Roles =UserRoles.Owner)]
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
        await mediator.Send(restaurantCommand);
        return NoContent();
    }
    [HttpPost("{id}/logo")]
    public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var command = new UploadRestaurantLogoCommand()
        {
            RestaurantId = id,
            FileName = $"{id}-{file.FileName}",
            File = stream
        };

        await mediator.Send(command);
        return NoContent();
    }
}
