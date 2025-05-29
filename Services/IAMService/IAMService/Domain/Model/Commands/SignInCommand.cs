namespace IAMService.Domain.Model.Commands;

public record SignInCommand(string Email, string Password);