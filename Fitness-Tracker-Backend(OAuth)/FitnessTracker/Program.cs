using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Configuration;
using Microsoft.OpenApi.Models;
using FitnessTracker;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("OAuth").AddJwtBearer("OAuth", config => {
    var secretBytes = System.Text.Encoding.UTF8.GetBytes(Constants.Secret);//string to binary
    var key = new SymmetricSecurityKey(secretBytes);

    config.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Query.ContainsKey("access_token"))
            {
                context.Token = context.Request.Query["access_token"];
            }
            return Task.CompletedTask;
        }
    };
    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = Constants.Issuer,
        ValidAudience = Constants.Audiance,
        IssuerSigningKey = key,
    };

});

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



builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
