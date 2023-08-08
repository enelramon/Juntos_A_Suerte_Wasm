using Juntos_A_Suerte_Wasm.Models;
using Blazored.LocalStorage;

namespace Juntos_A_Suerte_Wasm.Services;

public class PersonService
{
    private readonly ILocalStorageService _localStorage;
    private const string StorageKey = "people";

    public PersonService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        // Get the people from local storage
        return await _localStorage.GetItemAsync<List<Person>>(StorageKey) ?? new List<Person>();
    }

    public async Task AddPersonAsync(Person person)
    {
        // Add the person to the list
        var people = await GetPeopleAsync();
        people.Add(person);

        // Save the people to local storage
        await _localStorage.SetItemAsync(StorageKey, people);
    }

    public async Task DeletePersonAsync(Person person)
    {
        // Remove the person from the list
        var people = await GetPeopleAsync();
        people.Remove(person);

        // Save the people to local storage
        await _localStorage.SetItemAsync(StorageKey, people);
    }
}
