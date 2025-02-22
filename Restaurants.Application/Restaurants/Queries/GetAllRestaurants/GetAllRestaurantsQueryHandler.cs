using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
    {
        public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants");

            var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(query.SearchPhrase, 
                query.PageSize, 
                query.PageNumber,
                query.SortBy,
                query.SortDirection);

            var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, query.PageSize, query.PageNumber);

            return result;
        }
    }
}
