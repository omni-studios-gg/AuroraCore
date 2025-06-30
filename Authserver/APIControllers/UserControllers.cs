using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authserver.Models;
using Authserver.DBContexts;

namespace Authserver.APIControllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(AppDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ✅ REGISTER - POST: /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Attempting to register user: {Username}", request.Username);

        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Missing required fields." });
        }

        bool usernameExists = await _context.Users.AnyAsync(u => u.username == request.Username);
        bool emailExists = await _context.Users.AnyAsync(u => u.email == request.Email);

        if (usernameExists)
            return Conflict(new { message = "Username already exists." });

        if (emailExists)
            return Conflict(new { message = "Email already exists." });

        var user = new User
        {
            username = request.Username,
            password_hash = HashPassword(request.Password),
            email = request.Email,
            os = request.OS,
            last_ip = request.IP,
            created_at = DateTime.UtcNow
        };

        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("✅ User '{Username}' registered.", user.username);
            return Ok(new { message = "User registered", user.id });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Registration error for user: {Username}", request.Username);
            return StatusCode(500, new { message = "Internal server error", detail = e.Message });
        }
    }

    // ✅ LOGIN - POST: /api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == request.Username);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var hashedInput = HashPassword(request.Password);
        if (user.password_hash != hashedInput)
        {
            _logger.LogWarning("Login failed: Incorrect password for {Username}", request.Username);
            return Unauthorized(new { message = "Invalid username or password." });
        }

        user.last_login = DateTime.UtcNow;
        user.last_ip = request.IP;
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Login successful for user: {Username}", user.username);
        return Ok(new { message = "Login successful", user.id, user.username });
    }

    // ✅ GET ALL USERS - for debugging
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // ✅ GET USER BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(user);
    }

    // ✅ DELETE USER
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ✅ SHA256 Password Hash
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // ✅ DTOs
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public string OS { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
    }
}
