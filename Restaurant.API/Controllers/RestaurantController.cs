using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    //[Authorize(Policy = PolicyNames.CreatedAtleast2Restaurants)]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await restaurantsService.GetById(id);
        if(restaurant is null)
            return NotFound();
        return Ok(restaurant);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto  restaurant)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        int id = await restaurantsService.Create(restaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
