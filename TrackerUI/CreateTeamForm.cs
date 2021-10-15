using System;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        public CreateTeamForm()
        {
            InitializeComponent();
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

                GlobalConfig.Connection.CreatePerson(p);
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
    }
}