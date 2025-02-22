using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Delete restaurant with id: {request.Id}");

            var restaurant = await restaurantsRepository.GetByIdAsync( request.Id );

            if (restaurant == null)
                throw new NotFoundException(nameof(restaurant), request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbidException();

            await restaurantsRepository.Delete(restaurant);

        }
    }
}
