using System.Text.Json.Serialization;

namespace KanbanApp.API.Contracts.UsersControllers;

public record AuthRequest(
    [property: JsonPropertyName("login")] string Login, // Указываем, что в JSON поле называется "login"
    [property: JsonPropertyName("password")] string Password // Указываем, что в JSON поле называется "password"
);