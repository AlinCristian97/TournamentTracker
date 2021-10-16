﻿using System;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        private IPrizeRequester _callingForm;
        
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();

            _callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                var model = new PrizeModel(
                    placeNameValue.Text,
                    placeNumberValue.Text,
                    prizeAmountValue.Text,
                    prizePercentageValue.Text);

                GlobalConfig.Connection.CreatePrize(model);

                _callingForm.PrizeComplete(model);

                Close();

                // placeNameValue.Text = "";
                // placeNumberValue.Text = "";
                // prizeAmountValue.Text = "0";
                // prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;

            int placeNumber = 0;
            bool placeNumberValid = int.TryParse(placeNumberValue.Text, out placeNumber);

            if (!placeNumberValid)
            {
                output = false;
            }

            if (placeNumber < 1)
            {
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (!prizeAmountValid || !prizePercentageValid)
            {
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
            }

            if (prizeAmount > 0 && prizePercentage > 0)
            {
                output = false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }
            
            return output;
        }
    }
}