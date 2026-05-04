using MediatR;
using Restaurants.Application.Restaurants.Dtos;


namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{
    //public string? SearchPhrase { get; set; }
    //public int PageNumber { get; set; }
    //public int PageSize { get; set; }
    //public string? SortBy { get; set; }
    //public SortDirection SortDirection { get; set; }
}
