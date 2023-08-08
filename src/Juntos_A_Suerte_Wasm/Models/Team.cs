using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Juntos_A_Suerte_Wasm.Models;

public class Team
{
    [Key]
    public int TeamId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Name { get; set; }

    public List<Person> Members { get; set; } = new List<Person>();
}
