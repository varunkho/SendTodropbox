namespace SDB
{
    partial class RenameForm
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
            this.radioButtonPrefix = new System.Windows.Forms.RadioButton();
            this.radioButtonSuffix = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFix = new System.Windows.Forms.TextBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.chkNumbered = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // radioButtonPrefix
            // 
            this.radioButtonPrefix.AutoSize = true;
            this.radioButtonPrefix.Location = new System.Drawing.Point(5, 10);
            this.radioButtonPrefix.Name = "radioButtonPrefix";
            this.radioButtonPrefix.Size = new System.Drawing.Size(51, 17);
            this.radioButtonPrefix.TabIndex = 1;
            this.radioButtonPrefix.TabStop = true;
            this.radioButtonPrefix.Text = "&Prefix";
            this.radioButtonPrefix.UseVisualStyleBackColor = true;
            // 
            // radioButtonSuffix
            // 
            this.radioButtonSuffix.AutoSize = true;
            this.radioButtonSuffix.Location = new System.Drawing.Point(61, 10);
            this.radioButtonSuffix.Name = "radioButtonSuffix";
            this.radioButtonSuffix.Size = new System.Drawing.Size(51, 17);
            this.radioButtonSuffix.TabIndex = 2;
            this.radioButtonSuffix.TabStop = true;
            this.radioButtonSuffix.Text = "&Suffix";
            this.radioButtonSuffix.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 18);
            this.label1.TabIndex = 3;
            // 
            // textBoxFix
            // 
            this.textBoxFix.Location = new System.Drawing.Point(55, 33);
            this.textBoxFix.Name = "textBoxFix";
            this.textBoxFix.Size = new System.Drawing.Size(100, 20);
            this.textBoxFix.TabIndex = 4;
            this.textBoxFix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFix_KeyPress);
            // 
            // buttonRename
            // 
            this.buttonRename.Location = new System.Drawing.Point(5, 63);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(75, 23);
            this.buttonRename.TabIndex = 5;
            this.buttonRename.Text = "&Rename";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(85, 63);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // chkNumbered
            // 
            this.chkNumbered.AutoSize = true;
            this.chkNumbered.Location = new System.Drawing.Point(123, 10);
            this.chkNumbered.Name = "chkNumbered";
            this.chkNumbered.Size = new System.Drawing.Size(75, 17);
            this.chkNumbered.TabIndex = 7;
            this.chkNumbered.Text = "Numbered";
            this.chkNumbered.UseVisualStyleBackColor = true;
            // 
            // RenameForm
            // 
            this.AcceptButton = this.buttonRename;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(224, 82);
            this.Controls.Add(this.chkNumbered);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.textBoxFix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonSuffix);
            this.Controls.Add(this.radioButtonPrefix);
            this.Name = "RenameForm";
            this.Text = "Rename";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonPrefix;
        private System.Windows.Forms.RadioButton radioButtonSuffix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFix;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox chkNumbered;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}