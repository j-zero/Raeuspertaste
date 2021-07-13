using CoreAudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Räuspertaste
{
    public partial class MainForm : Form
    {
        RawHID rawHid = new RawHID(0x4B42, 0x6061);
        public MainForm()
        {
            InitializeComponent();
            rawHid.MessageReceived += RawHid_MessageReceived;
            
        }


        private void RawHid_MessageReceived(byte[] Report)
        {
            WriteLog("0x" + BitConverter.ToString(Report).Replace("-", "").ToLower());

            if(Report[0] == 0x00 && Report[1] == 0xde && Report[2] == 0xad && Report[3] == 0xba && Report[4] == 0xbe)
            {
                RenderDevice dev = (RenderDevice)cmbDevices.SelectedItem;
                if (Report[5] == 0x00)
                    dev.Device.AudioEndpointVolume.Mute = radioRaeusper.Checked;
                else if(Report[5] == 0x01)
                    dev.Device.AudioEndpointVolume.Mute = !radioRaeusper.Checked;
            }

            //throw new NotImplementedException();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshCaptureDevices();
            rawHid.Start();
            //SetupKeyboardHooks();
        }

        private void WriteLog(string Message)
        {
            txtLog.AppendText(Message + Environment.NewLine);
        }

        void RefreshCaptureDevices()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            var devCol = deviceEnumerator.EnumerateAudioEndPoints(EDataFlow.eAll, DEVICE_STATE.DEVICE_STATE_ACTIVE);
            for (int i = 0; i < devCol.Count; i++)
            {
                MMDevice dev = devCol[i];
                //dev.AudioEndpointVolume.MasterVolumeLevel = trackBar2.Value;
                //ComboBoxDevices.Items.Add(new RenderDevice(devCol[i]));
                WriteLog(dev.FriendlyName);
                cmbDevices.Items.Add(new RenderDevice(dev));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            RenderDevice dev = (RenderDevice)cmbDevices.SelectedItem;
            dev.Device.AudioEndpointVolume.Mute = radioRaeusper.Checked;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            RenderDevice dev = (RenderDevice)cmbDevices.SelectedItem;
            dev.Device.AudioEndpointVolume.Mute = !radioRaeusper.Checked;
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            WriteLog("VirtualKeyCode: " + e.KeyboardData.VirtualCode.ToString() + " " + e.KeyboardState.ToString());

            
            
            if (e.KeyboardData.VirtualCode == Keys.M)// && e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            {
                WriteLog("e.KeyboardState = " + e.KeyboardState.ToString());

                if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                {
                    WriteLog("Mute");
                }
                else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
                {
                    WriteLog("Unute");
                }


                e.Handled = true;
                return;
            }
                
            /*

            */
            // seems, not needed in the life.
            //if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
            //    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            //{
            //    MessageBox.Show("Alt + Print Screen");
            //    e.Handled = true;
            //}
            //else
            //

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _globalKeyboardHook?.Dispose();
        }
    }
}
