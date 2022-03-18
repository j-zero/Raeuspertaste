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

        byte FN_LAYER = 0x01;
        bool AllowDisplay = true;

        bool TeamsMuted = false;

        EDataFlow dataFlowCapture = EDataFlow.eCapture;
        DEVICE_STATE deviceStateCapture = DEVICE_STATE.DEVICE_STATE_ACTIVE;

        EDataFlow dataFlowSink = EDataFlow.eRender;
        DEVICE_STATE deviceStateSink = DEVICE_STATE.DEVICE_STATE_ACTIVE;

        enum ButtonState
        {
            Pressed,
            Released
        }

        //RawHID rawHid = new RawHID(0x4B42, 0x6061); // KBD75
        //RawHID rawHid = new RawHID(0xFEED, 0x6060); // Numpad
        RawHID rawHid = new RawHID(0xFEED, 0x6062); // Macropad
        RenderDevice currentCaptureDevice = null;
        RenderDevice currentSinkDevice = null;
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
            RefreshCaptureDevices();
            if (chkAutoDefaultDevice.Checked)
            {
                SetDefaultCaptureDevice();
                SetDefaultSinkDevice();
            }
            RefreshLEDs();
        }

        private void AudioDeviceManager_DeviceAdded(object sender, MMDeviceEventArgs e)
        {
            if (e.Device.DataFlow == this.dataFlowCapture)
            {
                WriteLog("capture device (\"" + e.Device.FriendlyName + "\") added, rescan...");

                RefreshCaptureDevices(new RenderDevice(e.Device));
                if (chkAutoDefaultDevice.Checked)
                    SetDefaultCaptureDevice();
                if (chkAutoNewDevice.Checked)
                    SetCaptureDevice(new RenderDevice(e.Device));
            }
            if (e.Device.DataFlow == this.dataFlowSink)
            {
                WriteLog("render device (\"" + e.Device.FriendlyName + "\") added, rescan...");

                RefreshSinkDevices(new RenderDevice(e.Device));

            }
            RefreshLEDs();
        }

        private void AudioDeviceManager_DefaultDeviceChanged(object sender, MMDeviceEventArgs e)
        {
            if (e.Device.DataFlow == this.dataFlowCapture && chkAutoDefaultDevice.Checked)
                SetCaptureDevice(new RenderDevice(e.Device));
            RefreshLEDs();
        }

        private void RawHid_DeviceConnected(HidSharp.HidDevice Device)
        {


            Console.WriteLine(Device.DevicePath);
            if (currentCaptureDevice != null)
            {
                for(byte i = 0; i< 9; i++)
                    rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { i, 0x00, 0x00, 0x00 });

                //System.Threading.Thread.Sleep(100); // warten bis das device kommandos annimmt.
                RefreshLEDs();
            }
            //rawHid.SendRawPayload(RawHID.Commands.Layer_On, new byte[] { FN_LAYER }); // Set FN-Layer
            
        }

        private void RawHid_MessageReceived(byte[] Report)
        {
            //WriteLog("DEBUG: < 0x" + BitConverter.ToString(Report).Replace("-", "").ToLower());
            if (currentCaptureDevice == null)
                return;

            // Audio Command
            if (Report[0] == 0x00 && Report[1] == 0xde && Report[2] == 0xad && Report[3] == 0xba && Report[4] == 0xbe)
            {
                if (Report[5] == 0x00)
                    ButtonEvent(ButtonState.Pressed);
                else if (Report[5] == 0x01)
                    ButtonEvent(ButtonState.Released);
                else if (Report[5] == 0x03)
                {
                    //currentDevice.Device.AudioEndpointVolume.Mute = !currentDevice.Device.AudioEndpointVolume.Mute;
                    if (radioRaeusper.Checked)
                    {
                        radioPushToTalk.Checked = true;
                        radioRaeusper.Checked = false;
                    }
                    else if (radioPushToTalk.Checked)
                    {
                        radioPushToTalk.Checked = false;
                        radioRaeusper.Checked = true;
                    }

                }
                else if (Report[5] == 0x05)
                {
                    MuteTeams();

                }
                else if (Report[5] == 0x0a) // VOL_UP
                {
                    currentSinkDevice.Device.AudioEndpointVolume.VolumeStepUp();
                    //deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eMultimedia).AudioEndpointVolume.VolumeStepUp(); // Default MultimediaDevice
                    int vol = (int)(currentSinkDevice.Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                    //WriteLog($"{currentSinkDevice.Device.FriendlyName} volume: {vol.ToString()}%");
                    volumeBar1.Value = vol;
                    RefreshLEDs();
                }
                else if (Report[5] == 0x0b) // VOL_DOWN
                {
                    currentSinkDevice.Device.AudioEndpointVolume.VolumeStepDown();

                    int vol = (int)(currentSinkDevice.Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);

                    volumeBar1.Value = vol;
                    //WriteLog($"{currentSinkDevice.Device.FriendlyName} volume: {vol.ToString()}%");
                    //deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eMultimedia).AudioEndpointVolume.VolumeStepDown(); // Default MultimediaDevice
                    RefreshLEDs();
                }
                else if (Report[5] == 0x0c) // Enc Press
                {
                    currentSinkDevice.Device.AudioEndpointVolume.Mute = !currentSinkDevice.Device.AudioEndpointVolume.Mute;
                    RefreshLEDs();
                }
                else if (Report[5] == 0x0d) // Enc release
                {

                }
                else if (Report[5] == 0x0e) // Enc Press
                {
                    if (radioSink1.Checked)
                    {
                        radioSink1.Checked = false;
                        radioSink2.Checked = true;
                    }
                    else if (radioSink2.Checked)
                    {
                        radioSink1.Checked = true;
                        radioSink2.Checked = false;
                    }

                }
                else if (Report[5] == 0x0f) // Enc release
                {

                }
            }
            // Layer Change
            else if (Report[0] == 0x00 && Report[1] == 0xb0)
            {
                RefreshLEDs();
            }

        }

        MMDevice GetCurrentActiveDefaultSinkDevice()
        {
            MMDevice mmDev = deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eMultimedia);
            MMDevice comDev = deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eCommunications);

            SessionCollection mmSessions = mmDev.AudioSessionManager2.Sessions;
            SessionCollection comSessions = comDev.AudioSessionManager2.Sessions;

            int mcount = 0;
            int ccount = 0;

            foreach (var m in mmSessions)
            {
                WriteLog($"m: ({m.IsSystemSoundsSession})\"{m.GetSessionIdentifier}\"");
                if (!m.IsSystemSoundsSession)
                    mcount++;
            }

            foreach (var c in comSessions)
            {
                WriteLog($"c: ({c.IsSystemSoundsSession})\"{c.GetSessionIdentifier}\"");
                if (!c.IsSystemSoundsSession)
                    ccount++;
            }

            if (ccount != 0)
                return comDev;
            else
                return mmDev;
            return null;
        }

        void ButtonEvent(ButtonState state)
        {
            if (currentCaptureDevice == null)
                return;
            bool pressed = state == ButtonState.Pressed;
            if (radioRaeusper.Checked)
            {
                currentCaptureDevice.Device.AudioEndpointVolume.Mute = pressed;
            }
            else if (radioPushToTalk.Checked)
            {
                currentCaptureDevice.Device.AudioEndpointVolume.Mute = !pressed;
            }
            else if (radioSwitch.Checked)
            {
                if (pressed)
                    currentCaptureDevice.Device.AudioEndpointVolume.Mute = !currentCaptureDevice.Device.AudioEndpointVolume.Mute;
            }
            else
            {
                throw new Exception("Hell No! GUI Fuckup!");
            }
        }


        void RefreshLEDs()
        {

            byte dimWhite = 0x30;

            //rawHid.SendRawPayload(RawHID.Commands.RGB_Single, Muted ? new byte[] { 0x00, 0xff, 0x00, 0x00 } : new byte[] { 0x00, 0x00, 0xff, 0x00 });
            //rawHid.SendRawPayload(RawHID.Commands.RGB_Single, !Muted ? new byte[] { 0x01, 0xff, 0x00, 0x00 } : new byte[] { 0x01, 0x00, 0xff, 0x00 });

            //SendColor(0, Color.Red); // First/Micpeak
            if (currentCaptureDevice != null)
            {
                if (currentCaptureDevice.Device.AudioEndpointVolume.Mute)
                    SendColor(1, 0xff, 0x00, 0x00);
                else
                    SendColor(1, 0x00, 0xff, 0x00);
            }
            // ignore 2
            SendColor(2, Color.Black); // Encoder
            SendColor(5, 0x80, 0x00, 0xff); // Teams


            if (radioSink1.Checked)
                SendColor(3, Color.Yellow);
            else if (radioSink2.Checked)
                SendColor(3, Color.Blue);

            if (currentSinkDevice != null)
            {
               
                //Console.WriteLine($"{String.Format("{0:0.00}", i)} := 0x{led_bar[0].ToString("X2")}{led_bar[1].ToString("X2")}{led_bar[2].ToString("X2")}");




                if (currentSinkDevice.Device.AudioEndpointVolume.Mute)
                    SendColor(4, Color.Red); // Encoder
                else
                    SendColor(4, Color.White); // play

                byte[] led_val = BrightnessFromFloat(currentSinkDevice.Device.AudioEndpointVolume.MasterVolumeLevelScalar, 3);

                SendColor(8, 0, 0, led_val[0]);
                SendColor(7, 0, 0, led_val[1]);
                SendColor(6, 0, 0, led_val[2]);

                /*
                WriteLog(currentSinkDevice.Device.AudioEndpointVolume.MasterVolumeLevelScalar.ToString());

                byte sink_vol = (byte)(map(currentSinkDevice.Device.AudioEndpointVolume.MasterVolumeLevelScalar * 255, 0, 255, 0, 0xffffff));
                
                byte led1_val = (byte)(sink_vol & 0x000000ff);
                byte led2_val = (byte)((sink_vol & 0x0000ff00) >> 8);
                byte led3_val = (byte)((sink_vol & 0x00ff0000) >> 16);

                if(led3_val > 0)
                    led2_val = 0xff;
                if (led2_val > 0)
                    led1_val = 0xff;
                    

                WriteLog(sink_vol.ToString("X2") + ": " + led1_val.ToString("X2") + " " + led2_val.ToString("X2") + " " + led3_val.ToString("X2"));


                */
            }





        }

        void SendColor(byte led, Color color)
        {
            SendColor(led, (byte)color.R, (byte)color.G, (byte)color.B);
        }

        void SendColor(byte led, byte r, byte g, byte b)
        {
            rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { led, r, g, b });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshDevices();

            if (chkAutoDefaultDevice.Checked)
            {
                SetDefaultCaptureDevice();
                SetDefaultSinkDevice();
            }

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
            SetCaptureDevice(new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(dataFlowCapture, ERole.eCommunications)));
        }

        void SetCaptureDevice(RenderDevice newDevice)
        {
            try
            {
                //RenderDevice defaultDevice = new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications));
                WriteLog("setting device to \"" + newDevice.Name + "\"...", true);
                foreach (RenderDevice dev in cmbCaptureDevices.Items)
                {
                    if (newDevice.Name == dev.Name)
                    {
                        WriteLog(" found");
                        cmbCaptureDevices.Invoke((MethodInvoker)delegate ()
                        {
                            cmbCaptureDevices.SelectedIndex = cmbCaptureDevices.Items.IndexOf(dev);
                        });
                    }
                }
            }
            catch
            {
                ;
            }
        }

        void SetDefaultSinkDevice()
        {
            //MMDevice dev = GetCurrentActiveDefaultSinkDevice();
           // WriteLog($"Set default sink to \"{dev.FriendlyName}\"");
            
            //SetSinkDevice(new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eCommunications)));

            MMDevice mmDev = deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eMultimedia);
            MMDevice comDev = deviceEnumerator.GetDefaultAudioEndpoint(dataFlowSink, ERole.eCommunications);

            SetSinkDevice(new RenderDevice(mmDev),cmbSinkDevices1);
            SetSinkDevice(new RenderDevice(comDev), cmbSinkDevices2);

            this.currentSinkDevice.Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotificationSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionCreated += AudioSessionManager2_OnSessionCreatedSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionDestroyed += AudioSessionManager2_OnSessionDestroyedSink;

        }

        void SetSinkDevice(RenderDevice newDevice, ComboBox cmbBox)
        {
            try
            {
                //RenderDevice defaultDevice = new RenderDevice(deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications));
                WriteLog("setting device to \"" + newDevice.Name + "\"...", true);
                foreach (RenderDevice dev in cmbBox.Items)
                {
                    if (newDevice.Name == dev.Name)
                    {
                        WriteLog(" found");
                        cmbBox.Invoke((MethodInvoker)delegate ()
                        {
                            cmbBox.SelectedIndex = cmbBox.Items.IndexOf(dev);
                        });
                    }
                }
            }
            catch
            {
                ;
            }
        }

        void RefreshDevices()
        {
            RefreshCaptureDevices();
            RefreshSinkDevices();
        }

        void RefreshCaptureDevices(RenderDevice newDevice)
        {
            if (cmbCaptureDevices.Items.Count == 0)
            {
                RefreshCaptureDevices();
                return;
            }
            foreach (RenderDevice dev in cmbCaptureDevices.Items)
            {
                if (newDevice.ID == dev.ID)
                {
                    WriteLog(" found");
                    cmbCaptureDevices.Invoke((MethodInvoker)delegate ()
                    {
                        cmbCaptureDevices.Items.Remove(dev);
                    });
                }
            }
            cmbCaptureDevices.Invoke((MethodInvoker)delegate ()
            {
                cmbCaptureDevices.Items.Add(newDevice);
            });
        }

        void RefreshCaptureDevices()
        {
            cmbCaptureDevices.Invoke((MethodInvoker)delegate ()
            {
                cmbCaptureDevices.Items.Clear();
            });
            
            var devCol = deviceEnumerator.EnumerateAudioEndPoints(dataFlowCapture, this.deviceStateCapture);
            for (int i = 0; i < devCol.Count; i++)
            {
                MMDevice dev = devCol[i];
                WriteLog($"Capture: {dev.FriendlyName}");
                cmbCaptureDevices.Invoke((MethodInvoker)delegate ()
                {
                    cmbCaptureDevices.Items.Add(new RenderDevice(dev));
                });
            }
        }

        void RefreshSinkDevices(RenderDevice newDevice)
        {
            if (cmbSinkDevices1.Items.Count == 0)
            {
                RefreshCaptureDevices();
                return;
            }
            foreach (RenderDevice dev in cmbSinkDevices1.Items)
            {
                if (newDevice.ID == dev.ID)
                {
                    WriteLog(" found");
                    cmbSinkDevices1.Invoke((MethodInvoker)delegate ()
                    {
                        cmbSinkDevices1.Items.Remove(dev);
                    });
                }
            }
            cmbSinkDevices1.Invoke((MethodInvoker)delegate ()
            {
                cmbSinkDevices1.Items.Add(newDevice);
            });
        }

        void RefreshSinkDevices()
        {
            cmbSinkDevices1.Invoke((MethodInvoker)delegate (){  cmbSinkDevices1.Items.Clear(); });
            cmbSinkDevices2.Invoke((MethodInvoker)delegate () { cmbSinkDevices2.Items.Clear(); });

            var devCol = deviceEnumerator.EnumerateAudioEndPoints(dataFlowSink, this.deviceStateSink);
            for (int i = 0; i < devCol.Count; i++)
            {
                MMDevice dev = devCol[i];
                WriteLog($"Sink: {dev.FriendlyName}");
                cmbSinkDevices1.Invoke((MethodInvoker)delegate (){ cmbSinkDevices1.Items.Add(new RenderDevice(dev)); });
                cmbSinkDevices2.Invoke((MethodInvoker)delegate () { cmbSinkDevices2.Items.Add(new RenderDevice(dev)); });
            }
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
            this.currentCaptureDevice = (RenderDevice)cmbCaptureDevices.SelectedItem;
            
            this.currentCaptureDevice.Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            this.currentCaptureDevice.Device.AudioSessionManager2.OnSessionCreated += AudioSessionManager2_OnSessionCreated;
            this.currentCaptureDevice.Device.AudioSessionManager2.OnSessionDestroyed += AudioSessionManager2_OnSessionDestroyed;
            RefreshLEDs();
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

            RefreshLEDs();
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            
        }

        private void radioRaeusper_CheckedChanged(object sender, EventArgs e)
        {
            currentCaptureDevice.Device.AudioEndpointVolume.Mute = false;
        }

        private void radioPushToTalk_CheckedChanged(object sender, EventArgs e)
        {
            currentCaptureDevice.Device.AudioEndpointVolume.Mute = true;
        }

        private void radioSwitch_CheckedChanged(object sender, EventArgs e)
        {
            currentCaptureDevice.Device.AudioEndpointVolume.Mute = !currentCaptureDevice.Device.AudioEndpointVolume.Mute;
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
            Environment.Exit(0);
        }

        private void MuteTeams()
        {
            string procName = "Teams";

            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            foreach (MMDevice device in DevEnum.EnumerateAudioEndPoints(EDataFlow.eRender, DEVICE_STATE.DEVICE_STATE_ACTIVE))
            {
                foreach (var session in device.AudioSessionManager2.Sessions)
                {
                    if (session.State == AudioSessionState.AudioSessionStateActive)
                    {
                        var proc = System.Diagnostics.Process.GetProcessById((int)session.GetProcessID);
                        if (proc.ProcessName == procName)
                        {
                            session.SimpleAudioVolume.Mute = !TeamsMuted;
                        }
                    }
                }
            }
            foreach (MMDevice device in DevEnum.EnumerateAudioEndPoints(EDataFlow.eCapture, DEVICE_STATE.DEVICE_STATE_ACTIVE))
            {
                
                foreach (var session in device.AudioSessionManager2.Sessions)
                {
                    if (session.State == AudioSessionState.AudioSessionStateActive)
                    {
                        var proc = System.Diagnostics.Process.GetProcessById((int)session.GetProcessID);
                        if (proc.ProcessName == procName)
                        {
                            WriteLog(session.GetSessionIdentifier);
                            if(!session.IsSystemSoundsSession)
                                session.SimpleAudioVolume.Mute = !TeamsMuted;
                        }
                    }
                }
            }
            TeamsMuted = !TeamsMuted;
        }


        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] color = new byte[] { 0x00, 0x00, 0x00, 0xff };
            rawHid.SendRawPayload(RawHID.Commands.RGB_Single, color);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MuteTeams();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (this.currentCaptureDevice != null && this.currentCaptureDevice.Device.AudioMeterInformation.MasterPeakValue != 0)
            if(this.currentCaptureDevice != null && !currentCaptureDevice.Device.AudioEndpointVolume.Mute && this.currentCaptureDevice.Device.AudioMeterInformation.MasterPeakValue != 0)
            {
                float peak = this.currentCaptureDevice.Device.AudioMeterInformation.MasterPeakValue;
                byte fval = (byte)map(peak, 0, 1, 0, 255);
                peakBar1.Value = fval;
                try
                {
                    rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { 0x00, (byte)LEDHelper.pwm_table[fval], 0x00, 0x00 });
                }
                catch
                {

                }
                //rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { 0x01, fval, 0x00, 0x00 });

            }
            else
            {
                //rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { 0x00, 0x00, 0x80, 0x00 });
                //rawHid.SendRawPayload(RawHID.Commands.RGB_Single, new byte[] { 0x01, 0x80, 0x00, 0x00 });
            }
        }

        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        static byte[] BrightnessFromFloat(float x, int led_count)
        {
            byte[] result = new byte[led_count];
            byte factor = 0xff; // led bits
            int val = (int)(x * ((factor + 1) * led_count));
            for (int i = led_count - 1; i >= 0; i--)
            {
                if (val >= factor)
                {
                    result[i] = factor;
                    val -= factor;
                }
                else
                {
                    result[i] = (byte)val;
                    break;
                }
            }
            return result;
        }

        private void cmbSinkDevice1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(radioSink1.Checked)
                this.currentSinkDevice = (RenderDevice)cmbSinkDevices1.SelectedItem;
            //radioSink1.Checked = true;
            //radioSink2.Checked = false;

            this.currentSinkDevice.Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotificationSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionCreated += AudioSessionManager2_OnSessionCreatedSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionDestroyed += AudioSessionManager2_OnSessionDestroyedSink;
            
        }

        private void AudioSessionManager2_OnSessionDestroyedSink(object sender, CoreAudio.Interfaces.IAudioSessionControl2 newSession)
        {
            //throw new NotImplementedException();
            SetDefaultSinkDevice();
        }

        private void AudioSessionManager2_OnSessionCreatedSink(object sender, CoreAudio.Interfaces.IAudioSessionControl2 newSession)
        {
            //throw new NotImplementedException();
            SetDefaultSinkDevice();
        }

        private void AudioEndpointVolume_OnVolumeNotificationSink(AudioVolumeNotificationData data)
        {
            //throw new NotImplementedException();
            //SetDefaultSinkDevice();
            //volumeBar1.Value = (int)(data.MasterVolume * 100);
            RefreshLEDs();
        }

        private void cmbSinkDevices2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (radioSink2.Checked)
                this.currentSinkDevice = (RenderDevice)cmbSinkDevices2.SelectedItem;
            //this.currentSinkDevice = (RenderDevice)cmbSinkDevices2.SelectedItem;
            //radioSink1.Checked = false;
            //radioSink2.Checked = true;

            this.currentSinkDevice.Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotificationSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionCreated += AudioSessionManager2_OnSessionCreatedSink;
            this.currentSinkDevice.Device.AudioSessionManager2.OnSessionDestroyed += AudioSessionManager2_OnSessionDestroyedSink;

            
        }

        private void radioSink1_CheckedChanged(object sender, EventArgs e)
        {
            this.currentSinkDevice = (RenderDevice)cmbSinkDevices1.SelectedItem;
            RefreshLEDs();
        }

        private void radioSink2_CheckedChanged(object sender, EventArgs e)
        {
            this.currentSinkDevice = (RenderDevice)cmbSinkDevices2.SelectedItem;
            RefreshLEDs();
        }
    }
}
