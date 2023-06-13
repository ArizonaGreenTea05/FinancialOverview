namespace FinancialOverview
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.monthlyDataGridView = new System.Windows.Forms.DataGridView();
            this.yearlyDataGridView = new System.Windows.Forms.DataGridView();
            this.allDataGridView = new System.Windows.Forms.DataGridView();
            this.restTextBox = new MetroFramework.Controls.MetroTextBox();
            this.all_label = new MetroFramework.Controls.MetroLabel();
            this.monthly_label = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.updateButton = new MetroFramework.Controls.MetroButton();
            this.unitComboBox = new MetroFramework.Controls.MetroComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gUIBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gUIBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.runBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.monthlyDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearlyDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            resources.ApplyResources(this.metroLabel1, "metroLabel1");
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // monthlyDataGridView
            // 
            this.monthlyDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.monthlyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.monthlyDataGridView, "monthlyDataGridView");
            this.monthlyDataGridView.Name = "monthlyDataGridView";
            // 
            // yearlyDataGridView
            // 
            this.yearlyDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.yearlyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.yearlyDataGridView, "yearlyDataGridView");
            this.yearlyDataGridView.Name = "yearlyDataGridView";
            // 
            // allDataGridView
            // 
            this.allDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.allDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.allDataGridView, "allDataGridView");
            this.allDataGridView.Name = "allDataGridView";
            this.allDataGridView.TabStop = false;
            // 
            // restTextBox
            // 
            // 
            // 
            // 
            this.restTextBox.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.restTextBox.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.restTextBox.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.restTextBox.CustomButton.Name = "";
            this.restTextBox.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.restTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.restTextBox.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.restTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.restTextBox.CustomButton.UseSelectable = true;
            this.restTextBox.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.restTextBox.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.restTextBox.FontWeight = MetroFramework.MetroTextBoxWeight.Bold;
            this.restTextBox.Lines = new string[0];
            resources.ApplyResources(this.restTextBox, "restTextBox");
            this.restTextBox.MaxLength = 32767;
            this.restTextBox.Name = "restTextBox";
            this.restTextBox.PasswordChar = '\0';
            this.restTextBox.ReadOnly = true;
            this.restTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.restTextBox.SelectedText = "";
            this.restTextBox.SelectionLength = 0;
            this.restTextBox.SelectionStart = 0;
            this.restTextBox.ShortcutsEnabled = false;
            this.restTextBox.TabStop = false;
            this.restTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.restTextBox.UseSelectable = true;
            this.restTextBox.UseStyleColors = true;
            this.restTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.restTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // all_label
            // 
            resources.ApplyResources(this.all_label, "all_label");
            this.all_label.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.all_label.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.all_label.Name = "all_label";
            // 
            // monthly_label
            // 
            resources.ApplyResources(this.monthly_label, "monthly_label");
            this.monthly_label.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.monthly_label.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.monthly_label.Name = "monthly_label";
            // 
            // metroLabel8
            // 
            resources.ApplyResources(this.metroLabel8, "metroLabel8");
            this.metroLabel8.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel8.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel8.Name = "metroLabel8";
            // 
            // updateButton
            // 
            resources.ApplyResources(this.updateButton, "updateButton");
            this.updateButton.Name = "updateButton";
            this.updateButton.UseSelectable = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // unitComboBox
            // 
            this.unitComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.unitComboBox, "unitComboBox");
            this.unitComboBox.Items.AddRange(new object[] {
            resources.GetString("unitComboBox.Items"),
            resources.GetString("unitComboBox.Items1")});
            this.unitComboBox.Name = "unitComboBox";
            this.unitComboBox.UseSelectable = true;
            this.unitComboBox.SelectedIndexChanged += new System.EventHandler(this.unitComboBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            resources.ApplyResources(this.loadToolStripMenuItem, "loadToolStripMenuItem");
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // gUIBindingSource
            // 
            this.gUIBindingSource.RaiseListChangedEvents = false;
            // 
            // gUIBindingSource1
            // 
            this.gUIBindingSource1.RaiseListChangedEvents = false;
            // 
            // runBindingSource
            // 
            this.runBindingSource.RaiseListChangedEvents = false;
            // 
            // GUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.unitComboBox);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.monthly_label);
            this.Controls.Add(this.all_label);
            this.Controls.Add(this.restTextBox);
            this.Controls.Add(this.allDataGridView);
            this.Controls.Add(this.yearlyDataGridView);
            this.Controls.Add(this.monthlyDataGridView);
            this.Controls.Add(this.metroLabel1);
            this.Name = "GUI";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            this.Resize += new System.EventHandler(this.GUI_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.monthlyDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearlyDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource gUIBindingSource;
        private System.Windows.Forms.BindingSource gUIBindingSource1;
        private System.Windows.Forms.BindingSource runBindingSource;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.DataGridView monthlyDataGridView;
        private System.Windows.Forms.DataGridView yearlyDataGridView;
        private System.Windows.Forms.DataGridView allDataGridView;
        private MetroFramework.Controls.MetroTextBox restTextBox;
        private MetroFramework.Controls.MetroLabel all_label;
        private MetroFramework.Controls.MetroLabel monthly_label;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroButton updateButton;
        private MetroFramework.Controls.MetroComboBox unitComboBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    }
}

