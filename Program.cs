using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User_API.Data;
using User_API.DataAccessObject;
using User_API.PasswordHandler;
using User_API.Services;
using MySqlConnector;
using System;
using Microsoft.OpenApi.Models;
using User_API.ExceptionHandlerMiddleware;
using User_API.Filter;
using Serilog;
using User_API.Configurations;

var builder = WebApplication.CreateBuilder(args); // setting up environment using builder for services like logging,auth.

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

// Registering the custom filter 
builder.Services.AddControllers(options =>{options.Filters.Add(typeof(UserVerificationFilter));});
//Connect MySql DB

builder.Services.AddDbContext<AppdbContext>(options =>options.UseMySql
        (
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
        ));

// services Injection.
builder.Services.AddServices();

builder.Host.UseSerilog();

//making a global logger
ConfigureLogger.CongigureGlobalLogger();

builder.Services.AddHttpClient();
//builder.Services.AddAuthorization(); // optional but is used when we add user roles for authorisation

var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();


// Configuring the HTTP request pipeline.
if (app.Environment.IsDevelopment()) //in lauchsettings.json, environment is set to Development.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();


//app.UseAuthorization();

app.MapControllers();

app.Run();