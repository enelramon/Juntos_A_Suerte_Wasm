using System.ComponentModel.DataAnnotations;

namespace Juntos_A_Suerte_Wasm.Models;

public class Person
{
    [Key]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "El Nombre es obligatorio")]
    public string? Name { get; set; }
}
