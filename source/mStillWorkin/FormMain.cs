using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mStillWorkin
{
    public partial class FormMain : Form
    {
        #region Properties

        public DateTime DateTimeStartWorking { get; set; }

        INPUT[] inputs = new INPUT[1];

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOftInputsArray);

        #endregion

        #region Events

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitializeFormProperties();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            SimulateWorking();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            HideForm();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTimer();
        }

        #endregion

        #region Methods

        void InitializeFormProperties()
        {
            notifyIcon.Visible = true;

            INPUT input = new INPUT
            {
                Type = 1
            };
            input.Data.Keyboard = new KEYBDINPUT();
            input.Data.Keyboard.Vk = (ushort)KeyCode.CAPS_LOCK;
            input.Data.Keyboard.Scan = 0;
            input.Data.Keyboard.Flags = 2;
            input.Data.Keyboard.Time = 0;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            inputs[0] = input;
        }

        void StartTimer()
        {
            timer.Start();
            DateTimeStartWorking = DateTime.Now;
            ToggleButtons();
        }

        void StopTimer()
        {
            timer.Stop();
            ToggleButtons();
        }

        void ToggleButtons()
        {
            btnStart.Enabled = btnStop.Enabled;
            btnStop.Enabled = !btnStart.Enabled;
        }

        void SimulateWorking()
        {
            TimeSpan timeSpan = (DateTime.Now - DateTimeStartWorking);

            string time = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            statusStripLblStatus.Text = string.Format("    {0}", time);

            try
            {
                if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}; {1}", ex.Message, ex.ToString()), string.Format("{0} Error", this.Text));
            }
        }

        void HideForm()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        void ShowForm()
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        #endregion


    }
}
