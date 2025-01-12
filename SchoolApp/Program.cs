using Microsoft.EntityFrameworkCore;
using SchoolApp.Configurations;
using SchoolApp.Data;
using SchoolApp.SomeHelper;
using Serilog;
using AutoMapper;
using SchoolApp.Data.Repository;

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
