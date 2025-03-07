using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Models;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller responsible for authentication-related operations.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    [SwaggerTag("Authentication & Authorization")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="config">Configuration settings.</param>
        /// <param name="userService">Service for user-related operations.</param>
        public AuthController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <remarks>
        /// **Example Request:**
        /// ```json
        /// {
        ///   "username": "adminuser",
        ///   "password": "password123"
        /// }
        /// ```
        /// </remarks>
        /// <param name="loginRequest">The user's login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [SwaggerOperation(Summary = "User Authentication", Description = "Authenticates a user using username and password. Returns a JWT token for authorization.")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = await _userService.GetUserByUsernameAsync(loginRequest.Username);

            // Verify password using BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var token = GenerateJwtToken(user.Username, user.Role);
            return Ok(new { token });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// **Example Request:**
        /// ```json
        /// {
        ///   "username": "newuser",
        ///   "password": "SecurePass123!",
        ///   "email": "newuser@email.com",
        ///   "role": "guest"
        /// }
        /// ```
        /// </remarks>
        /// <param name="registerRequest">User registration details.</param>
        /// <returns>A success message if registration is successful.</returns>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [SwaggerOperation(Summary = "User Registration", Description = "Registers a new user with a username, password, email, and optional role.")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrWhiteSpace(registerRequest.Username) ||
                string.IsNullOrWhiteSpace(registerRequest.Password) ||
                string.IsNullOrWhiteSpace(registerRequest.Email))
            {
                return BadRequest(new { message = "Username, password, and email are required." });
            }

            var existingUser = await _userService.GetUserByUsernameAsync(registerRequest.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var user = new User
            {
                Username = registerRequest.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password), // Hash password
                Email = registerRequest.Email,
                Role = registerRequest.Role ?? "guest"
            };

            await _userService.CreateUserAsync(user);
            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Generates a JWT token for authenticated users.
        /// </summary>
        /// <param name="username">The username of the authenticated user.</param>
        /// <param name="role">The user's role.</param>
        /// <returns>A JWT token.</returns>
        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("Missing SecretKey in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
