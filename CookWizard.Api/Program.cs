using CookWizard.Application.Extensions;
using CookWizard.Infraestructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//dependency injection
builder.Services.AddApplication();
builder.Services.AddInfraestructure();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
