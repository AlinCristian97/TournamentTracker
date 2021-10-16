using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeams();
        private List<TeamModel> _selectedTeams = new List<TeamModel>();
        private List<PrizeModel> _selectedPrizes = new List<PrizeModel>();
        
        public CreateTournamentForm()
        {
            InitializeComponent();

            InitializeLists();
        }

        private void InitializeLists()
        {
            const string teamsDisplayMember = "TeamName";
            const string prizesDisplayMember = "PlaceName";
            
            selectTeamDropdown.DataSource = _availableTeams;
            selectTeamDropdown.DisplayMember = teamsDisplayMember;

            tournamentTeamsListBox.DataSource = _selectedTeams;
            tournamentTeamsListBox.DisplayMember = teamsDisplayMember;

            prizesListBox.DataSource = _selectedPrizes;
            prizesListBox.DisplayMember = prizesDisplayMember;
        }
    }
}