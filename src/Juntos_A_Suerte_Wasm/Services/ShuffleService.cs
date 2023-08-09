using Juntos_A_Suerte_Wasm.Models;

namespace Juntos_A_Suerte_Wasm.Services;

public static class ShuffleService
{
    public static void ShufflePeopleIntoTeams(List<Team> teams, List<Person> people)
    {
        Random random = new Random();

        var shuffledPeople = people.OrderBy(p => random.Next()).ToList();
        int teamIndex = 0;

        teams.ForEach(t => t.Members.Clear());
        foreach (var person in shuffledPeople)
        {
            var currentTeam = teams[teamIndex];
            currentTeam.Members.Add(person);

            teamIndex = (teamIndex + 1) % teams.Count; // Move to the next team in a circular manner
        }

        //Random random = new Random();

        //var shuffledPeople = people.OrderBy(p => random.Next()).ToList();
        //int peopleIndex = 0;

        //foreach (var team in teams)
        //{
        //    team.Members.Clear();
        //    for (int i = 0; i < shuffledPeople.Count / teams.Count; i++)
        //    {
        //        if (peopleIndex >= shuffledPeople.Count)
        //        {
        //            break; // Break if there are no more people to assign
        //        }

        //        var person = shuffledPeople[peopleIndex];
        //        team.Members.Add(person);
        //        peopleIndex++;
        //    }
        //}
    }

}