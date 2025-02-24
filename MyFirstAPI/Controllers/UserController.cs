using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.Models;
using MyFirstAPI.Data;
using System.Threading.Tasks;

namespace MyFirstAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly MongoDbService _mongoService;

        public UserController(MongoDbService mongoService)
        {
            _mongoService = mongoService;
        }

        // ✅ GET all users from MongoDB
        [HttpGet("mongo")]
        public async Task<IActionResult> GetMongoUsers()
        {
            var users = await _mongoService.GetUsersAsync();
            return Ok(users);
        }

        // ✅ GET user by ID from MongoDB
        [HttpGet("mongo/{id}")]
        public async Task<IActionResult> GetMongoUserById(int id)
        {
            var user = await _mongoService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found in MongoDB.");
            }
            return Ok(user);
        }

        // ✅ POST user to MongoDB
        [HttpPost("mongo")]
        public async Task<IActionResult> CreateMongoUser([FromBody] User newUser)
        {
            await _mongoService.AddUserAsync(newUser);
            return CreatedAtAction(nameof(GetMongoUserById), new { id = newUser.Id }, newUser);
        }

        // ✅ PUT update user in MongoDB
        [HttpPut("mongo/{id}")]
        public async Task<IActionResult> UpdateMongoUser(int id, [FromBody] User updatedUser)
        {
            var existingUser = await _mongoService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found in MongoDB.");
            }

            updatedUser.Id = id; // Ensure the correct ID is assigned
            await _mongoService.UpdateUserAsync(updatedUser);
            return Ok(updatedUser);
        }

        // ✅ DELETE user from MongoDB
        [HttpDelete("mongo/{id}")]
        public async Task<IActionResult> DeleteMongoUser(int id)
        {
            var existingUser = await _mongoService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found in MongoDB.");
            }

            await _mongoService.DeleteUserAsync(id);
            return Ok($"User with ID {id} deleted from MongoDB.");
        }
    }
}
