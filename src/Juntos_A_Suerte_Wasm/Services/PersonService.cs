using Juntos_A_Suerte_Wasm.Models;
using Blazored.LocalStorage;

namespace Juntos_A_Suerte_Wasm.Services;

public class PersonService
{
    private readonly ILocalStorageService _localStorage;
    private List<Person> _existingPeople = new List<Person>();
    private const string StorageKey = "people";

    public PersonService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

	public async Task SaveAsync(string[] names)
	{
		_existingPeople = await GetPeopleAsync();

        if (names.Length == 1)
        {
            AddPerson(names[0]);
        }
        else
        {
            AddPeopleList(names);
        }
		await SavePersonAsync();
	}

	private void AddPerson(string name)
    {
        var newPerson = new Person
        {
            PersonId = GetNextId(),
            Name = name.Trim()
        };

        _existingPeople.Add(newPerson);
    }

    private void AddPeopleList(string[] names)
    {
        foreach (var name in names)
        {
            AddPerson(name);
        }
    }

    private int GetNextId()
    {
        return _existingPeople!.Any() ? _existingPeople!.Max(p => p.PersonId) + 1 : 1;
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        return await _localStorage.GetItemAsync<List<Person>>(StorageKey) ?? new List<Person>();
    }

    public async Task DeletePersonAsync(int personId)
    {
        _existingPeople = await GetPeopleAsync();
        var personToRemove = _existingPeople.FirstOrDefault(p => p.PersonId == personId);
        if (personToRemove != null)
        {
            _existingPeople.Remove(personToRemove);
            await SavePersonAsync();
        }
    }
    private async Task SavePersonAsync()
    {
        await _localStorage.SetItemAsync(StorageKey, _existingPeople);
    }
}
