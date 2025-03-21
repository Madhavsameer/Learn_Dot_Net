using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> Register(User user, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return "User already exists.";

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return GenerateToken(user);
    }

    // public async Task<string> Login(string email, string password)
    // {
    //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    //     if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
    //         return "Invalid credentials.";

    //     return GenerateToken(user);
    // }


    public async Task<UserDto> Login(string email, string password)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        return null; // Return null if authentication fails.

    var token = GenerateToken(user);

    return new UserDto
    {
        Id = user.Id,
        Username = user.Name,
        Email = user.Email,
        Role = user.Role,
        Token = token
    };
}


    private string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}
