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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.file_toolStripItem = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gUIBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gUIBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.runBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.monthlyDataGridView = new System.Windows.Forms.DataGridView();
            this.yearlyDataGridView = new System.Windows.Forms.DataGridView();
            this.allDataGridView = new System.Windows.Forms.DataGridView();
            this.metroTextBox7 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox8 = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox9 = new MetroFramework.Controls.MetroTextBox();
            this.all_label = new MetroFramework.Controls.MetroLabel();
            this.monthly_label = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.monthlySaveButton = new MetroFramework.Controls.MetroButton();
            this.yearlySaveButton = new MetroFramework.Controls.MetroButton();
            this.miniToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthlyDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearlyDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.file_toolStripItem });
            resources.ApplyResources(this.miniToolStrip, "miniToolStrip");
            this.miniToolStrip.Name = "miniToolStrip";
            // 
            // file_toolStripItem
            // 
            this.file_toolStripItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.file_toolStripItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.loadToolStripMenuItem });
            resources.ApplyResources(this.file_toolStripItem, "file_toolStripItem");
            this.file_toolStripItem.Name = "file_toolStripItem";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
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
            // metroTextBox7
            // 
            // 
            // 
            // 
            this.metroTextBox7.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.metroTextBox7.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode")));
            this.metroTextBox7.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.metroTextBox7.CustomButton.Name = "";
            this.metroTextBox7.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.metroTextBox7.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox7.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.metroTextBox7.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox7.CustomButton.UseSelectable = true;
            this.metroTextBox7.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            resources.ApplyResources(this.metroTextBox7, "metroTextBox7");
            this.metroTextBox7.Lines = new string[0];
            this.metroTextBox7.MaxLength = 32767;
            this.metroTextBox7.Name = "metroTextBox7";
            this.metroTextBox7.PasswordChar = '\0';
            this.metroTextBox7.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox7.SelectedText = "";
            this.metroTextBox7.SelectionLength = 0;
            this.metroTextBox7.SelectionStart = 0;
            this.metroTextBox7.ShortcutsEnabled = true;
            this.metroTextBox7.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox7.TabStop = false;
            this.metroTextBox7.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox7.UseSelectable = true;
            this.metroTextBox7.UseStyleColors = true;
            this.metroTextBox7.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox7.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox8
            // 
            // 
            // 
            // 
            this.metroTextBox8.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.metroTextBox8.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode1")));
            this.metroTextBox8.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location1")));
            this.metroTextBox8.CustomButton.Name = "";
            this.metroTextBox8.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size1")));
            this.metroTextBox8.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox8.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex1")));
            this.metroTextBox8.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox8.CustomButton.UseSelectable = true;
            this.metroTextBox8.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible1")));
            resources.ApplyResources(this.metroTextBox8, "metroTextBox8");
            this.metroTextBox8.Lines = new string[0];
            this.metroTextBox8.MaxLength = 32767;
            this.metroTextBox8.Name = "metroTextBox8";
            this.metroTextBox8.PasswordChar = '\0';
            this.metroTextBox8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox8.SelectedText = "";
            this.metroTextBox8.SelectionLength = 0;
            this.metroTextBox8.SelectionStart = 0;
            this.metroTextBox8.ShortcutsEnabled = true;
            this.metroTextBox8.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox8.TabStop = false;
            this.metroTextBox8.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox8.UseSelectable = true;
            this.metroTextBox8.UseStyleColors = true;
            this.metroTextBox8.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox8.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox9
            // 
            // 
            // 
            // 
            this.metroTextBox9.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            this.metroTextBox9.CustomButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("resource.ImeMode2")));
            this.metroTextBox9.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location2")));
            this.metroTextBox9.CustomButton.Name = "";
            this.metroTextBox9.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size2")));
            this.metroTextBox9.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox9.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex2")));
            this.metroTextBox9.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox9.CustomButton.UseSelectable = true;
            this.metroTextBox9.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible2")));
            resources.ApplyResources(this.metroTextBox9, "metroTextBox9");
            this.metroTextBox9.Lines = new string[0];
            this.metroTextBox9.MaxLength = 32767;
            this.metroTextBox9.Name = "metroTextBox9";
            this.metroTextBox9.PasswordChar = '\0';
            this.metroTextBox9.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox9.SelectedText = "";
            this.metroTextBox9.SelectionLength = 0;
            this.metroTextBox9.SelectionStart = 0;
            this.metroTextBox9.ShortcutsEnabled = true;
            this.metroTextBox9.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox9.TabStop = false;
            this.metroTextBox9.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox9.UseSelectable = true;
            this.metroTextBox9.UseStyleColors = true;
            this.metroTextBox9.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox9.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
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
            // monthlySaveButton
            // 
            resources.ApplyResources(this.monthlySaveButton, "monthlySaveButton");
            this.monthlySaveButton.Name = "monthlySaveButton";
            this.monthlySaveButton.UseSelectable = true;
            this.monthlySaveButton.Click += new System.EventHandler(this.monthlySaveButton_Click);
            // 
            // yearlySaveButton
            // 
            resources.ApplyResources(this.yearlySaveButton, "yearlySaveButton");
            this.yearlySaveButton.Name = "yearlySaveButton";
            this.yearlySaveButton.UseSelectable = true;
            this.yearlySaveButton.Click += new System.EventHandler(this.yearlySaveButton_Click);
            // 
            // GUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.yearlySaveButton);
            this.Controls.Add(this.monthlySaveButton);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.monthly_label);
            this.Controls.Add(this.all_label);
            this.Controls.Add(this.metroTextBox7);
            this.Controls.Add(this.metroTextBox8);
            this.Controls.Add(this.metroTextBox9);
            this.Controls.Add(this.allDataGridView);
            this.Controls.Add(this.yearlyDataGridView);
            this.Controls.Add(this.monthlyDataGridView);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.miniToolStrip);
            this.Name = "GUI";
            this.Resize += new System.EventHandler(this.GUI_Resize);
            this.miniToolStrip.ResumeLayout(false);
            this.miniToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gUIBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthlyDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearlyDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton file_toolStripItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.BindingSource gUIBindingSource;
        private System.Windows.Forms.BindingSource gUIBindingSource1;
        private System.Windows.Forms.BindingSource runBindingSource;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.DataGridView monthlyDataGridView;
        private System.Windows.Forms.DataGridView yearlyDataGridView;
        private System.Windows.Forms.DataGridView allDataGridView;
        private MetroFramework.Controls.MetroTextBox metroTextBox7;
        private MetroFramework.Controls.MetroTextBox metroTextBox8;
        private MetroFramework.Controls.MetroTextBox metroTextBox9;
        private MetroFramework.Controls.MetroLabel all_label;
        private MetroFramework.Controls.MetroLabel monthly_label;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroButton monthlySaveButton;
        private MetroFramework.Controls.MetroButton yearlySaveButton;
    }
}

