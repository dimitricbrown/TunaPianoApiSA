using System.ComponentModel.DataAnnotations;

namespace TunaPianoApiSA.Models;

public class Songs
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public int ArtistId { get; set; }
    public Artists? Artist { get; set; }
    public string? Album { get; set; }
    public int Length { get; set; }
    public List<SongGenres>? Genres { get; set; }
}
