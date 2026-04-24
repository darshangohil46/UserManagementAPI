using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users.");
            }
        }

        [HttpGet(""{id}"")]
        public IActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0.");

            try
            {
                var user = _service.GetById(id);
                if (user == null) return NotFound($"User with id {id} was not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve user {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user.");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null) return BadRequest("Request body is required.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = _service.Create(user);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
            }
        }

        [HttpPut(""{id}"")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0.");
            if (user == null) return BadRequest("Request body is required.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                if (!_service.Update(id, user)) return NotFound($"User with id {id} was not found.");
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
            }
        }

        [HttpDelete(""{id}"")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0.");

            try
            {
                if (!_service.Delete(id)) return NotFound($"User with id {id} was not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
            }
        }
    }
}
