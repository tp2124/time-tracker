public class User
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required ICollection<DayOfTheWeek> WorkDays { get; set; } = new List<DayOfTheWeek>();
	public required int WorkHoursPerDay { get; set; }
	public int? BreakMinutesIncludedDaily { get; set; }

	// Alerts
	public int? ConcurrentWorkMinutesWarning { get; set; }
	// Make the below into it's own class. Likely able to be able to store number of warnings leading up to it, etc.
	// public ICollection<DateTime> ExpectedWorkStopTimes {get; } = new List<DateTime>();
	public bool EndOfDayWarningEnabled { get; set; } = true;


	public ICollection<WorkEvent> WorkEvents { get; } = new List<WorkEvent>();
}

public enum DayOfTheWeek
{
	Monday,
	Tuesday,
	Wednesday,
	Thursday,
	Friday,
	Saturday,
	Sunday
}