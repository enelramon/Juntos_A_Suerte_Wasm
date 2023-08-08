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

    public async Task AddPersonAsync(Person person)
    {
        var people = await GetPeopleAsync();
        person.PersonId = people.Any() ? people.Max(p => p.PersonId) + 1 : 1;
        people.Add(person);
        await SavePeopleAsync(people);
    }

    private async Task SavePeopleAsync(List<Person> people)
    {
        await _localStorage.SetItemAsync(StorageKey, people);
    }

    public async Task AddPeopleListAsync(List<Person> newPeople)
    {
        var existingPeople = await GetPeopleAsync();
        existingPeople.AddRange(newPeople);
        await SavePeopleAsync(existingPeople);
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        return await _localStorage.GetItemAsync<List<Person>>(StorageKey) ?? new List<Person>();
    }

    public async Task DeletePersonAsync(int personId)
    {
        var people = await GetPeopleAsync();
        var personToRemove = people.FirstOrDefault(p => p.PersonId == personId);
        if (personToRemove != null)
        {
            people.Remove(personToRemove);
            await SavePeopleAsync(people);
        }
    }
}
