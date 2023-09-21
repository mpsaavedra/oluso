using System.Configuration;
using Data;
using Data.Data;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oluso.Data.Extensions;
using Oluso.Extensions;
using Shouldly;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((host, services) =>
{
    services
        .AddDbContext<ApplicationDbContext>(opts =>
            opts.UseInMemoryDatabase("DataPackageExample"))
        .AddUnitOfWork(typeof(Program))
        .AddRepositories(typeof(Program));
});
using var host = builder.Build();
var helper = new LectureHelper();
await helper.SeedDatabase(host.Services);
await helper.RunTests(host.Services);
// await host.RunAsync();

