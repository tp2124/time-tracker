using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

#region Endpoints

app.MapGet("/users", async (TimeTrackerDBContext db) =>
	await db.Users.ToListAsync());
app.MapGet("/users/enabledWarnings", async (TimeTrackerDBContext db) =>
	await db.Users.Where(t => t.EndOfDayWarningEnabled).ToListAsync());

app.MapGet("/users/{id}", async (int id, TimeTrackerDBContext db) =>
	await db.Users.FindAsync(id)
		is User user
			? Results.Ok(user)
			: Results.NotFound());

app.MapPost("/users", async (User user, TimeTrackerDBContext db) =>
{
	db.Users.Add(user);
	await db.SaveChangesAsync();

	return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", async (int id, User inputUser, TimeTrackerDBContext db) =>
{
	var todo = await db.Users.FindAsync(id);

	if (todo is null) return Results.NotFound();

	todo.Name = inputUser.Name;
	todo.EndOfDayWarningEnabled = inputUser.EndOfDayWarningEnabled;

	await db.SaveChangesAsync();

	return Results.NoContent();
});

app.MapDelete("/users/{id}", async (int id, TimeTrackerDBContext db) =>
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

app.Run();