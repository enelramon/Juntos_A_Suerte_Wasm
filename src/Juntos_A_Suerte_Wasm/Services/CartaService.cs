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

	public async Task SaveOrUpdatePasanteAsync(RegistroPasante pasante)
	{
		await _localStorage.SetItemAsync(StorageKey, pasante);
	}

	public async Task<RegistroPasante?> GetPasanteAsync()
	{
		return await _localStorage.GetItemAsync<RegistroPasante>(StorageKey);
	}
}
