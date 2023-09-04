using Microsoft.EntityFrameworkCore;
using TunaPianoApiSA.Models;
using System.Runtime.CompilerServices;

public class TunaPianoDbContext : DbContext
{

    public DbSet<Artists> Artists { get; set; }
    public DbSet<Genres> Genres { get; set; }
    public DbSet<Songs> Songs { get; set; }
    public DbSet<SongGenres> SongGenres { get; set; }

    public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data with artists
        modelBuilder.Entity<Artists>().HasData(new Artists[]
        {
        new Artists {Id = 1, Name = "Beyoncé", Age = 42, Bio = "Beyoncé Giselle Knowles-Carter is an American singer, songwriter, and businesswoman. Known as 'Queen Bey', her artistry, vocal abilities, performances and visual presentations have led her to become a pop culture figure of the 21st century."},
        new Artists {Id = 2, Name = "Kelly Rowland", Age = 42, Bio = "Kelendria Trene Rowland is an American singer, actress, and television personality. She rose to fame in the late 1990s as a member of Destiny's Child, one of the world's best-selling girl groups of all time."},
        new Artists {Id = 3, Name = "Michelle Williams", Age = 44, Bio = "Tenitra Michelle Williams is an American singer and actress. She rose to fame in the early 2000s as a member of Destiny's Child, one of the world's best-selling girl groups of all time."},
        new Artists {Id = 4, Name = "Aaliyah", Age = 22, Bio = "Aaliyah Dana Haughton was an American singer and actress. She has been credited for helping to redefine contemporary R&B, pop and hip hop, earning her the nicknames the 'Princess of R&B' and 'Queen of Urban Pop'."}
        });

        // seed data with songs
        modelBuilder.Entity<Songs>().HasData(new Songs[]
        {
        new Songs {Id = 1, Title = "HEATED", ArtistId = 1, Album = "RENAISSANCE", Length = 260},
        new Songs {Id = 2, Title = "Red Wine", ArtistId = 2, Album = "Talk A Good Game", Length = 259},
        new Songs {Id = 3, Title = "If We Had Your Eyes (ft. Fantasia)", ArtistId = 3, Album = "Journey To Freedom", Length = 274},
        new Songs {Id = 4, Title = "More Than A Woman", ArtistId = 4, Album = "Aaliyah", Length = 229},
        new Songs {Id = 5, Title = "I Care 4 U", ArtistId = 4, Album = "I Care 4 U", Length = 273}
        });

        // seed data with genres
        modelBuilder.Entity<Genres>().HasData(new Genres[]
        {
        new Genres {Id = 1, Description = "Pop"},
        new Genres {Id = 2, Description = "R&B"},
        new Genres {Id = 3, Description = "Gospel"},
        new Genres {Id = 4, Description = "Dance"},
        new Genres {Id = 5, Description = "House"},
        new Genres {Id = 6, Description = "Hip Hop"}
        });

        // seed data with a song genres
        modelBuilder.Entity<SongGenres>().HasData(new SongGenres[]
        {
        new SongGenres {Id = 1, SongId = 1, GenreId = 4},
        new SongGenres {Id = 2, SongId = 1, GenreId = 5},
        new SongGenres {Id = 3, SongId = 2, GenreId = 1},
        new SongGenres {Id = 4, SongId = 2, GenreId = 2},
        new SongGenres {Id = 5, SongId = 3, GenreId = 2},
        new SongGenres {Id = 6, SongId = 3, GenreId = 3},
        new SongGenres {Id = 7, SongId = 4, GenreId = 1},
        new SongGenres {Id = 8, SongId = 4, GenreId = 2},
        new SongGenres {Id = 9, SongId = 4, GenreId = 6},
        new SongGenres {Id = 10, SongId = 5, GenreId = 1},
        new SongGenres {Id = 11, SongId = 5, GenreId = 2},
        new SongGenres {Id = 12, SongId = 5, GenreId = 6}
        });
    }
}