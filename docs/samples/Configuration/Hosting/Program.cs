using Oluso.Configuration.Hosting.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddConfigurationService()
     .AddFilesystemProvider(cfg => 
        cfg
            .WithPath(@"Configurations"))
    // .AddGitProvider(cfg =>
    //     cfg
    //         .WithRepositoryUrl("ssh://mpsaavedra@10.42.0.1:/home/mpsaavedra/settings-repo")
    //         .WithLocalPath("C:/local-repo")
    //         .WithUsername("mpsaavedra")
    //         .WithPassword("P3lvmten")
    //         .WithBranch("master"))
     .AddRabbitMqPublisher(cfg =>
         cfg
             .WithHostName($"localhost")
             .WithUserName("guest")
             .WithPassword("guest")
             .WithVirtualHost("/"));

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
app.MapConfigurationService();

app.Run();
