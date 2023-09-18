using Oluso.Logger.Abstractions;
using Oluso.Logger.Extensions;
using ILogger = Oluso.Logger.Abstractions.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLoggerService(options =>
{
    options.UseSerilog(cfg =>
    {
        cfg.AddConsole();
        cfg.AddFile(@"TestLogs\\.logs");
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var svr = app.Services.GetRequiredService<ILoggerService>();
var logger = svr.CreateLogger("TestLogger");
logger.Info($"This is a simple information log");
logger.Error($"This is a simple error log");
logger.Debug($"This is a simple debug log");
logger.Trace($"This is a simple trace log");

app.Run();
