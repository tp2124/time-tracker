using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

#region Builder Setup
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Swagger setup Start
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
// Swagger setup End
builder.Services.AddDbContext<TimeTrackerDBContext>();
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endregion
#region App Initialize
var app = builder.Build();
// Swagger setup Start
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
// Swagger setup End


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
#endregion

#region Endpoints
#region Users
const string usersUrlBase = "users";
RouteGroupBuilder usersGroup = app.MapGroup($"/{usersUrlBase}");
usersGroup.MapGet("/", async (TimeTrackerDBContext db) =>
	await db.Users.ToListAsync());
usersGroup.MapGet("/enabledWarnings", async (TimeTrackerDBContext db) =>
	await db.Users.Where(t => t.EndOfDayWarningEnabled).ToListAsync());

usersGroup.MapGet("/{id}", async (int id, TimeTrackerDBContext db) =>
	await db.Users.FindAsync(id)
		is User user
			? Results.Ok(user)
			: Results.NotFound());

usersGroup.MapPost("/", async (User user, TimeTrackerDBContext db) =>
{
	db.Users.Add(user);
	await db.SaveChangesAsync();

	return Results.Created($"/{usersUrlBase}/{user.Id}", user);
});

usersGroup.MapPut("/{id}", async (int id, User inputUser, TimeTrackerDBContext db) =>
{
	var todo = await db.Users.FindAsync(id);

	if (todo is null) return Results.NotFound();

	todo.Name = inputUser.Name;
	todo.EndOfDayWarningEnabled = inputUser.EndOfDayWarningEnabled;

	await db.SaveChangesAsync();

	return Results.NoContent();
});

usersGroup.MapDelete("/{id}", async (int id, TimeTrackerDBContext db) =>
{
	if (await db.Users.FindAsync(id) is User user)
	{
		db.Users.Remove(user);
		await db.SaveChangesAsync();
		return Results.NoContent();
	}

	return Results.NotFound();
});
#endregion
#endregion

app.Run();