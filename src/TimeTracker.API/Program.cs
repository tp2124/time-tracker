using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TimeTracker.API.Endpoints;

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
RouteGroupBuilder usersGroup = app.MapGroup($"/users");
usersGroup.MapGet("/", UserEndpoints.GetAllUsers);
usersGroup.MapGet("/enabledWarnings", UserEndpoints.GetUsersWithWarnings);
usersGroup.MapGet("/{id}", UserEndpoints.GetUser);
usersGroup.MapPost("/", UserEndpoints.CreateUser);
usersGroup.MapPut("/{id}", UserEndpoints.UpdateUser);
usersGroup.MapDelete("/{id}", UserEndpoints.DeleteUser);

RouteGroupBuilder eventsGroup = app.MapGroup($"/events");
eventsGroup.MapGet("/", WorkEventEndpoints.GetRecentWorkEvents);
eventsGroup.MapGet("/forUser/{user_id}", WorkEventEndpoints.GetWorkEventsForUser);
eventsGroup.MapGet("/{id}", WorkEventEndpoints.GetWorkEvent);
eventsGroup.MapPost("/", WorkEventEndpoints.CreateUser);
eventsGroup.MapPut("/{id}", WorkEventEndpoints.UpdateUser);
eventsGroup.MapDelete("/{id}", WorkEventEndpoints.DeleteUser);
#endregion

app.Run();