using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly HotelBookingDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">Database context for managing user data.</param>
        public UserService(HotelBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user object if found; otherwise, null.</returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Creates a new user and saves it to the database.
        /// </summary>
        /// <param name="user">The user object containing registration details.</param>
        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
