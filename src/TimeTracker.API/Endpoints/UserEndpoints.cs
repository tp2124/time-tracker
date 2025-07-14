using Microsoft.EntityFrameworkCore;

namespace TimeTracker.API.Endpoints;

public static class UserEndpoints
{
	public static async Task<IResult> GetAllUsers(TimeTrackerDBContext db)
	{
		return TypedResults.Ok(await db.Users.ToArrayAsync());
	}

	public static async Task<IResult> GetUsersWithWarnings(TimeTrackerDBContext db)
	{
		return TypedResults.Ok(await db.Users.Where(t => t.EndOfDayWarningEnabled).ToListAsync());
	}

	public static async Task<IResult> GetUser(int id, TimeTrackerDBContext db)
	{
		return await db.Users.FindAsync(id)
		is User user
			? TypedResults.Ok(user)
			: TypedResults.NotFound();
	}

	public static async Task<IResult> CreateUser(User user, TimeTrackerDBContext db)
	{
		db.Users.Add(user);
		await db.SaveChangesAsync();

		return TypedResults.Created($"/users/{user.Id}", GetUser(user.Id, db));
	}

	public static async Task<IResult> UpdateUser(int id, User inputUser, TimeTrackerDBContext db)
	{
		var todo = await db.Users.FindAsync(id);

		if (todo is null) return Results.NotFound();

		todo.Name = inputUser.Name;
		todo.EndOfDayWarningEnabled = inputUser.EndOfDayWarningEnabled;

		await db.SaveChangesAsync();

		return TypedResults.NoContent();
	}

	public static async Task<IResult> DeleteUser(int id, TimeTrackerDBContext db)
	{
		if (await db.Users.FindAsync(id) is User user)
		{
			db.Users.Remove(user);
			await db.SaveChangesAsync();
			return TypedResults.NoContent();
		}

		return TypedResults.NotFound();
	}
}
