using Microsoft.EntityFrameworkCore;
using SchoolApp.Configurations;
using SchoolApp.Data;
using SchoolApp.SomeHelper;
using Serilog;
using AutoMapper;
using SchoolApp.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();


builder.Logging.AddLog4Net();


builder.Services.AddDbContext<CollegeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("con"));
});


// Add services to the container.

builder.Services.AddControllers(options=>options.ReturnHttpNotAcceptable=true).AddNewtonsoftJson().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IStudentRepository, Repository>();
builder.Services.AddScoped(typeof(ISchoolRepository<>), typeof(SchoolRepository<>));

//Adding CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnOnlyLocalHost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com","http://gmail.com").WithHeaders("Accept", "Something").WithMethods("GET", "POST");
    });




});

// adding jwt
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/api/ok",
        context => context.Response.WriteAsync("echo"))
        .RequireCors("AllowOnOnlyLocalHost");

    endpoints.MapControllers()
             .RequireCors();

    endpoints.MapGet("/api/ok5",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));

});

app.Run();
