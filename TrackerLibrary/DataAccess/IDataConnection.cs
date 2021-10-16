using System.Collections.Generic;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetPeople();
        
        TeamModel CreateTeam(TeamModel model);
        List<TeamModel> GetTeams();
    }
}