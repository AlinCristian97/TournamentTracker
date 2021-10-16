﻿using System;
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
    }
}