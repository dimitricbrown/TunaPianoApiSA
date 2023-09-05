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

// Update an artist
app.MapPut("/api/artists/{artistId}", (TunaPianoDbContext db, int id, Artists artist) =>
{
    Artists artistToUpdate = db.Artists.SingleOrDefault(a => a.Id == id);
    if (artistToUpdate == null)
    {
        return Results.NotFound();
    }
    artistToUpdate.Name = artist.Name;
    artistToUpdate.Age = artist.Age;
    artistToUpdate.Bio = artist.Bio;
    db.SaveChanges();
    return Results.NoContent();
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

// Update a song
app.MapPut("/api/songs/{songId}", (TunaPianoDbContext db, int id, Songs song) =>
{
    Songs songToUpdate = db.Songs.SingleOrDefault(s => s.Id == id);
    if (songToUpdate == null)
    {
        return Results.NotFound();
    }
    songToUpdate.Title = song.Title;
    songToUpdate.ArtistId = song.ArtistId;
    songToUpdate.Album = song.Album;
    songToUpdate.Length = song.Length;
    db.SaveChanges();
    return Results.NoContent();
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

// Update a genre
app.MapPut("/api/genres/{genreId}", (TunaPianoDbContext db, int id, Genres genre) =>
{
    Genres genreToUpdate = db.Genres.SingleOrDefault(g => g.Id == id);
    if (genreToUpdate == null)
    {
        return Results.NotFound();
    }
    genreToUpdate.Description = genre.Description;
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
