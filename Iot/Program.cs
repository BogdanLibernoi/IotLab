using Iot.Data;
using Microsoft.EntityFrameworkCore;


IotDeviceManager deviceManager = new IotDeviceManager();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql("Host=localhost;Port=5432;Database=iot_db;Username=postgres;Password=12345"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();



//app.MapGet("/", async (AppDbContext db) =>
//await db.Devices.ToListAsync()

//);

//app.MapGet("/TodayAverageTemp", async (AppDbContext db) =>
//{
//    var NowDataList = await db.Devices.Where(d => d.Date.StartsWith(DateTime.Now.Day.ToString()))
//      .ToListAsync();

//    var Templist = NowDataList.Average<DeviceData>(d => d.Tempeture);

//    return $"Average Today Temperature is {Templist} C";
    
//}
//);

//app.MapGet("/TodayAverageHum", async (AppDbContext db) =>
//{
//    var NowDataList = await db.Devices.Where(d => d.Date.StartsWith(DateTime.Now.Day.ToString()))
//      .ToListAsync();

//    var Templist = NowDataList.Average<DeviceData>(d => d.Humidity);

//    return $"Average Today Humidity is {Templist} %";

//}
//);

//app.MapGet("/TodayAverageData", async (AppDbContext db) =>
//{
//    var NowDataList = await db.Devices.Where(d => d.Date.StartsWith(DateTime.Now.Day.ToString()))
//      .ToListAsync();

//    var Templist = NowDataList.Average<DeviceData>(d => d.Tempeture);
//    var HumList = NowDataList.Average<DeviceData>(d => d.Humidity);

//    return $"Average Today Tempeture is {Templist} % Humidity is {HumList}";

//}
//);

//app.MapGet("/Temperature/Chart", async (AppDbContext db) =>
//{
    
//}
//    );


app.Run();
