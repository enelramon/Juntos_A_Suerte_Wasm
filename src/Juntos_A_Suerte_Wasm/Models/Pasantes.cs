using System.ComponentModel.DataAnnotations;

namespace Juntos_A_Suerte_Wasm.Models;

public class Pasantes
{
	[Key]
	public int PasanteId {  get; set; }

	[Required(ErrorMessage ="Este campo es Obligatorio")]
    [RegularExpression(@"^[a-zA-ZÁÉÍÓÚÑ][a-zA-ZÁÉÍÓÚÑ ]*$", ErrorMessage = "El Apellido debe comenzar con una letra mayúscula y no debe contener números.")]
    public string? Nombre { get; set; }

	[Required(ErrorMessage = "Este campo es Obligatorio")]
    [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\W]{1,13}$", ErrorMessage = "La Cédula debe tener máximo 13 caracteres alfanuméricos, incluyendo símbolos")]
    public string? Cedula { get; set;}

    [Required(ErrorMessage = "Este campo es Obligatorio")]
    [RegularExpression(@"^[a-zA-ZÁÉÍÓÚÑ][a-zA-ZÁÉÍÓÚÑ ]*$", ErrorMessage = "La Carrera debe comenzar con una letra mayúscula y no debe contener números.")]

    public string? Carrera { get; set; }

    [Required(ErrorMessage = "Este campo es Obligatorio")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "La Matrícula debe contener exactamente 8 dígitos.")]
    public string? Matricula { get; set; }

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public string? Pasantia { get; set; }

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public DateTime? DiaInicio { get; set; } = DateTime.Now;

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public DateTime DiaFinal { get; set; } = DateTime.Now;

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public int CantidadHoras { get; set; }

	public DateTime Expide {  get; set; } = DateTime.Now;
}
