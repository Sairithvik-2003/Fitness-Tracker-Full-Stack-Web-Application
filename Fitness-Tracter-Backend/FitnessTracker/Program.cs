using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FitnessTrackerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FitnessTrackerDbContext"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFitnessGoalBLRepository, FitnessBLRepository>();
builder.Services.AddScoped<IFitnessGoalRepository, FitnessGoalRepository>();
builder.Services.AddScoped<IUserBLRepository, UserBLRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkoutBLRepository, WorkoutBLRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors("AllowAll"); // Apply the CORS policy here

app.MapControllers();

app.Run();