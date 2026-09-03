namespace UverTeaServerApp.Shared.Security;

public record LoginResponseDto(string Token, string Username, string Role);
