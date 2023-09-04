using System.ComponentModel.DataAnnotations;

namespace TunaPianoApiSA.Models;

public class Genres
{
    public int Id { get; set; }
    [Required]
    public string Description { get; set; }
    List<Songs> Songs { get; set; }
}