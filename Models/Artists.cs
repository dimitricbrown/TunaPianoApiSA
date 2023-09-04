using System.ComponentModel.DataAnnotations;

namespace TunaPianoApiSA.Models;
public class Artists
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Age { get; set; }
    public string Bio { get; set; }
    List<Songs> Songs { get; set; }
}
