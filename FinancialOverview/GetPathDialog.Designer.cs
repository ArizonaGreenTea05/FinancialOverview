namespace FinancialOverview
{
    partial class GetPathDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetPathDialog));
            this.filepathTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.okayButton = new MetroFramework.Controls.MetroButton();
            this.cancelButton = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // filepathTextBox
            // 
            // 
            // 
            // 
            this.filepathTextBox.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.filepathTextBox.CustomButton.Location = ((System.Drawing.Point)(resources.GetObject("resource.Location")));
            this.filepathTextBox.CustomButton.Name = "";
            this.filepathTextBox.CustomButton.Size = ((System.Drawing.Size)(resources.GetObject("resource.Size")));
            this.filepathTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.filepathTextBox.CustomButton.TabIndex = ((int)(resources.GetObject("resource.TabIndex")));
            this.filepathTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.filepathTextBox.CustomButton.UseSelectable = true;
            this.filepathTextBox.CustomButton.Visible = ((bool)(resources.GetObject("resource.Visible")));
            this.filepathTextBox.Lines = new string[0];
            resources.ApplyResources(this.filepathTextBox, "filepathTextBox");
            this.filepathTextBox.MaxLength = 32767;
            this.filepathTextBox.Name = "filepathTextBox";
            this.filepathTextBox.PasswordChar = '\0';
            this.filepathTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.filepathTextBox.SelectedText = "";
            this.filepathTextBox.SelectionLength = 0;
            this.filepathTextBox.SelectionStart = 0;
            this.filepathTextBox.ShortcutsEnabled = true;
            this.filepathTextBox.UseSelectable = true;
            this.filepathTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.filepathTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            resources.ApplyResources(this.metroLabel1, "metroLabel1");
            this.metroLabel1.Name = "metroLabel1";
            // 
            // okayButton
            // 
            this.okayButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okayButton, "okayButton");
            this.okayButton.Name = "okayButton";
            this.okayButton.UseSelectable = true;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseSelectable = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // GetPathDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.filepathTextBox);
            this.Name = "GetPathDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox filepathTextBox;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton okayButton;
        private MetroFramework.Controls.MetroButton cancelButton;
    }
}