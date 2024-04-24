using System.ComponentModel.DataAnnotations;

namespace Juntos_A_Suerte_Wasm.Models;

public class RegistroPasante
{
	[Key]
	public int CodigoRegistro {  get; set; }

	[Required(ErrorMessage ="Este campo es Obligatorio")]
	public string? Nombre { get; set; }

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public string? Apellido { get; set; }

	[Required(ErrorMessage = "Este campo es Obligatorio")]
	public string? Cedula { get; set;}

    [Required(ErrorMessage = "Este campo es Obligatorio")]
    public string? Carrera { get; set; }

    [Required(ErrorMessage = "Este campo es Obligatorio")]
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
