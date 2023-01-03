using BusSchedule.API.DbContext;
using BusSchedule.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
//adding timeonly suport
builder.Services.AddDateOnlyTimeOnlyStringConverters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseDateOnlyTimeOnlyStringConverters();
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPatch = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    c.IncludeXmlComments(xmlCommentsFullPatch);
});

builder.Services.AddDbContext<BusScheduleContext>(
    dbContextOptions => dbContextOptions.UseSqlite("Data Source = BusSchedule.db"));

builder.Services.AddScoped<IBusScheduleRepository, BusScheduleRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    

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
