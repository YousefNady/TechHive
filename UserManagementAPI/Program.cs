
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


// 1. Error-handling middleware (First)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error.", details = ex.Message });
    }
});

// 2. Authentication middleware (Next)
app.Use(async (context, next) =>
{
    // check for the presence of an Authorization header (this is a simple check for demonstration purposes)
    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Unauthorized. Token is missing." });
        return;
    }
    await next();
});

// 3. Logging middleware (Last)
app.Use(async (context, next) =>
{
    await next(); // call the next middleware first to ensure we log the response status code
    Console.WriteLine($"[LOG] Method: {context.Request.Method} | Path: {context.Request.Path} | Status: {context.Response.StatusCode}");
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

var users = new List<User>
{
    new User { Id = 1, Name = "Ahmed Ali", Email = "ahmed@techhive.com", Role = "Admin" },
    new User { Id = 2, Name = "Sara Smith", Email = "sara@techhive.com", Role = "Developer" }
};

// --- CRUD Endpoints with Debugging and Validation ---

// GET: retrieve all users with error handling
app.MapGet("/users", () =>
{
    try
    {
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred while retrieving users. [{ex.GetType().Name}] {ex.Message} | TraceId: {Guid.NewGuid()}");
    }
});

// GET: validate and retrieve a user by ID with error handling
app.MapGet("/users/{id}", (int id) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        return user is not null ? Results.Ok(user) : Results.NotFound(new { Message = $"User with ID {id} not found." });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred during lookup. [{ex.GetType().Name}] {ex.Message} | TraceId: {Guid.NewGuid()}");
    }
});

// POST: validate and add a new user with error handling
app.MapPost("/users", (User newUser) =>
{
    try
    {
        // Validation
        if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email))
        {
            return Results.BadRequest(new { Message = "Name and Email are required." });
        }

        if (!newUser.Email.Contains("@"))
        {
            return Results.BadRequest(new { Message = "Invalid email format." });
        }

        newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(newUser);
        return Results.Created($"/users/{newUser.Id}", newUser);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to add the user. [{ex.GetType().Name}] {ex.Message} | TraceId: {Guid.NewGuid()}");
    }
});

// PUT: validate and update a user with error handling
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    try
    {
        var existingUser = users.FirstOrDefault(u => u.Id == id);
        if (existingUser is null) return Results.NotFound(new { Message = "User not found." });

        // Validation
        if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
        {
            return Results.BadRequest(new { Message = "Invalid update data." });
        }

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.Role = updatedUser.Role;

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred during update. [{ex.GetType().Name}] {ex.Message} | TraceId: {Guid.NewGuid()}");
    }
});

// DELETE: delete a user with error handling
app.MapDelete("/users/{id}", (int id) =>
{
    try
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Results.NotFound(new { Message = "User not found." });

        users.Remove(user);
        return Results.Ok(new { Message = $"User {id} deleted successfully." });
    }
    catch (Exception ex)
    {
        return Results.Problem($"User deletion failed. [{ex.GetType().Name}] {ex.Message} | TraceId: {Guid.NewGuid()}");
    }
});
app.Run();

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
