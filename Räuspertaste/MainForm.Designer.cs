
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cmbCaptureDevices = new System.Windows.Forms.ComboBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.radioPushToTalk = new System.Windows.Forms.RadioButton();
            this.radioRaeusper = new System.Windows.Forms.RadioButton();
            this.radioSwitch = new System.Windows.Forms.RadioButton();
            this.chkAutoDefaultDevice = new System.Windows.Forms.CheckBox();
            this.chkAutoNewDevice = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkMute = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioSink1 = new System.Windows.Forms.RadioButton();
            this.radioSink2 = new System.Windows.Forms.RadioButton();
            this.cmbSinkDevices2 = new System.Windows.Forms.ComboBox();
            this.cmbSinkDevices1 = new System.Windows.Forms.ComboBox();
            this.lblOut = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.volumeBar1 = new System.Windows.Forms.ProgressBar();
            this.peakBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbCaptureDevices
            // 
            this.cmbCaptureDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCaptureDevices.FormattingEnabled = true;
            this.cmbCaptureDevices.Location = new System.Drawing.Point(12, 121);
            this.cmbCaptureDevices.Name = "cmbCaptureDevices";
            this.cmbCaptureDevices.Size = new System.Drawing.Size(308, 21);
            this.cmbCaptureDevices.TabIndex = 0;
            this.cmbCaptureDevices.SelectedValueChanged += new System.EventHandler(this.cmbDevices_SelectedValueChanged);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 220);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1220, 404);
            this.txtLog.TabIndex = 1;
            // 
            // btnTrigger
            // 
            this.btnTrigger.Location = new System.Drawing.Point(437, 121);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(75, 23);
            this.btnTrigger.TabIndex = 2;
            this.btnTrigger.Text = "trigger";
            this.btnTrigger.UseVisualStyleBackColor = true;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            this.btnTrigger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            this.btnTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button1_MouseUp);
            // 
            // radioPushToTalk
            // 
            this.radioPushToTalk.AutoSize = true;
            this.radioPushToTalk.Location = new System.Drawing.Point(106, 148);
            this.radioPushToTalk.Name = "radioPushToTalk";
            this.radioPushToTalk.Size = new System.Drawing.Size(80, 17);
            this.radioPushToTalk.TabIndex = 3;
            this.radioPushToTalk.Text = "push to talk";
            this.radioPushToTalk.UseVisualStyleBackColor = true;
            this.radioPushToTalk.CheckedChanged += new System.EventHandler(this.radioPushToTalk_CheckedChanged);
            // 
            // radioRaeusper
            // 
            this.radioRaeusper.AutoSize = true;
            this.radioRaeusper.Checked = true;
            this.radioRaeusper.Location = new System.Drawing.Point(12, 148);
            this.radioRaeusper.Name = "radioRaeusper";
            this.radioRaeusper.Size = new System.Drawing.Size(88, 17);
            this.radioRaeusper.TabIndex = 4;
            this.radioRaeusper.TabStop = true;
            this.radioRaeusper.Text = "Räuspertaste";
            this.radioRaeusper.UseVisualStyleBackColor = true;
            this.radioRaeusper.CheckedChanged += new System.EventHandler(this.radioRaeusper_CheckedChanged);
            // 
            // radioSwitch
            // 
            this.radioSwitch.AutoSize = true;
            this.radioSwitch.Location = new System.Drawing.Point(184, 148);
            this.radioSwitch.Name = "radioSwitch";
            this.radioSwitch.Size = new System.Drawing.Size(79, 17);
            this.radioSwitch.TabIndex = 5;
            this.radioSwitch.Text = "umschalten";
            this.radioSwitch.UseVisualStyleBackColor = true;
            this.radioSwitch.CheckedChanged += new System.EventHandler(this.radioSwitch_CheckedChanged);
            // 
            // chkAutoDefaultDevice
            // 
            this.chkAutoDefaultDevice.AutoSize = true;
            this.chkAutoDefaultDevice.Checked = true;
            this.chkAutoDefaultDevice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDefaultDevice.Location = new System.Drawing.Point(268, 149);
            this.chkAutoDefaultDevice.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoDefaultDevice.Name = "chkAutoDefaultDevice";
            this.chkAutoDefaultDevice.Size = new System.Drawing.Size(137, 17);
            this.chkAutoDefaultDevice.TabIndex = 6;
            this.chkAutoDefaultDevice.Text = "auto use default device";
            this.chkAutoDefaultDevice.UseVisualStyleBackColor = true;
            // 
            // chkAutoNewDevice
            // 
            this.chkAutoNewDevice.AutoSize = true;
            this.chkAutoNewDevice.Location = new System.Drawing.Point(268, 168);
            this.chkAutoNewDevice.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoNewDevice.Name = "chkAutoNewDevice";
            this.chkAutoNewDevice.Size = new System.Drawing.Size(125, 17);
            this.chkAutoNewDevice.TabIndex = 7;
            this.chkAutoNewDevice.Text = "auto use new device";
            this.chkAutoNewDevice.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(437, 40);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkMute
            // 
            this.chkMute.AutoSize = true;
            this.chkMute.Enabled = false;
            this.chkMute.Location = new System.Drawing.Point(409, 148);
            this.chkMute.Margin = new System.Windows.Forms.Padding(2);
            this.chkMute.Name = "chkMute";
            this.chkMute.Size = new System.Drawing.Size(49, 17);
            this.chkMute.TabIndex = 9;
            this.chkMute.Text = "mute";
            this.chkMute.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Räuspertaste";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1244, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.dateiToolStripMenuItem.Text = "File";
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.beendenToolStripMenuItem.Text = "Exit";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 178);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 24);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(94, 178);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 24);
            this.button2.TabIndex = 12;
            this.button2.Text = "teams";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.volumeBar1);
            this.panel1.Controls.Add(this.cmbSinkDevices2);
            this.panel1.Controls.Add(this.cmbSinkDevices1);
            this.panel1.Controls.Add(this.radioSink2);
            this.panel1.Controls.Add(this.radioSink1);
            this.panel1.Location = new System.Drawing.Point(9, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 52);
            this.panel1.TabIndex = 15;
            // 
            // radioSink1
            // 
            this.radioSink1.AutoSize = true;
            this.radioSink1.Checked = true;
            this.radioSink1.Location = new System.Drawing.Point(317, 6);
            this.radioSink1.Name = "radioSink1";
            this.radioSink1.Size = new System.Drawing.Size(14, 13);
            this.radioSink1.TabIndex = 0;
            this.radioSink1.UseVisualStyleBackColor = true;
            this.radioSink1.CheckedChanged += new System.EventHandler(this.radioSink1_CheckedChanged);
            // 
            // radioSink2
            // 
            this.radioSink2.AutoSize = true;
            this.radioSink2.Location = new System.Drawing.Point(317, 30);
            this.radioSink2.Name = "radioSink2";
            this.radioSink2.Size = new System.Drawing.Size(14, 13);
            this.radioSink2.TabIndex = 1;
            this.radioSink2.UseVisualStyleBackColor = true;
            this.radioSink2.CheckedChanged += new System.EventHandler(this.radioSink2_CheckedChanged);
            // 
            // cmbSinkDevices2
            // 
            this.cmbSinkDevices2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSinkDevices2.FormattingEnabled = true;
            this.cmbSinkDevices2.Location = new System.Drawing.Point(0, 27);
            this.cmbSinkDevices2.Name = "cmbSinkDevices2";
            this.cmbSinkDevices2.Size = new System.Drawing.Size(308, 21);
            this.cmbSinkDevices2.TabIndex = 16;
            this.cmbSinkDevices2.SelectedValueChanged += new System.EventHandler(this.cmbSinkDevices2_SelectedValueChanged);
            // 
            // cmbSinkDevices1
            // 
            this.cmbSinkDevices1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSinkDevices1.FormattingEnabled = true;
            this.cmbSinkDevices1.Location = new System.Drawing.Point(0, 0);
            this.cmbSinkDevices1.Name = "cmbSinkDevices1";
            this.cmbSinkDevices1.Size = new System.Drawing.Size(308, 21);
            this.cmbSinkDevices1.TabIndex = 15;
            this.cmbSinkDevices1.SelectedValueChanged += new System.EventHandler(this.cmbSinkDevice1_SelectedValueChanged);
            // 
            // lblOut
            // 
            this.lblOut.AutoSize = true;
            this.lblOut.Location = new System.Drawing.Point(9, 24);
            this.lblOut.Name = "lblOut";
            this.lblOut.Size = new System.Drawing.Size(42, 13);
            this.lblOut.TabIndex = 16;
            this.lblOut.Text = "Output:";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(12, 105);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(34, 13);
            this.lblInput.TabIndex = 17;
            this.lblInput.Text = "Input:";
            // 
            // volumeBar1
            // 
            this.volumeBar1.Location = new System.Drawing.Point(337, 15);
            this.volumeBar1.MarqueeAnimationSpeed = 0;
            this.volumeBar1.Name = "volumeBar1";
            this.volumeBar1.Size = new System.Drawing.Size(82, 18);
            this.volumeBar1.TabIndex = 18;
            // 
            // peakBar1
            // 
            this.peakBar1.Location = new System.Drawing.Point(346, 121);
            this.peakBar1.Maximum = 255;
            this.peakBar1.Name = "peakBar1";
            this.peakBar1.Size = new System.Drawing.Size(82, 20);
            this.peakBar1.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 636);
            this.Controls.Add(this.peakBar1);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.lblOut);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkMute);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chkAutoNewDevice);
            this.Controls.Add(this.chkAutoDefaultDevice);
            this.Controls.Add(this.radioSwitch);
            this.Controls.Add(this.radioRaeusper);
            this.Controls.Add(this.radioPushToTalk);
            this.Controls.Add(this.btnTrigger);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cmbCaptureDevices);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Räuspertaste";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCaptureDevices;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.RadioButton radioPushToTalk;
        private System.Windows.Forms.RadioButton radioRaeusper;
        private System.Windows.Forms.RadioButton radioSwitch;
        private System.Windows.Forms.CheckBox chkAutoDefaultDevice;
        private System.Windows.Forms.CheckBox chkAutoNewDevice;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkMute;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioSink2;
        private System.Windows.Forms.RadioButton radioSink1;
        private System.Windows.Forms.ComboBox cmbSinkDevices2;
        private System.Windows.Forms.ComboBox cmbSinkDevices1;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.ProgressBar volumeBar1;
        private System.Windows.Forms.ProgressBar peakBar1;
    }
}

