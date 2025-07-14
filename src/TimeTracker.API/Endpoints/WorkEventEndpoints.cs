using Microsoft.EntityFrameworkCore;
using TimeTracker.API.DTOs;

namespace TimeTracker.API.Endpoints;

public static class WorkEventEndpoints
{
	public static async Task<IResult> GetRecentWorkEvents(TimeTrackerDBContext db)
	{
		return TypedResults.Ok(await db.WorkEvents.ToArrayAsync());
	}

	public static async Task<IResult> GetWorkEventsForUser(int userId, TimeTrackerDBContext db)
	{
		return TypedResults.Ok(await db.WorkEvents.Where(t => t.UserId == userId).ToListAsync());
	}

	public static async Task<IResult> GetWorkEvent(int id, TimeTrackerDBContext db)
	{
		return await db.WorkEvents.FindAsync(id)
		is WorkEvent workEvent
			? TypedResults.Ok(workEvent)
			: TypedResults.NotFound();
	}

	public static async Task<IResult> CreateUser(CreateWorkEventDTO workEventDTO, TimeTrackerDBContext db)
	{
		var user = await db.Users.FindAsync(workEventDTO.UserId);
		if (user is null) return Results.BadRequest("Associated user not found");
		WorkEvent workEvent = new WorkEvent
		{
			Description = workEventDTO.Description,
			Timestamp = workEventDTO.Timestamp,
			UserId = workEventDTO.UserId,
			User = user
		};
		db.WorkEvents.Add(workEvent);
		await db.SaveChangesAsync();

		return TypedResults.Created($"/events/{workEvent.Id}", workEvent.Id);
	}

	public static async Task<IResult> UpdateUser(int id, CreateWorkEventDTO inputWorkEventDTO, TimeTrackerDBContext db)
	{
		WorkEvent? workEvent = await db.WorkEvents.FindAsync(id);

		if (workEvent is null) return Results.NotFound();

		workEvent.Description = inputWorkEventDTO.Description;
		workEvent.Timestamp = inputWorkEventDTO.Timestamp;

		await db.SaveChangesAsync();

		return TypedResults.NoContent();
	}

	public static async Task<IResult> DeleteUser(int id, TimeTrackerDBContext db)
	{
		if (await db.WorkEvents.FindAsync(id) is WorkEvent workEvent)
		{
			db.WorkEvents.Remove(workEvent);
			await db.SaveChangesAsync();
			return TypedResults.NoContent();
		}

		return TypedResults.NotFound();
	}
}
