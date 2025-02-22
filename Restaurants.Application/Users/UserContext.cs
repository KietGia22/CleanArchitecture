using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            string userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            string email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

            string? nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;

            string? dateofBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;

            DateOnly? dateOfBirth = dateofBirthString == null
                ? (DateOnly?)null
                : DateOnly.ParseExact(dateofBirthString, "yyyy-MM-dd");

            return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
        }
    }
}
