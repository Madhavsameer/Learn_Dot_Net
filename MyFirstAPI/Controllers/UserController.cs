using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.Models;
using MyFirstAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MyFirstAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        private readonly MongoDbService _mongoService;

        public UserController(UserDbContext dbContext, MongoDbService mongoService)
        {
            _dbContext = dbContext;
            _mongoService = mongoService;
        }

        // ✅ GET all users from SQL
        [HttpGet("sql")]
        public async Task<IActionResult> GetSqlUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return Ok(users);
        }

        // ✅ GET all users from MongoDB
        [HttpGet("mongo")]
        public async Task<IActionResult> GetMongoUsers()
        {
            var users = await _mongoService.GetUsersAsync();
            return Ok(users);
        }

        // ✅ POST user to SQL Server
        [HttpPost("sql")]
        public async Task<IActionResult> CreateSqlUser([FromBody] User newUser)
        {
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSqlUsers), new { id = newUser.Id }, newUser);
        }

        // ✅ POST user to MongoDB
        [HttpPost("mongo")]
        public async Task<IActionResult> CreateMongoUser([FromBody] User newUser)
        {
            await _mongoService.AddUserAsync(newUser);
            return CreatedAtAction(nameof(GetMongoUsers), new { id = newUser.Id }, newUser);
        }

        // ✅ PUT update user in SQL Server
        [HttpPut("sql/{id}")]
        public async Task<IActionResult> UpdateSqlUser(int id, [FromBody] User updatedUser)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            await _dbContext.SaveChangesAsync();
            return Ok(user);
        }

        // ✅ PUT update user in MongoDB
        [HttpPut("mongo/{id}")]
        public async Task<IActionResult> UpdateMongoUser(int id, [FromBody] User updatedUser)
        {
            await _mongoService.UpdateUserAsync(updatedUser);
            return Ok(updatedUser);
        }

        // ✅ DELETE user from SQL Server
        [HttpDelete("sql/{id}")]
        public async Task<IActionResult> DeleteSqlUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return NotFound();

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return Ok($"User with ID {id} deleted from SQL Server.");
        }

        // ✅ DELETE user from MongoDB
        [HttpDelete("mongo/{id}")]
        public async Task<IActionResult> DeleteMongoUser(int id)
        {
            await _mongoService.DeleteUserAsync(id);
            return Ok($"User with ID {id} deleted from MongoDB.");
        }
    }
}
