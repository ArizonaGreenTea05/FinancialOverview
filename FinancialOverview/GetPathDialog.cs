using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialOverview
{
    public partial class GetPathDialog : Form
    {
        private string _filePath = string.Empty;

        public string Filepath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                filepathTextBox.Text = _filePath;
            }
        }

        public GetPathDialog()
        {
            InitializeComponent();
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            Filepath = filepathTextBox.Text;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Filepath = string.Empty;
        }
    }
}
