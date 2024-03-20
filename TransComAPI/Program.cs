using Microsoft.Extensions.DependencyInjection;
using TransComAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register SerialService with dependency injection. No need to specify COM port here.
builder.Services.AddSingleton<SerialService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<SerialService>>();
    // La configuration spécifique du port COM est maintenant gérée automatiquement par SerialService
    return new SerialService(logger);
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