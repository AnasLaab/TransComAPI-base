using Microsoft.Extensions.DependencyInjection;
using TransComAPI.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register ParsingService with dependency injection.
builder.Services.AddSingleton<ParsingService>();

// Register SerialService with dependency injection. Inject ParsingService as well.
builder.Services.AddSingleton<SerialService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<SerialService>>();
    var parsingService = serviceProvider.GetRequiredService<ParsingService>();
    // La configuration sp�cifique du port COM est maintenant g�r�e automatiquement par SerialService
    // Passez le parsingService comme param�tre au constructeur de SerialService si n�cessaire
    return new SerialService(logger, parsingService);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
