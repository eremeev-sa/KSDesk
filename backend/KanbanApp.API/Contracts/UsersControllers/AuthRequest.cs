using System.Text.Json.Serialization;

namespace KanbanApp.API.Contracts.UsersControllers;

public record AuthRequest(
    string login,
    string password
);