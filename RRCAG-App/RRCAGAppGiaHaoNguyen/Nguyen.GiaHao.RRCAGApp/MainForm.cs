using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nguyen.GiaHao.RRCAGApp
{
    public partial class MainForm : Form
    {
        private static VehicleDataForm vehicleDataForm;
        public MainForm()
        {
            InitializeComponent();

            /// Handles the Click event of the file open Sales Quote menu item 
            this.mnuFileOpenSalesQuote.Click += MnuFileOpenSalesQuote_Click;

            /// Handles the Click event of the file open Car Wash menu item 
            this.mnuFileOpenCarWash.Click += MnuFileOpenCarWash_Click;

            /// Handles the Click event of the data vehicle menu item 
            this.mnuDataVehicles.Click += MnuDataVehicles_Click;

            /// Handle the Click event of the file exit menu item
            this.mnuFileExit.Click += MnuFileExit_Click;

            /// Handle the Click event of the help about menu item
            this.mnuHelpAbout.Click += MnuAbout_Click;
        }

        /// <summary>
        /// Handles the Click event of the data vehicle menu item 
        /// </summary>
        private void MnuDataVehicles_Click(object sender, EventArgs e)
        {
            if (vehicleDataForm == null || vehicleDataForm.IsDisposed)
            {
                vehicleDataForm = new VehicleDataForm();

                vehicleDataForm.Show();
            }
            else
            {
                vehicleDataForm.BringToFront();
            }

            vehicleDataForm.MdiParent = this;

           
        }

        /// <summary>
        /// Handle the Click event of the file exit menu item
        /// </summary>
        private void MnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

            
        /// <summary>
        /// Handles the Click event of the file open Car Wash menu item 
        /// </summary>
        private void MnuFileOpenCarWash_Click(object sender, EventArgs e)
        {
            CarWashForm form = new CarWashForm();

            form.MdiParent = this;

            form.Show();
        }


        /// <summary>
        /// Handles the Click event of the file open Sales Quote menu item 
        /// </summary>
        private void MnuFileOpenSalesQuote_Click(object sender, EventArgs e)
        {
            SalesQuoteForm form = new SalesQuoteForm();

            form.MdiParent = this;

            form.Show();
        }


        /// <summary>
        /// Handle the Click event of the help about menu item
        /// </summary>
        private void MnuAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }
    }
}
