using ACE.BIT.ADEV.CarWash;
using Nguyen.GiaHao.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nguyen.GiaHao.RRCAGApp
{
    public partial class CarWashForm : ACE.BIT.ADEV.Forms.CarWashForm
    {
        private CarWashInvoice carWashInvoice;

        private BindingSource fragancesSource;
        private BindingSource packagesSource;
        private BindingSource interiorServicesSource;
        private BindingSource exteriorServicesSource;
        private BindingSource carWashInvoiceSource;   

        private BindingList<CarWashItem> fragances;
        private BindingList<Package> packages;
        private BindingList<string> interiorServices;
        private BindingList<string> exteriorServices;

        // Helper variables
        private int pineIndex = -1;

        public CarWashForm()
        {
            // Data Bindings
            this.fragances = new BindingList<CarWashItem>(InitializeFragances());
            this.fragancesSource = new BindingSource();
            this.fragancesSource.DataSource = this.fragances;

            this.packages = new BindingList<Package>(InitializePackages());
            this.packagesSource = new BindingSource();
            this.packagesSource.DataSource = this.packages;

            this.interiorServices = new BindingList<string>();
            this.interiorServicesSource = new BindingSource();
            this.interiorServicesSource.DataSource = this.interiorServices;

            this.exteriorServices = new BindingList<string>();
            this.exteriorServicesSource = new BindingSource();
            this.exteriorServicesSource.DataSource = this.exteriorServices;

            this.carWashInvoiceSource = new BindingSource();
            this.carWashInvoiceSource.DataSource = typeof(CarWashInvoice);

            BindControls();
            
            // Form Load
            InitializeComponent();
            this.cboFragrance.SelectedIndex = this.pineIndex;

            // Events
            this.cboPackage.SelectedIndexChanged += CboPackage_SelectedIndexChanged;
            this.cboFragrance.SelectedIndexChanged += CboFragrance_SelectedIndexChanged;
            this.mnuFileClose.Click += MnuFileClose_Click;
            this.mnuToolsGenerateInvoice.Click += MnuToolsGenerateInvoice_Click;
        }

        /// <summary>
        /// Handles the Click event of the "Generate Invoice" menu item in the Tools menu.
        /// Creates and shows the CarWashInvoiceForm to display the car wash invoice.
        /// </summary>
        private void MnuToolsGenerateInvoice_Click(object sender, EventArgs e)
        {
            CarWashInvoiceForm form = new CarWashInvoiceForm(this.carWashInvoice, this);

            form.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the "Close" menu item in the File menu.
        /// Closes the CarWashForm.
        /// </summary>
        private void MnuFileClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the index change event of combo box Fragance.
        /// </summary>
        private void CboFragrance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cboFragrance.SelectedIndex != -1)
            {
                UpdateFraganceShit();
            }

            UpdateCarWashInvoice();
            carWashBindControls();
        }

        /// <summary>
        /// This method help to change the description of the first interior service called "Fragance- {Current selected fragance}" to match the current select fragance.
        /// </summary>
        private void UpdateFraganceShit()
        {
            if(this.cboPackage.SelectedIndex != -1)
            {
                this.interiorServices.RemoveAt(0);
                this.interiorServices.Insert(0, "Fragance-" + this.cboFragrance.SelectedItem.ToString());
            }        
        }

        /// <summary>
        /// Handles the index change event of combo box Package
        /// </summary>
        private void CboPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.interiorServices.Clear();
            this.exteriorServices.Clear();

            if (this.cboPackage.SelectedIndex != -1) 
            {              
                foreach(string service in this.packages[this.cboPackage.SelectedIndex].InteriorServices) 
                {
                    this.interiorServices.Add(service);
                }

                foreach(string service in this.packages[this.cboPackage.SelectedIndex].ExteriorServices)
                {
                    this.exteriorServices.Add(service);
                }

                UpdateFraganceShit();
            }

            UpdateCarWashInvoice();
            carWashBindControls();
        }

        /// <summary>
        /// This method help to bind this form's controls to their datasources
        /// </summary>
        private void BindControls()
        {
            this.cboFragrance.DataSource = this.fragancesSource;    

            this.cboPackage.DataSource = this.packagesSource;

            this.lstInterior.DataSource = this.interiorServicesSource;

            this.lstExterior.DataSource = this.exteriorServicesSource;

            carWashBindControls();
        }

        /// <summary>
        /// This method help to bind labels to the car wash invoice source
        /// </summary>
        private void carWashBindControls()
        {
            this.lblSubtotal.DataBindings.Clear();
            this.lblProvincialSalesTax.DataBindings.Clear();
            this.lblGoodsAndServicesTax.DataBindings.Clear();
            this.lblTotal.DataBindings.Clear();

            Binding carWashBind;

            carWashBind = new Binding("Text", this.carWashInvoiceSource, "Subtotal", true);
            carWashBind.FormatString = "C";
            this.lblSubtotal.DataBindings.Add(carWashBind);

            carWashBind = new Binding("Text", this.carWashInvoiceSource, "ProvincialSalesTaxCharged", true);
            carWashBind.FormatString = "N2";
            this.lblProvincialSalesTax.DataBindings.Add(carWashBind);

            carWashBind = new Binding("Text", this.carWashInvoiceSource, "GoodsAndServicesTaxCharged", true);
            carWashBind.FormatString = "N2";
            this.lblGoodsAndServicesTax.DataBindings.Add(carWashBind);

            carWashBind = new Binding("Text", this.carWashInvoiceSource, "Total", true);
            carWashBind.FormatString = "C";
            this.lblTotal.DataBindings.Add(carWashBind);
        }

        /// <summary>
        /// This method will create or update if exists the form's car wash invoice
        /// </summary>
        private void UpdateCarWashInvoice()
        {
            if(this.cboPackage.SelectedIndex != -1 && this.Text != "[Title]") 
            {
                if(this.carWashInvoice == null)
                {
                    this.carWashInvoice = new CarWashInvoice(0.7M, 0.5M, this.packages[this.cboPackage.SelectedIndex].Price, this.fragances[this.cboFragrance.SelectedIndex].Price);
                    
                }      
                else
                {
                    this.carWashInvoice.PackageCost = this.packages[this.cboPackage.SelectedIndex].Price;
                    this.carWashInvoice.FragranceCost = this.fragances[this.cboFragrance.SelectedIndex].Price;

                }

                this.carWashInvoiceSource.DataSource = this.carWashInvoice;

                this.mnuToolsGenerateInvoice.Enabled = true;
            }
        }      

        /// <summary>
        /// Initializes the list of car wash packages available on this form.
        /// </summary>
        /// <returns>A list of Package objects representing the available car wash packages.</returns>
        private List<Package> InitializePackages()
        {       
            /*Note: All these lists must have same number of elements*/
            List<string> interiorServices = new List<string> { "Fragance-", "Shampoo Carpets", "Shampoo Upholstery", "Interior Protection Coat"};
            List<string> exteriorServices = new List<string> { "Hand Wash", "Hand Wax", "Wheel Polish", "Detail Engine Compartment"};
            List<string> packagesDescription = new List<string> { "Standard", "Deluxe", "Executive", "Luxury"};
            List<decimal> packagesPrice = new List<decimal> { 7.5M, 15.0M, 35.0M, 55.0M};
            
            return packagesGenerator(interiorServices, exteriorServices, packagesDescription, packagesPrice);
        }


        /// <summary>
        /// Generates a list of car wash packages based on provided parameters.
        /// </summary>
        /// <param name="interiorServices">The list of interior services for each package.</param>
        /// <param name="exteriorServices">The list of exterior services for each package.</param>
        /// <param name="packagesDescription">The description of each car wash package.</param>
        /// <param name="packagesPrice">The price of each car wash package.</param>
        /// <returns>A list of Package objects representing the generated car wash packages.</returns>
        private List<Package> packagesGenerator(List<string> interiorServices, List<string> exteriorServices, List<string> packagesDescription, List<decimal> packagesPrice)
        {
            List<Package> packages = new List<Package>();

            for (int i = 0; i < packagesDescription.Count; i++)
            {
                int servicesToRemove = packagesDescription.Count - i - 1;

                List<string> currentInteriorServices = new List<string>(interiorServices);
                List<string> currentExteriorServices = new List<string>(exteriorServices);

                currentInteriorServices.RemoveRange(i + 1, servicesToRemove);
                currentExteriorServices.RemoveRange(i + 1, servicesToRemove);

                packages.Add(new Package(packagesDescription[i], packagesPrice[i], currentInteriorServices, currentExteriorServices));
            }

            return packages;
        }

        /// <summary>
        /// Initializes the list of car wash fragrances based on data from the "fragrances.txt" file.
        /// </summary>
        /// <returns>A sorted list of CarWashItem objects representing available fragrances, including a default "Pine" fragrance.</returns>
        private List<CarWashItem> InitializeFragances()
        {
            try
            {
                string filepath = "fragrances.txt";

                FileStream fileStream;
                fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

                StreamReader fileReader;
                fileReader = new StreamReader(fileStream);

                List<CarWashItem> fragances = new List<CarWashItem>();
                fragances.Add(new CarWashItem("Pine", 0.0M));

                CarWashItem carWashItem;

                // Loop while there is more data to read
                while (fileReader.Peek() != -1)
                {
                    // Read a record (line) from the file
                    string record = fileReader.ReadLine();

                    char[] delimiters = { ',' };

                    // Each field is stored as an element in the array
                    string[] fields = record.Split(delimiters);

                    string description = fields[0];

                    decimal price = decimal.Parse(fields[1]);

                    carWashItem = new CarWashItem(description, price);

                    fragances.Add(carWashItem);
                }

                // Sort the frangances items in alphabetical order
                fragances = fragances.OrderBy(item => item.Description).ToList();

                // help to keep track of pine fragance item index
                pineIndex = fragances.FindIndex(item => item.Description == "Pine");

                fileReader.Close();

                return fragances;
            }          
            catch(FileNotFoundException) 
            {
                MessageBox.Show("Fragances data file not found.", "Data File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("An error occured while reading the data file.", "Data File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return new List<CarWashItem>();
        }

        /// <summary>
        /// Initializes the CarWashForm to its initial state.
        /// </summary>
        public void SetInitial()
        {
            InitializeComponent();
            this.mnuToolsGenerateInvoice.Enabled = false;
            this.cboPackage.SelectedIndex = -1;
            this.cboFragrance.SelectedIndex = this.pineIndex;

            // Reset labels to empty strings
            this.lblSubtotal.Text = string.Empty;
            this.lblProvincialSalesTax.Text = string.Empty;
            this.lblGoodsAndServicesTax.Text = string.Empty;
            this.lblTotal.Text = string.Empty;
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // lstExterior
            // 
            this.lstExterior.ItemHeight = 16;
            // 
            // lstInterior
            // 
            this.lstInterior.ItemHeight = 16;
            // 
            // CarWashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(527, 528);
            this.Name = "CarWashForm";
            this.Text = "Car Wash";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.cboPackage.SelectedIndex = -1;
        }
    }
}
