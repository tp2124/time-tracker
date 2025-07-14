namespace TimeTracker.API.DTOs;

public record CreateWorkEventDTO(int UserId, DateTime Timestamp, string? Description);