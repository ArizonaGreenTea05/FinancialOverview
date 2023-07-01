using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace WinFormsApp
{
    public partial class GUI : MetroForm
    {
        private readonly BusinessLogic.FinancialOverview _financialOverview;
        private DataTable _allSales;
        private readonly ComponentResourceManager _resources = new ComponentResourceManager(typeof(GUI));
        private const string FILE_FILTER_FOR_XML_FILES = "XML files (.xml)|*.xml";
        private FormWindowState _lastWindowState = FormWindowState.Minimized;
        private bool _guiLoaded = false;
        private readonly string _appDataFilePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData) + @"\WinFormsFinance", "WinFormsFinance.AppData");

        public GUI()
        {
            InitializeComponent();
            _financialOverview = new BusinessLogic.FinancialOverview();
            _financialOverview.DefaultFilePath = LoadStringFromAppData().Split('\r', '\n')[0];
            _financialOverview.OnDefaultFilePathChanged += OnDefaultFilePathChanged;
            if (!_financialOverview.LoadData())
                _financialOverview.LoadData(GetFilepathFromUser("", "", ".xml", FILE_FILTER_FOR_XML_FILES, FileDialogType.Open));
            monthlyDataGridView.DataSource = _financialOverview.MonthlySales;
            yearlyDataGridView.DataSource = _financialOverview.YearlySales;
            _allSales = _financialOverview.AllSales;
            allDataGridView.DataSource = _allSales;
            KeyPreview = true;
        }

        private void OnDefaultFilePathChanged(object sender, string path)
        {
            SaveStringToAppData(path);
        }

        private void SaveStringToAppData(string s)
        {
            if(!Directory.Exists(_appDataFilePath)) Directory.CreateDirectory(Path.GetDirectoryName(_appDataFilePath));
            File.WriteAllText(_appDataFilePath, s);
        }

        private string LoadStringFromAppData()
        {
            return File.Exists(_appDataFilePath)
                ? File.ReadAllText(_appDataFilePath)
                : string.Empty;
        }

        private void GUI_Resize(object sender, EventArgs e)
        {
            ResizeGui();
            if (WindowState != _lastWindowState)
            {
                _lastWindowState = WindowState;
                if (WindowState != FormWindowState.Minimized)
                {
                    ResizeGui();
                    ResizeDataGridViews();
                }
            }
        }

        private void GUI_ResizeEnd(object sender, EventArgs e)
        {
            ResizeDataGridViews();
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
            var filepath = GetFilepathFromUser(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, ".xml",
                FILE_FILTER_FOR_XML_FILES, FileDialogType.Save);
            if (null != filepath) _financialOverview.SaveData(filepath);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filepath = GetFilepathFromUser(_financialOverview.DefaultDirectory, _financialOverview.DefaultFilename, ".xml",
                FILE_FILTER_FOR_XML_FILES, FileDialogType.Open);
            if (null != filepath) _financialOverview.LoadData(filepath);
        }

        private void unitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _financialOverview.UnitOfAll = (BusinessLogic.FinancialOverview.Unit)unitComboBox.SelectedIndex;
            UpdateGui();
        }

        private void GUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                _financialOverview.SaveData();
                e.SuppressKeyPress = true;
            }
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            UpdateGui();
            _guiLoaded = true;
            ResizeDataGridViews();
        }

        private void UpdateGui()
        {
            _allSales = _financialOverview.AllSales;
            unitComboBox.SelectedIndex = (int)_financialOverview.UnitOfAll;
            restTextBox.Text =
                $"{_financialOverview.GetRest().ToString("F")}{_resources.GetString("Unit") ?? "€"} ";
        }

        private void ResizeGui()
        {
            var restTextBoxBounds = restTextBox.Bounds;
            var unitComboBoxBounds = unitComboBox.Bounds;
            var monthlyDataGridViewBounds = monthlyDataGridView.Bounds;
            var monthlyLabelBounds = monthlyLabel.Bounds;
            var allDataGridViewBounds = allDataGridView.Bounds;
            var yearlyDataGridViewBounds = yearlyDataGridView.Bounds;
            var yearlyLabelBounds = yearlyLabel.Bounds;
            var updateButtonBounds = updateButton.Bounds;

            restTextBoxBounds.Width = Width / 2 - unitComboBoxBounds.Width - restTextBox.Margin.Right
                                      - unitComboBox.Margin.Left - 60;
            restTextBoxBounds.Y = unitComboBoxBounds.Y = allDataGridViewBounds.Y + allDataGridViewBounds.Height +
                                                         allDataGridView.Margin.Bottom + restTextBox.Margin.Top;
            unitComboBoxBounds.X = restTextBoxBounds.X + restTextBoxBounds.Width + restTextBox.Margin.Right +
                                   unitComboBox.Margin.Left;
            monthlyDataGridViewBounds.Width = yearlyDataGridViewBounds.Width = allDataGridViewBounds.Width =
                restTextBoxBounds.Width + restTextBox.Margin.Right + unitComboBox.Margin.Left + unitComboBoxBounds.Width;
            monthlyDataGridViewBounds.X = yearlyDataGridViewBounds.X = monthlyLabelBounds.X = yearlyLabelBounds.X
                = Width / 2 + 30;
            monthlyDataGridViewBounds.Height = yearlyDataGridViewBounds.Height
                = (allDataGridViewBounds.Height + allDataGridView.Margin.Bottom + unitComboBox.Margin.Top +
                    unitComboBox.Height - 32) / 2;
            allDataGridViewBounds.Height = Height - 174;
            yearlyDataGridViewBounds.Y = monthlyDataGridViewBounds.Y + monthlyDataGridViewBounds.Height + 32;
            yearlyLabelBounds.Y = yearlyDataGridViewBounds.Y - yearlyLabelBounds.Height - yearlyDataGridView.Margin.Top;
            updateButtonBounds.X = allDataGridViewBounds.X + allDataGridViewBounds.Width - updateButtonBounds.Width;
            
            restTextBox.Bounds = restTextBoxBounds;
            unitComboBox.Bounds = unitComboBoxBounds;
            monthlyDataGridView.Bounds = monthlyDataGridViewBounds;
            monthlyLabel.Bounds = monthlyLabelBounds;
            allDataGridView.Bounds = allDataGridViewBounds;
            yearlyDataGridView.Bounds = yearlyDataGridViewBounds;
            yearlyLabel.Bounds = yearlyLabelBounds;
            updateButton.Bounds = updateButtonBounds;
        }

        private void ResizeDataGridViews()
        {
            if (!_guiLoaded) return;
            monthlyDataGridView.Columns["Sales"].Width =
                yearlyDataGridView.Columns["Sales"].Width =
                    allDataGridView.Columns["Sales"].Width = monthlyDataGridView.Bounds.Width / 6 - 18;
            monthlyDataGridView.Columns["Name"].Width =
                yearlyDataGridView.Columns["Name"].Width =
                    allDataGridView.Columns["Name"].Width = monthlyDataGridView.Bounds.Width / 3 - 20;
            monthlyDataGridView.Columns["Addition"].Width =
                yearlyDataGridView.Columns["Addition"].Width =
                    allDataGridView.Columns["Addition"].Width = monthlyDataGridView.Bounds.Width / 2 - 5;
        }

        public enum FileDialogType
        {
            Save,
            Open
        }

        private static string GetFilepathFromUser(string defaultDirectory, string defaultFilename, string defaultExtension, string filter, FileDialogType type)
        {
            FileDialog dialog;
            if (type == FileDialogType.Save)
                dialog = new SaveFileDialog();
            else
                dialog = new OpenFileDialog();
            dialog.Title = $@"Select {defaultExtension}";
            dialog.InitialDirectory = Directory.Exists(defaultDirectory) ? defaultDirectory : Directory.GetCurrentDirectory();
            dialog.FileName = defaultFilename;
            dialog.DefaultExt = defaultExtension;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == DialogResult.Cancel) return null;
            return dialog.FileName;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _financialOverview.Undo();
            UpdateGui();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _financialOverview.Redo();
            UpdateGui();
        }
    }
}
