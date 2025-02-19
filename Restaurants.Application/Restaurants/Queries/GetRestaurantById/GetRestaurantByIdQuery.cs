using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto>
    {
        public int Id { get; } = Id;
    }
}
