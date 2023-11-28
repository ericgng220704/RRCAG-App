using Nguyen.GiaHao.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Nguyen.GiaHao.RRCAGApp
{
    public partial class SalesQuoteForm : Form
    {
        public SalesQuoteForm()
        {
            InitializeComponent();

            // Initial state
            SetInitialState();

            // Calculate Button Click (Calculating Vehicle Sales Quote)
            this.btnCalculateQuote.Click += BtnCalculateQuote_Click;

            // Changes events
            /* TextBox changes */
            this.txtVehicleSalePrice.TextChanged += Input_TextChanged;
            this.txtTradeInValue.TextChanged += Input_TextChanged;

            /* Options and Finance changes */
            this.chkStereoSystem.CheckedChanged += OptionsAndFinance_Changed; 
            this.chkLeatherInterior.CheckedChanged += OptionsAndFinance_Changed;
            this.chkComputerNavigation.CheckedChanged += OptionsAndFinance_Changed;
            this.radStandard.CheckedChanged += OptionsAndFinance_Changed;
            this.radPearlized.CheckedChanged += OptionsAndFinance_Changed;
            this.radCustomizedDetailing.CheckedChanged += OptionsAndFinance_Changed;
            this.nudNoOfYear.ValueChanged += OptionsAndFinance_Changed;
            this.nudAnnualInterestRate.ValueChanged += OptionsAndFinance_Changed;


            // Reset Button Click
            this.btnReset.Click += BtnReset_Click;
        }

        /// <summary>
        /// Hanldes the Change event of Options and Finance groups inputs
        /// </summary>
        private void OptionsAndFinance_Changed(object sender, EventArgs e)
        {
            // Check if the sales quote has been created
            if (!this.lblAmountDue.Text.Equals(string.Empty))
            {
                Calculate();
            }            
        }

        /// <summary>
        /// Hanldes the Change event of textboxes input
        /// </summary>
        private void Input_TextChanged(object sender, EventArgs e)
        {
            // Clear the summary and monthly payment 
            this.lblVehicleSalePrice.Text = string.Empty;
            this.lblOption.Text = string.Empty;
            this.lblSubtotal.Text = string.Empty;
            this.lblSalesTaxRate.Text = string.Empty;
            this.lblTotal.Text = string.Empty;
            this.lblTradeInValue.Text = string.Empty;
            this.lblAmountDue.Text = string.Empty;
            this.lblMonthlyPayment.Text = string.Empty;
        }

        /// <summary>
        /// Handles Click event of Btn CalculateQuote
        /// </summary>
        private void BtnCalculateQuote_Click(object sender, EventArgs e)
        {
            // Error check
            ErrorCheck();
        
            // Do calculation if there are no errors
            if (this.errorProvider.GetError(txtVehicleSalePrice).Equals(string.Empty) && this.errorProvider.GetError(txtTradeInValue).Equals(string.Empty))
            {
                Calculate();
            }        
        }

        /// <summary>
        /// Create a new Sale Quote, do the calculation and display result to labels in summary group and finance group
        /// </summary>
        private void Calculate()
        {
            // Calculate Summary group
            SalesQuote salesQuote = CreateSalesQuote();

            SetSummaryOutputs(salesQuote);

            // Calculate Finance group
            SetMonthlyPayment(this.nudAnnualInterestRate.Value, this.nudNoOfYear.Value, salesQuote.AmountDue);
        }

        /// <summary>
        /// Handles Click event of Btn Reset
        /// </summary>
        private void BtnReset_Click(object sender, EventArgs e)
        {
            // Message box setting
            string caption = "Reset Form";
            string text = "Do you want to reset the form?";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;

            // Show message box
            DialogResult result = MessageBox.Show(text, caption, button, icon, MessageBoxDefaultButton.Button2);    
            
            // Handle message box result
            switch (result) 
            {
                case DialogResult.Yes:
                    {
                        SetInitialState();
                        this.errorProvider.SetError(this.txtTradeInValue, string.Empty);
                        this.errorProvider.SetError(this.txtVehicleSalePrice, string.Empty);
                        break;
                    }          
            }
        }

        /// <summary>
        /// Set the Sales Quote Form to it initial state
        /// </summary>
        private void SetInitialState()
        {
            this.txtVehicleSalePrice.Text = string.Empty;

            this.txtTradeInValue.Text = string.Empty;
            
            // Accessories group
            this.chkComputerNavigation.Checked = false;
            this.chkLeatherInterior.Checked = false;
            this.chkStereoSystem.Checked = false;

            // Exterior Finish group
            this.radStandard.Checked = true;

            // Summary group
            this.lblVehicleSalePrice.Text = string.Empty;
            this.lblOption.Text = string.Empty;
            this.lblSubtotal.Text = string.Empty;
            this.lblSalesTaxRate.Text = string.Empty;
            this.lblTotal.Text = string.Empty;
            this.lblTradeInValue.Text = string.Empty;
            this.lblAmountDue.Text = string.Empty;

            // Finance group
            this.nudNoOfYear.Value = 1;
            this.nudAnnualInterestRate.Value = 5;
            this.lblMonthlyPayment.Text = string.Empty;
        }

        /// <summary>
        /// Validates input data for the Vehicle Sale Price and Trade-In Value textboxes,
        /// and displays error messages using an error provider.
        /// </summary>
        private void ErrorCheck()
        {
            // Error Provider setting
            this.errorProvider.SetIconPadding(this.txtVehicleSalePrice, 3);
            this.errorProvider.SetIconPadding(this.txtTradeInValue, 3);

            this.errorProvider.SetError(this.txtVehicleSalePrice, string.Empty);
            this.errorProvider.SetError(this.txtTradeInValue, string.Empty) ;

            // Check error for Vehicle sale price textbox
            try
            {
               
                decimal input = Decimal.Parse(this.txtVehicleSalePrice.Text);

                if(input <= 0)
                {
                    DisplayError(this.txtVehicleSalePrice, "Vehicle price cannot be less than or equal to 0.");
                }

            }
            catch (FormatException) 
            {
                if (this.txtVehicleSalePrice.Text == string.Empty)
                {
                    DisplayError(this.txtVehicleSalePrice, "Vehicle price is a required field.");
                }
                else
                {
                    DisplayError(this.txtVehicleSalePrice, "Vehicle price cannot contain letters or special characters.");
                }
            }

            // Check error for Trade in value textbox
            try
            {
                decimal input = Decimal.Parse(this.txtTradeInValue.Text);

                if (input < 0)
                {
                    DisplayError(this.txtTradeInValue, "Trade-in value cannot be less than 0.");
                }

                if (Decimal.Parse(this.txtTradeInValue.Text) > Decimal.Parse(this.txtVehicleSalePrice.Text)) 
                {
                    DisplayError(this.txtTradeInValue, "Trade-in value cannot exceed the vehicle sale price.");
                }

            }
            catch (FormatException)
            {
                if (this.txtTradeInValue.Text == string.Empty)
                {
                    DisplayError(this.txtTradeInValue, "Vehicle price is a required field.");
                }
                else
                {
                    DisplayError(this.txtTradeInValue, "Vehicle price cannot contain letters or special characters.");
                }
            }

        }

        /// <summary>
        /// Displays an error message for the specified control using the error provider.
        /// </summary>
        /// <param name="control">The control for which the error message is displayed.</param>
        /// <param name="errorMessage">The error message to display.</param>
        private void DisplayError(Control control, string errorMessage)
        {
            this.errorProvider.SetError(control, errorMessage);
        }

        /// <summary>
        /// Determines the selected accessories based on the state of checkboxes for stereo system, leather interior, and computer navigation.
        /// </summary>
        /// <returns>An enum value representing the selected accessories.</returns>
        private Accessories CheckAccessories()
        {
            if (chkStereoSystem.Checked)
            {
                if (chkLeatherInterior.Checked)
                {
                    return chkComputerNavigation.Checked ? Accessories.All : Accessories.StereoAndLeather;
                }
                else if (chkComputerNavigation.Checked)
                {
                    return Accessories.StereoAndNavigation;
                }
                else
                {
                    return Accessories.StereoSystem;
                }
            }
            else if (chkLeatherInterior.Checked)
            {
                return chkComputerNavigation.Checked ? Accessories.LeatherAndNavigation : Accessories.LeatherInterior;
            }
            else if (chkComputerNavigation.Checked)
            {
                return Accessories.ComputerNavigation;
            }
            else
            {
                return Accessories.None;
            }
        }

        /// <summary>
        /// Determines the selected exterior finish based on the state of radio buttons for standard, pearlized, and customized detailing.
        /// </summary>
        /// <returns>An enum value representing the selected exterior finish.</returns>
        private ExteriorFinish CheckExteriorFinish()
        {
            ExteriorFinish exteriorFinish = ExteriorFinish.None;
            if (this.radStandard.Checked) 
            {
                exteriorFinish = ExteriorFinish.Standard;
            }
            else if (this.radPearlized.Checked) 
            {
                exteriorFinish = ExteriorFinish.Pearlized;
            }
            else if (this.radCustomizedDetailing.Checked) 
            {
                exteriorFinish = ExteriorFinish.Custom;
            }
            else
            {
                exteriorFinish= ExteriorFinish.None;
            }

            return exteriorFinish;
        }

        /// <summary>
        /// Creates a SalesQuote instance based on user input for vehicle sale price, trade-in value, selected accessories, and exterior finish.
        /// </summary>
        /// <returns>A SalesQuote instance representing the sales quote for a vehicle.</returns>
        private SalesQuote CreateSalesQuote()
        {
            SalesQuote salesQuote;
            // Get user's input
            decimal vehicleSalePrice = Decimal.Parse(this.txtVehicleSalePrice.Text);

            decimal tradeInValue = Decimal.Parse(this.txtTradeInValue.Text);

            Accessories accessoriesChosen = CheckAccessories();

            ExteriorFinish exteriorFinishChosen = CheckExteriorFinish();

            // Create Sales Quote instance
            salesQuote = new SalesQuote(vehicleSalePrice, tradeInValue, 0.12M, accessoriesChosen, exteriorFinishChosen);               
            
            // Return 
            return salesQuote;
        }

        /// <summary>
        /// Sets the summary outputs on the user interface based on the provided SalesQuote instance.
        /// </summary>
        /// <param name="salesQuote">The SalesQuote instance containing calculated values to display.</param>
        private void SetSummaryOutputs(SalesQuote salesQuote)
        {
            this.lblVehicleSalePrice.Text = salesQuote.VehicleSalePrice.ToString("C");

            this.lblOption.Text = salesQuote.TotalOptions.ToString("N2");

            this.lblSubtotal.Text = salesQuote.SubTotal.ToString("C");

            this.lblSalesTaxRate.Text = salesQuote.SalesTax.ToString("N2");

            this.lblTotal.Text = salesQuote.Total.ToString("C");

            this.lblTradeInValue.Text ="-" + salesQuote.TradeInAmount.ToString("N2");

            this.lblAmountDue.Text = salesQuote.AmountDue.ToString("C");
        }

        /// <summary>
        /// Calculates and sets the monthly payment amount on the user interface based on the provided interest rate, number of years, and amount due.
        /// </summary>
        /// <param name="rate">The annual interest rate as a decimal.</param>
        /// <param name="numberOfYears">The number of years for the loan.</param>
        /// <param name="amountDue">The total amount due for the purchase.</param>
        private void SetMonthlyPayment(decimal rate,decimal numberOfYears, decimal amountDue)
        {
            int intNumberOfYears = int.Parse(numberOfYears.ToString());
            int numberofPaymentPeriods = intNumberOfYears * 12;

            this.lblMonthlyPayment.Text = Financial.GetPayment((rate/100/12), numberofPaymentPeriods, amountDue).ToString("C");
        }
    }
}
