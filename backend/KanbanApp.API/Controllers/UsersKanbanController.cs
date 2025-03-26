using KanbanApp.API.Contracts.UsersControllers;
using KanbanApp.Core.Abstractions.IUsers;
using KanbanApp.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Добавляем логирование

namespace KanbanApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersKanbanController : ControllerBase
    {
        private readonly IUsersKanbanService _usersService;
        private readonly ILogger<UsersKanbanController> _logger; // Добавляем ILogger

        public UsersKanbanController(IUsersKanbanService usersService, ILogger<UsersKanbanController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        // Метод для получения всех пользователей
        [HttpGet]
        public async Task<ActionResult<List<UsersKanbanResponse>>> GetUsers()
        {
            _logger.LogInformation("Received GET request to fetch all users.");
            var users = await _usersService.GetAllUsers();
            var response = users.Select(b => new UsersKanbanResponse(b.Id, b.Name, b.Login, b.Password));
            _logger.LogInformation("Successfully fetched {UserCount} users.", users.Count);
            return Ok(response);
        }

        // Метод для создания нового пользователя
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersKanbanRequest request)
        {
            _logger.LogInformation("Received POST request to create user with login: {Login}.", request.Login);
            (UserKanban? user, string error) = UserKanban.Create(
                Guid.NewGuid(),
                request.Name,
                request.Login,
                request.Password);

            if (user == null)
            {
                _logger.LogWarning("Failed to create user: {Error}.", error);
                return BadRequest(error);
            }

            var userId = await _usersService.CreateUser(user);
            _logger.LogInformation("Successfully created user with ID: {UserId}.", userId);
            return Ok(userId);
        }

        // Метод для авторизации пользователя
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            _logger.LogInformation("Received POST request to authenticate user with login: {Login}.", request?.Login);
            if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Authentication failed: Login or password is empty.");
                return BadRequest("Login and password are required.");
            }

            var user = await _usersService.Authenticate(request.Login, request.Password);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: Invalid login or password for login: {Login}.", request.Login);
                return Unauthorized("Invalid login or password.");
            }

            _logger.LogInformation("Successfully authenticated user with ID: {UserId}.", user.Id);
            return Ok(user.Id);
        }

        // Метод для обновления данных пользователя
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateUsers(Guid id, [FromBody] UsersKanbanRequest request)
        {
            _logger.LogInformation("Received PUT request to update user with ID: {Id}.", id);
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Update failed: Name, Login, or Password is empty for user ID: {Id}.", id);
                return BadRequest("Имя, Логин, Пароль обязательны для заполнения");
            }

            var userId = await _usersService.UpdateUser(id, request.Name, request.Login, request.Password);
            _logger.LogInformation("Successfully updated user with ID: {UserId}.", userId);
            return Ok(userId);
        }

        // Метод для удаления пользователя
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteUser(Guid id)
        {
            _logger.LogInformation("Received DELETE request to delete user with ID: {Id}.", id);
            var userId = await _usersService.DeleteUser(id);
            _logger.LogInformation("Successfully deleted user with ID: {UserId}.", userId);
            return Ok(userId);
        }
    }
}