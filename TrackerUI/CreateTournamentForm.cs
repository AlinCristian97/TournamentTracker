using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeams();
        private List<TeamModel> _selectedTeams = new List<TeamModel>();
        private List<PrizeModel> _selectedPrizes = new List<PrizeModel>();
        
        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            const string teamsDisplayMember = "TeamName";
            const string prizesDisplayMember = "PlaceName";

            selectTeamDropdown.DataSource = null;
            tournamentTeamsListBox.DataSource = null;
            prizesListBox.DataSource = null;
            
            selectTeamDropdown.DataSource = _availableTeams;
            selectTeamDropdown.DisplayMember = teamsDisplayMember;

            tournamentTeamsListBox.DataSource = _selectedTeams;
            tournamentTeamsListBox.DisplayMember = teamsDisplayMember;

            prizesListBox.DataSource = _selectedPrizes;
            prizesListBox.DisplayMember = prizesDisplayMember;
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            var t = (TeamModel)selectTeamDropdown.SelectedItem;

            if (t != null)
            {
                _availableTeams.Remove(t);
                _selectedTeams.Add(t);
            
                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            var form = new CreatePrizeForm(this);
            form.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            _selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            _selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new CreateTeamForm(this);
            form.Show();
        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            var t = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (t != null)
            {
                _selectedTeams.Remove(t);
                _availableTeams.Add(t);
            
                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            var p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                _selectedPrizes.Remove(p);
                
                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate data
            decimal fee = 0;
            bool feeValid = decimal.TryParse(entryFeeValue.Text, out fee);

            if (!feeValid)
            {
                MessageBox.Show(
                    "You need to enter a valid Entry fee.",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                return;
            }
            
            // Create our tournament model
            var tm = new TournamentModel
            {
                TournamentName = tournamentNameValue.Text,
                EntryFee = fee,
                Prizes = _selectedPrizes,
                EnteredTeams = _selectedTeams
            };
            
            // Create our matchups

            // Create Tournament entry
            // Create all of the prizes entries
            // Create all of the team entries
            GlobalConfig.Connection.CreateTournament(tm);
        }
    }
}