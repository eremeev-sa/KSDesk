namespace KanbanApp.API.Contracts.UsersControllers;

public record AuthRequest(
    string Login,
    string Password
);