﻿using CoreAudio;
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

        byte FN_LAYER = 0x01;
        bool AllowDisplay = false;

        EDataFlow dataFlow = EDataFlow.eCapture;
        DEVICE_STATE deviceState = DEVICE_STATE.DEVICE_STATE_ACTIVE;

        enum ButtonState
        {
            Pressed,
            Released
        }

        //RawHID rawHid = new RawHID(0x4B42, 0x6061); // KBD75
        RawHID rawHid = new RawHID(0xFEED, 0x6060); // Numpad
        RenderDevice currentDevice = null;
        //private GlobalKeyboardHook _globalKeyboardHook;
        MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
        MMDeviceEventManager audioDeviceManager = new MMDeviceEventManager();

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(AllowDisplay ? value : AllowDisplay);
        }

        public MainForm()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.microphone_unmuted;
            notifyIcon1.Icon = Properties.Resources.microphone_unmuted;

            rawHid.MessageReceived += RawHid_MessageReceived;
            rawHid.DeviceConnected += RawHid_DeviceConnected;

            audioDeviceManager.DeviceAdded += AudioDeviceManager_DeviceAdded;
            audioDeviceManager.DeviceRemoved += AudioDeviceManager_DeviceRemoved;
            audioDeviceManager.DeviceStateChanged += AudioDeviceManager_DeviceStateChanged;
            audioDeviceManager.DefaultDeviceChanged += AudioDeviceManager_DefaultDeviceChanged;

            

            //SetupKeyboardHooks();
            //  dev = (RenderDevice)cmbDevices.SelectedItem;


        }

        private void AudioDeviceManager_DeviceStateChanged(object sender, MMDeviceStateEventArgs e)
        {/*
            if (e.Device.DataFlow == this.dataFlow)
            {
                WriteLog("device (\"" + e.Device.FriendlyName + "\") state changed, rescan...");
                RefreshCaptureDevices(new RenderDevice(e.Device)); // bekommt noch keine ID...
                if (chkAutoDefaultDevice.Checked)
                    SetDefaultCaptureDevice();
                if (chkAutoNewDevice.Checked)
                    SetCaptureDevice(new RenderDevice(e.Device));
            }
            */
        }

        private void AudioDeviceManager_DeviceRemoved(object sender, MMDeviceRemovedEventArgs e)
        {
                WriteLog("device removed, rescan...");
                RefreshDevices();
                if (chkAutoDefaultDevice.Checked)
                    SetDefaultCaptureDevice();
        }

        private void AudioDeviceManager_DeviceAdded(object sender, MMDeviceEventArgs e)
        {
            if (e.Device.DataFlow == this.dataFlow)
            {
                WriteLog("device (\"" + e.Device.FriendlyName + "\") added, rescan...");

                RefreshDevices(new RenderDevice(e.Device));
                if (chkAutoDefaultDevice.Checked)
                    SetDefaultCaptureDevice();
                if (chkAutoNewDevice.Checked)
                    SetCaptureDevice(new RenderDevice(e.Device));
            }
        }

        private void AudioDeviceManager_DefaultDeviceChanged(object sender, MMDeviceEventArgs e)
        {
            if (e.Device.DataFlow == this.dataFlow && chkAutoDefaultDevice.Checked)
                SetCaptureDevice(new RenderDevice(e.Device));
        }

        private void RawHid_DeviceConnected(HidSharp.HidDevice Device)
        {
            Console.WriteLine(Device.DevicePath);
            if (currentDevice != null)
            {
                //System.Threading.Thread.Sleep(100); // warten bis das device kommandos annimmt.
                SetMuteColor(currentDevice.Device.AudioEndpointVolume.Mute);
            }
            rawHid.SendRawPayload(RawHID.Commands.Layer_On, new byte[] { FN_LAYER }); // Set FN-Layer
        }

        private void RawHid_MessageReceived(byte[] Report)
        {
            WriteLog("0x" + BitConverter.ToString(Report).Replace("-", "").ToLower());
            if (currentDevice == null)
                return;

            // Audio Command
            if (Report[0] == 0x00 && Report[1] == 0xde && Report[2] == 0xad && Report[3] == 0xba && Report[4] == 0xbe)
            {
                if (Report[5] == 0x00)
                    ButtonEvent(ButtonState.Pressed);
                else if (Report[5] == 0x01)
                    ButtonEvent(ButtonState.Released);
                else if (Report[5] == 0x03)
                    currentDevice.Device.AudioEndpointVolume.Mute = !currentDevice.Device.AudioEndpointVolume.Mute;
            }
            // Layer Change
            else if (Report[0] == 0x00 && Report[1] == 0xb0)
            {
                SetMuteColor(currentDevice.Device.AudioEndpointVolume.Mute);
            }

        }

        void ButtonEvent(ButtonState state)
        {
            if (currentDevice == null)
                return;
            bool pressed = state == ButtonState.Pressed;
            if (radioRaeusper.Checked)
            {
                currentDevice.Device.AudioEndpointVolume.Mute = pressed;
            }
            else if (radioPushToTalk.Checked)
            {
                currentDevice.Device.AudioEndpointVolume.Mute = !pressed;
            }
            else if (radioSwitch.Checked)
            {
                if (pressed)
                    currentDevice.Device.AudioEndpointVolume.Mute = !currentDevice.Device.AudioEndpointVolume.Mute;
            }
            else
            {
                throw new Exception("Hell No! GUI Fuckup!");
            }
        }



        void SetMuteColor(bool Muted)
        {
            byte[] color = new byte[] { 0xff, 0xff, 0xff };

            if (Muted)
                color = new byte[] { 0xff, 0x00, 0x00 };
            else
                color = new byte[] { 0x00, 0xff, 0x00 };

            rawHid.SendRawPayload(RawHID.Commands.RGB_SetColorRGB, color);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshDevices();

            if (chkAutoDefaultDevice.Checked)
                SetDefaultCaptureDevice();

            rawHid.Start();
        }

        private void WriteLog(string Message)
        {
            WriteLog(Message, false);
        }
        private void WriteLog(string Message, bool NoNewLine)
        {
            txtLog.Invoke((MethodInvoker)delegate ()
            {
                txtLog.AppendText(Message + (NoNewLine ? "" : Environment.NewLine ));
            });
        }

        void SetDefaultCaptureDevice()
        {
            SetCaptureDevice(new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(dataFlow, ERole.eCommunications)));
        }

        void SetCaptureDevice(RenderDevice newDevice)
        {
            try
            {
                //RenderDevice defaultDevice = new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications));
                WriteLog("setting device to \"" + newDevice.Name + "\"...", true);
                foreach (RenderDevice dev in cmbDevices.Items)
                {
                    if (newDevice.Name == dev.Name)
                    {
                        WriteLog(" found");
                        cmbDevices.Invoke((MethodInvoker)delegate ()
                        {
                            cmbDevices.SelectedIndex = cmbDevices.Items.IndexOf(dev);
                        });
                    }
                }
            }
            catch
            {
                ;
            }
        }
        void RefreshDevices(RenderDevice newDevice)
        {
            if (cmbDevices.Items.Count == 0)
            {
                RefreshDevices();
                return;
            }
            foreach (RenderDevice dev in cmbDevices.Items)
            {
                if (newDevice.ID == dev.ID)
                {
                    WriteLog(" found");
                    cmbDevices.Invoke((MethodInvoker)delegate ()
                    {
                        cmbDevices.Items.Remove(dev);
                    });
                }
            }
            cmbDevices.Invoke((MethodInvoker)delegate ()
            {
                cmbDevices.Items.Add(newDevice);
            });
        }
        void RefreshDevices()
        {
            cmbDevices.Invoke((MethodInvoker)delegate ()
            {
                cmbDevices.Items.Clear();
            });
            
            var devCol = deviceEnumerator.EnumerateAudioEndPoints(dataFlow, this.deviceState);
            for (int i = 0; i < devCol.Count; i++)
            {
                MMDevice dev = devCol[i];
                //dev.AudioEndpointVolume.MasterVolumeLevel = trackBar2.Value;
                //ComboBoxDevices.Items.Add(new RenderDevice(devCol[i]));
                WriteLog(dev.FriendlyName);
                cmbDevices.Invoke((MethodInvoker)delegate ()
                {
                    cmbDevices.Items.Add(new RenderDevice(dev));
                });
            }
            //SetDefaultCaptureDevice();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {

            ButtonEvent(ButtonState.Pressed);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonEvent(ButtonState.Released);
        }

        public void SetupKeyboardHooks()
        {
            //_globalKeyboardHook = new GlobalKeyboardHook();
            //_globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
           //WriteLog("VirtualKeyCode: " + e.KeyboardData.VirtualCode.ToString() + " " + e.KeyboardState.ToString());

            
            
            if (e.KeyboardData.VirtualCode == Keys.F22)// && e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            {
                WriteLog("e.KeyboardState = " + e.KeyboardState.ToString());

                if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                {
                    //ButtonEvent(ButtonState.Pressed);
                }
                else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
                {
                    //ButtonEvent(ButtonState.Released);
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

        }

        private void cmbDevices_SelectedValueChanged(object sender, EventArgs e)
        {
            this.currentDevice = (RenderDevice)cmbDevices.SelectedItem;
            
            this.currentDevice.Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            this.currentDevice.Device.AudioSessionManager2.OnSessionCreated += AudioSessionManager2_OnSessionCreated;
            this.currentDevice.Device.AudioSessionManager2.OnSessionDestroyed += AudioSessionManager2_OnSessionDestroyed;
            SetMuteColor(this.currentDevice.Device.AudioEndpointVolume.Mute);
        }

        private void AudioSessionManager2_OnSessionDestroyed(object sender, CoreAudio.Interfaces.IAudioSessionControl2 newSession)
        {
            WriteLog("session destroyed:");
        }

        private void AudioSessionManager2_OnSessionCreated(object sender, CoreAudio.Interfaces.IAudioSessionControl2 newSession)
        {
            string sessionName = "";
            newSession.GetDisplayName(out sessionName);
            WriteLog("session created: \"" + sessionName + "\"");
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            chkMute.Invoke((MethodInvoker)delegate()
            {
                chkMute.Checked = data.Muted;
                notifyIcon1.Icon = data.Muted ? Properties.Resources.microphone_muted : Properties.Resources.microphone_unmuted;
                this.Icon = notifyIcon1.Icon;
            });
            
            SetMuteColor(data.Muted);
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {

        }

        private void radioRaeusper_CheckedChanged(object sender, EventArgs e)
        {
            currentDevice.Device.AudioEndpointVolume.Mute = false;
        }

        private void radioPushToTalk_CheckedChanged(object sender, EventArgs e)
        {
            currentDevice.Device.AudioEndpointVolume.Mute = true;
        }

        private void radioSwitch_CheckedChanged(object sender, EventArgs e)
        {
            currentDevice.Device.AudioEndpointVolume.Mute = !currentDevice.Device.AudioEndpointVolume.Mute;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.AllowDisplay = true;
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void Exit()
        {
            //_globalKeyboardHook?.Dispose();
            rawHid.SendRawPayload(RawHID.Commands.Layer_Off, new byte[] { FN_LAYER }); // Disable FN-Layer
            rawHid.Stop();
            Application.Exit();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}
