using Blazored.LocalStorage;
using Juntos_A_Suerte_Wasm.Models;
namespace Juntos_A_Suerte_Wasm.Services;

public class CartaService
{
    private readonly ILocalStorageService _localStorage;
    private const string StorageKey = "pasantes";

    public CartaService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<Pasantes?> GetPasanteAsync(int PasanteId)
    {
        return await _localStorage.GetItemAsync<Pasantes>(GetStorageKey(PasanteId));
    }

    public async Task<int> GetNextCodigoRegistroAsync()
    {
        // Recuperar los pasantes 
        var pasantes = await GetAllPasantesAsync();

        // Si no hay pasantes almacenados, el proximo codigo sera 1
        if (pasantes.Count == 0)
        {
            return 1;
        }

        // Encontrar el maximo valor
        int maxCodigo = pasantes.Max(p => p.PasanteId);

        // Incrementar
        return maxCodigo + 1;
    }

    public async Task<Pasantes?> GetPasanteAsyncNew()
    {
        var pasantes = await GetAllPasantesAsync();
        return pasantes.FirstOrDefault();
    }


    public async Task SaveOrUpdatePasanteAsync(Pasantes pasante)
    {
        // Verificar si el pasante ya existe
        var existingPasante = await GetPasanteAsync(pasante.PasanteId);

        if (existingPasante != null)
        {
            // Si el pasante existe, actualizar sus propiedades
            existingPasante.Nombres = pasante.Nombres;
            existingPasante.Cedula = pasante.Cedula;
            existingPasante.Carrera = pasante.Carrera;
            existingPasante.Matricula = pasante.Matricula;
            existingPasante.DiaFinal = pasante.DiaFinal;
            existingPasante.DiaInicio = pasante.DiaInicio;
            existingPasante.CantidadHoras = pasante.CantidadHoras;
            existingPasante.Pasantia = pasante.Pasantia;

            // Guardar el pasante actualizado
            await _localStorage.SetItemAsync(GetStorageKey(pasante.PasanteId), existingPasante);
        }
        else
        {
            // Si el pasante no existe, asignar un nuevo codigo de registro y guardarlo
            pasante.PasanteId = await GetNextCodigoRegistroAsync();
            await _localStorage.SetItemAsync(GetStorageKey(pasante.PasanteId), pasante);
        }
    }


    private string GetStorageKey(int PasanteId)
    {
        return $"{StorageKey}_{PasanteId}";
    }

    public async Task<List<Pasantes>> GetAllPasantesAsync()
    {
        // Obtener todas las claves almacenadas en LocalStorage
        var keys = await _localStorage.KeysAsync();

        // Filtrar las claves 
        var pasanteKeys = keys.Where(key => key.StartsWith(StorageKey));

        // Recuperar los pasantes
        var pasantes = new List<Pasantes>();
        foreach (var key in pasanteKeys)
        {
            var pasante = await _localStorage.GetItemAsync<Pasantes>(key);
            if (pasante != null)
            {
                pasantes.Add(pasante);
            }
        }

        return pasantes;
    }

    public async Task<List<Pasantes>> ListarPasantesAsync()
    {
        // Obtener todos los pasantes almacenados
        var pasantes = await GetAllPasantesAsync();
        return pasantes;
    }
}


