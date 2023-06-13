using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace FinancialOverview
{
    public partial class GUI : MetroForm
    {
        private readonly FinancialOverview _financialOverview;
        private DataTable _allSales;

        public GUI()
        {
            InitializeComponent();
            _financialOverview = new FinancialOverview();
            monthlyDataGridView.DataSource = _financialOverview.MonthlySales;
            yearlyDataGridView.DataSource = _financialOverview.YearlySales;
            _allSales = _financialOverview.AllSales;
            allDataGridView.DataSource = _allSales;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void GUI_Resize(object sender, EventArgs e)
        {

        }

        private void monthlySaveButton_Click(object sender, EventArgs e)
        {
            _allSales = _financialOverview.AllSales;
        }

        private void yearlySaveButton_Click(object sender, EventArgs e)
        {
            _allSales = _financialOverview.AllSales;
        }
    }
}
