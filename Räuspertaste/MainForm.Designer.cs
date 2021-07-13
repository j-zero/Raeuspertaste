
namespace Räuspertaste
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radioPushToTalk = new System.Windows.Forms.RadioButton();
            this.radioRaeusper = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(12, 12);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(308, 21);
            this.cmbDevices.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 74);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(430, 140);
            this.txtLog.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Mute";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            this.button1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button1_MouseUp);
            // 
            // radioPushToTalk
            // 
            this.radioPushToTalk.AutoSize = true;
            this.radioPushToTalk.Location = new System.Drawing.Point(106, 51);
            this.radioPushToTalk.Name = "radioPushToTalk";
            this.radioPushToTalk.Size = new System.Drawing.Size(80, 17);
            this.radioPushToTalk.TabIndex = 3;
            this.radioPushToTalk.Text = "push to talk";
            this.radioPushToTalk.UseVisualStyleBackColor = true;
            // 
            // radioRaeusper
            // 
            this.radioRaeusper.AutoSize = true;
            this.radioRaeusper.Checked = true;
            this.radioRaeusper.Location = new System.Drawing.Point(12, 51);
            this.radioRaeusper.Name = "radioRaeusper";
            this.radioRaeusper.Size = new System.Drawing.Size(88, 17);
            this.radioRaeusper.TabIndex = 4;
            this.radioRaeusper.TabStop = true;
            this.radioRaeusper.Text = "Räuspertaste";
            this.radioRaeusper.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 226);
            this.Controls.Add(this.radioRaeusper);
            this.Controls.Add(this.radioPushToTalk);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cmbDevices);
            this.Name = "MainForm";
            this.Text = "Räuspertaste";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioPushToTalk;
        private System.Windows.Forms.RadioButton radioRaeusper;
    }
}

