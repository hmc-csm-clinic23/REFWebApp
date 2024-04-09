using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Npgsql.Replication;
using REFWebApp.Server.Data;
using REFWebApp.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Connection 
/*
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
*/
// Add services to the container.

builder.Services.AddControllers();
    //.AddJsonOptions(x => 
    //                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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

// be able to get data from database
/*
Console.WriteLine("audio files:");

/*
using PostgresContext context = new PostgresContext();
List<AudioFile> audios = context.AudioFiles.ToList();

foreach (AudioFile a in audios) {
    Console.WriteLine(a.Path);
    Console.WriteLine(a.GroundTruth);
    Console.WriteLine("-------------------------------------------------------");
}
Console.WriteLine("audio files have been printed");
*/

using (var context = new PostgresContext())
{

    List<Scenario> scenarios = context.Scenarios
                                  .Include(s => s.Audios)
                                  .ToList();

    ICollection<AudioFile> audios = scenarios[0].Audios;

    Console.WriteLine(audios.Count);
    foreach (AudioFile a in audios) {
        Console.WriteLine(a.Path);
        Console.WriteLine(a.GroundTruth);
        Console.WriteLine("-------------------------------------------------------");
    }
    /*
    string fileName = "Test/transcriptions.json";
    string jsonString = File.ReadAllText(fileName);
    var transcriptions = JsonSerializer.Deserialize<List<Transcription>>(jsonString);
    // creating transciption object 
    Transcription test = new Transcription()
    {
        AudioId = 1818,
        SttId = 1,
        Timestamp = DateTime.Now,
        Transcript = "aaaaa"
    };
    

    foreach (var t in transcriptions) { 
        Console.WriteLine(t.AudioId);
        Console.WriteLine(t.SttId);
    }
    context.BulkInsert(transcriptions);
    context.SaveChanges();
    */
    //using PostgresContext context = new PostgresContext();
    Console.WriteLine("adding scenario");
    List<AudioFile> audiofiles = context.AudioFiles.Where(a => a.Id == 1818).ToList();
    Scenario s = new Scenario() {
        Name = "hello",
        Audios = audiofiles
    };

    context.Scenarios.Add(s);
    context.SaveChanges();
    Console.WriteLine("scenario added");


}


