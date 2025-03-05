using HotelBooking.Domain.Entities;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    /// <summary>
    /// Service interface for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="user">The user object containing registration details.</param>
        Task CreateUserAsync(User user);
    }
}
