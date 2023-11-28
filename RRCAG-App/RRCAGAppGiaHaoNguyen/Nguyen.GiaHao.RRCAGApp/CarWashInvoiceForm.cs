using Nguyen.GiaHao.Business;
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
    public partial class CarWashInvoiceForm : ACE.BIT.ADEV.Forms.CarWashInvoiceForm
    {
        BindingSource carWashInvoiceSource;
        CarWashForm carWashForm;

        public CarWashInvoiceForm(CarWashInvoice carWashInvoice, CarWashForm carWashForm)
        {
            this.carWashInvoiceSource = new BindingSource();
            this.carWashInvoiceSource.DataSource = carWashInvoice;

            this.carWashForm = carWashForm;

            BindControls();

            InitializeComponent();

            this.FormClosed += CarWashInvoiceForm_FormClosed;
        }

        /// <summary>
        /// Handles the FormClosed event of the CarWashInvoiceForm.
        /// Resets the CarWashForm to its initial state when the invoice form is closed.
        /// </summary>
        private void CarWashInvoiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            carWashForm.SetInitial();
        }

        /// <summary>
        /// Binds the controls on the CarWashInvoiceForm to the CarWashInvoiceSource.
        /// </summary>
        private void BindControls() 
        {
            Binding carWashInvoiceBinding;

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "PackageCost", true);
            carWashInvoiceBinding.FormatString = "C";
            this.lblPackagePrice.DataBindings.Add(carWashInvoiceBinding);

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "FragranceCost", true);
            carWashInvoiceBinding.FormatString = "N2";
            this.lblFragrancePrice.DataBindings.Add(carWashInvoiceBinding);

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "SubTotal", true);
            carWashInvoiceBinding.FormatString = "C";
            this.lblSubtotal.DataBindings.Add(carWashInvoiceBinding);

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "ProvincialSalesTaxCharged", true);
            carWashInvoiceBinding.FormatString = "N2";
            this.lblProvincialSalesTax.DataBindings.Add(carWashInvoiceBinding);

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "GoodsAndServicesTaxCharged", true);
            carWashInvoiceBinding.FormatString = "N2";
            this.lblGoodsAndServicesTax.DataBindings.Add(carWashInvoiceBinding);

            carWashInvoiceBinding = new Binding("Text", carWashInvoiceSource, "Total", true);
            carWashInvoiceBinding.FormatString = "C";
            this.lblTotal.DataBindings.Add(carWashInvoiceBinding);
        }

      
    }
}
