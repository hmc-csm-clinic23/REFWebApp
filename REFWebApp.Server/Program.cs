var builder = WebApplication.CreateBuilder(args);

// Database Connection 
ConfigureServices(
    builder.Services,
    builder.Configuration
);

void ConfigureServices(IServiceCollection services, ConfigurationManager configManager) {
    services.AddDbContext<PostgresContext>(
        opts => {
            opts.UseNpgsql(configManager.GetConnectionString("REFDb"));

        }, ServiceLifetime.Transient);
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
