using TunaPianoApiSA.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

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

// Get a single artist, including its associated songs
app.MapGet("/api/artists/{artistId}", (TunaPianoDbContext db, int id) =>
{
    Artists artist = db.Artists
        .Include(a => a.Songs)
        .FirstOrDefault(a => a.Id == id);
    if (artist == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artist);
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

// Delete an artist
app.MapDelete("/api/artists/{artistId}", (TunaPianoDbContext db, int id) =>
{
    Artists artist = db.Artists.SingleOrDefault(a => a.Id == id);
    if (artist == null)
    {
        return Results.NotFound();
    }
    db.Artists.Remove(artist);
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

// Get a single song, including its associated genres and artist details
app.MapGet("/api/songs/{songId}", (TunaPianoDbContext db, int id) =>
{
    Songs song = db.Songs
        .Include(s => s.Artist)
        .Include(s => s.Genres)
        .ThenInclude(g => g.Genre)
        .FirstOrDefault(s => s.Id == id);
    if (song == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(song);
});

// Create a new song
app.MapPost("/api/songs", (TunaPianoDbContext db, Songs song) =>
{
    db.Songs.Add(song);
    db.SaveChanges();

    if (song.Genres != null)
    {
        foreach (var songGenre in song.Genres)
        {
            db.SongGenres.Add(songGenre);
            songGenre.SongId = song.Id;
        }
    }
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

// Delete a song
app.MapDelete("/api/songs/{songId}", (TunaPianoDbContext db, int songId) =>
{
    Songs song = db.Songs.SingleOrDefault(s => s.Id == songId);
    if (song == null)
    {
        return Results.NotFound();
    }
    var deletedSongGenres = db.SongGenres.Where(sg => sg.SongId == songId);
    db.SongGenres.RemoveRange(deletedSongGenres);

    db.Songs.Remove(song);
    db.SaveChanges();
    return Results.NoContent();
});

// GENRES
// Getting all genres
app.MapGet("/api/genres", (TunaPianoDbContext db) =>
{
    return db.Genres.ToList();
});

// Get a single genre, including its associated songs
app.MapGet("/api/genres/{genreId}", (TunaPianoDbContext db, int id) =>
{
    Genres genre = db.Genres
        .Include(g => g.Songs)
        .ThenInclude(s => s.Song)
        .FirstOrDefault(g => g.Id == id);
    if (genre == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);
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

// Delete a genre
app.MapDelete("/api/genres/{genreId}", (TunaPianoDbContext db, int genreId) =>
{
    Genres genre = db.Genres.SingleOrDefault(g => g.Id == genreId);
    if (genre == null)
    {
        return Results.NotFound();
    }

    var deletedGenreSongs = db.SongGenres.Where(sg => sg.GenreId == genreId);
    db.SongGenres.RemoveRange(deletedGenreSongs);

    db.Genres.Remove(genre);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
