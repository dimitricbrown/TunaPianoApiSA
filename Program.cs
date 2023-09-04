using TunaPianoApiSA.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ARTISTS
// Getting all artists
app.MapGet("/api/artists", (TunaPianoDbContext db) =>
{
    return db.Artists.ToList();
});

// Create a new artist
app.MapPost("/api/artists", (TunaPianoDbContext db, Artists artist) =>
{
    db.Artists.Add(artist);
    db.SaveChanges();
    return Results.Created($"/api/artists/{artist.Id}", artist);
});

// SONGS
// Getting all songs
app.MapGet("/api/songs", (TunaPianoDbContext db) =>
{
    return db.Songs
        .Include(s => s.Artist)
        .ToList();
});

// Create a new song
app.MapPost("/api/songs", (TunaPianoDbContext db, Songs song) =>
{
    db.Songs.Add(song);
    db.SaveChanges();
    return Results.Created($"/api/songs/{song.Id}", song);
});

// GENRES
// Getting all genres
app.MapGet("/api/genres", (TunaPianoDbContext db) =>
{
    return db.Genres.ToList();
});

// Create a new genre
app.MapPost("/api/genres", (TunaPianoDbContext db, Genres genre) =>
{
    db.Genres.Add(genre);
    db.SaveChanges();
    return Results.Created($"/api/genres/{genre.Id}", genre);
});

app.Run();
