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
        private ComponentResourceManager _resources = new ComponentResourceManager(typeof(GUI));

        public GUI()
        {
            InitializeComponent();
            _financialOverview = new FinancialOverview();
            _financialOverview.LoadData();
            monthlyDataGridView.DataSource = _financialOverview.MonthlySales;
            yearlyDataGridView.DataSource = _financialOverview.YearlySales;
            _allSales = _financialOverview.AllSales;
            allDataGridView.DataSource = _allSales;
        }

        private void GUI_Resize(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _financialOverview.SaveData();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _financialOverview.SaveData(null);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _financialOverview.LoadData(null);
        }

        private void unitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _financialOverview.UnitOfAll = (FinancialOverview.Unit) unitComboBox.SelectedIndex;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            _financialOverview.LoadData();
            unitComboBox.SelectedIndex = (int) _financialOverview.UnitOfAll;
        }

        private void UpdateGui()
        {
            _allSales = _financialOverview.AllSales;
            restTextBox.Text = $"{_financialOverview.GetRest()}{_resources.GetString("Unit")} ";
        }
    }
}
