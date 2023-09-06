using System.ComponentModel.DataAnnotations;

namespace TunaPianoApiSA.Models;

public class SongGenres
{
    public int Id { get; set; }
    [Required]
    public int SongId { get; set; }
    public Songs? Song { get; set; }
    public int GenreId { get; set; }
    public Genres? Genre { get; set; }
}