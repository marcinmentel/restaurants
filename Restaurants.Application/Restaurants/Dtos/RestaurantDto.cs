

using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool NameHasDelivery { get; set; } = default!;
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public List<DishDto> Dishes { get; set; } = [];

    public static RestaurantDto? FromEntity (Restaurant? r)
    {
        if (r == null) return null;
        return new RestaurantDto
        {
            Category = r.Category,
            Description = r.Description,
            Id = r.Id,
            NameHasDelivery = r.NameHasDelivery,
            City = r.Address?.City,
            Street = r.Address?.Street,
            PostalCode = r.Address?.PostalCode,
            Dishes = r.Dishes.Select(DishDto.FromEntity).ToList()
        };
    }
}
