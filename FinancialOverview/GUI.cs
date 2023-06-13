using System;
using System.ComponentModel;
using System.Data;
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
            KeyPreview = true;
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
            var dialog = new GetPathDialog();
            dialog.Filepath = _financialOverview.DefaultPath;
            if (dialog.ShowDialog() == DialogResult.OK)
                _financialOverview.SaveData(dialog.Filepath);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new GetPathDialog();
            dialog.Filepath = _financialOverview.DefaultPath;
            if (dialog.ShowDialog() == DialogResult.OK)
                _financialOverview.LoadData(dialog.Filepath);
        }

        private void unitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _financialOverview.UnitOfAll = (FinancialOverview.Unit) unitComboBox.SelectedIndex;
            UpdateGui();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void UpdateGui()
        {
            _allSales = _financialOverview.AllSales;
            unitComboBox.SelectedIndex = (int)_financialOverview.UnitOfAll;
            restTextBox.Text = $"{_financialOverview.GetRest()}{_resources.GetString("Unit")} ";
        }

        private void GUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                _financialOverview.SaveData();
                e.SuppressKeyPress = true;
            }
        }
    }
}
