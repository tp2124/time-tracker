public class WorkEvent
{
	public int Id { get; set; }
	public string? Description { get; set; }
	public required DateTime Timestamp { get; set; }

	public int UserId { get; set; }
	public required User User { get; set; }
}