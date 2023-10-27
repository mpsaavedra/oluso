using Oluso.Logger;
using Oluso.Logger.Abstractions;
using Oluso.Logger.Extensions;
using Oluso.Logger.Settings;
using ILogger = Oluso.Logger.Abstractions.ILogger;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// builder.Services.AddLoggerService(options =>
// {
//     options.UseSerilog(cfg =>
//     {
//         // cfg.AddConsole();
//         // cfg.AddFile();
//         cfg.AddConsole(LoggerLevel.Trace);
//         cfg.AddFile(@"TestLogs\\.logs", LoggerLevel.Trace);
//     });
// });
// var loggerSection = builder.Configuration.GetSection("Logger");
// var loggerSettings = loggerSection.Get<LoggerSettings>();
// builder.Services.AddLoggerService(loggerSettings);
//
// var app = builder.Build();
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
// app.UseAuthorization();
// app.MapControllers();
//
// var svr = app.Services.GetRequiredService<ILoggerService>();
// var logger = svr.CreateLogger("TestLogger");
var logger = Log.UseSerilog(builder.Configuration).CreateLogger<Program>();
logger.Info($"This is a simple information log");
logger.Debug($"This is a simple debug log");
logger.Trace($"This is a simple trace log");
logger.Error($"This is a simple error log");

// app.Run();
