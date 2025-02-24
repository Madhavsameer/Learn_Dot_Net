using Microsoft.EntityFrameworkCore;
using MyFirstAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Read MySQL Connection String from appsettings.json
var mysqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Configure MySQL Service Provider
builder.Services.AddDbContext<EmployeeContext>(options =>
    options.UseMySql(mysqlConnectionString, new MySqlServerVersion(new Version(8, 0, 32))));

// Configure MongoDB Service Provider
builder.Services.AddSingleton<MongoDbService>();

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();



app.UseAuthorization();
app.MapControllers();
app.Run();
