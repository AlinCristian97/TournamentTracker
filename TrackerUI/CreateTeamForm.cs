using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> _availableTeamMembers = GlobalConfig.Connection.GetPeople();
        private List<PersonModel> _selectedTeamMembers = new List<PersonModel>();
        
        public CreateTeamForm()
        {
            InitializeComponent();
            
            //Testing
            //CreateSampleData();
            
            WireUpLists();
        }

        private void CreateSampleData()
        {
            _availableTeamMembers.Add(new PersonModel {FirstName = "Alin", LastName = "Apostol"});
            _availableTeamMembers.Add(new PersonModel {FirstName = "John", LastName = "Spicey"});
            
            _selectedTeamMembers.Add(new PersonModel {FirstName = "Alin", LastName = "Apostol"});
            _selectedTeamMembers.Add(new PersonModel {FirstName = "Brown", LastName = "Adams"});
        }

        private void WireUpLists()
        {
            selectTeamMemberDropdown.DataSource = null;
            teamMembersListBox.DataSource = null;
            
            selectTeamMemberDropdown.DataSource = _availableTeamMembers;
            selectTeamMemberDropdown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = _selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                var p = new PersonModel
                {
                    FirstName = firstNameValue.Text,
                    LastName = lastNameValue.Text,
                    EmailAddress = emailValue.Text,
                    PhoneNumber = phoneNumberValue.Text
                };

                p = GlobalConfig.Connection.CreatePerson(p);
                
                _selectedTeamMembers.Add(p);
            
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                phoneNumberValue.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill in all the fields.");
            }
        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            
            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }
            
            if (emailValue.Text.Length == 0)
            {
                return false;
            }
            
            if (phoneNumberValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }
        
        private void addMemberButton_Click(object sender, EventArgs e)
        {
            var p = (PersonModel)selectTeamMemberDropdown.SelectedItem;

            if (p != null)
            {
                _availableTeamMembers.Remove(p);
                _selectedTeamMembers.Add(p);
            
                WireUpLists();
            }
        }

        private void removeSelectedButton_Click(object sender, EventArgs e)
        {
            var p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                _selectedTeamMembers.Remove(p);
                _availableTeamMembers.Add(p);
            
                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            var t = new TeamModel
            {
                TeamName = teamNameValue.Text,
                TeamMembers = _selectedTeamMembers
            };

            t = GlobalConfig.Connection.CreateTeam(t);
            
            //TODO - If we aren't closing this form after creation, reset the form
        }
    }
}