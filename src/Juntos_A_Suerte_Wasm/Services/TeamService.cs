using Blazored.LocalStorage;
using Juntos_A_Suerte_Wasm.Models;

namespace Juntos_A_Suerte_Wasm.Services;

public class TeamService
{
    private readonly ILocalStorageService _localStorage;
    private const string StorageKey = "teams";

    public TeamService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task AddTeamAsync(Team team)
    {
        var teams = await GetTeamsAsync();
        team.TeamId = teams.Any() ? teams.Max(t => t.TeamId) + 1 : 1;
        teams.Add(team);
        await SaveTeamsAsync(teams);
    }

    private async Task SaveTeamsAsync(List<Team> teams)
    {
        await _localStorage.SetItemAsync(StorageKey, teams);
    }

    public async Task<List<Team>> GetTeamsAsync()
    {
        return await _localStorage.GetItemAsync<List<Team>>(StorageKey) ?? new List<Team>();
    }

    public async Task DeleteTeamAsync(int teamId)
    {
        var teams = await GetTeamsAsync();
        var teamToRemove = teams.FirstOrDefault(t => t.TeamId == teamId);
        if (teamToRemove != null)
        {
            teams.Remove(teamToRemove);
            await SaveTeamsAsync(teams);
        }
    }
}
