using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeams();
        
        public CreateTournamentForm()
        {
            InitializeComponent();

            InitializeLists();
        }

        private void InitializeLists()
        {
            selectTeamDropdown.DataSource = _availableTeams;
            selectTeamDropdown.DisplayMember = "TeamName";
        }
    }
}