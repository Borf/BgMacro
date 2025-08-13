using BgMacro;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace BgMacro
{
    public partial class Form1 : Form
    {
        Settings Settings;

        Stopwatch lastClick = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbWindows.Items.Clear();
            WinInput.EnumWindows((hWnd, lParam) =>
            {
                if (!WinInput.IsWindowVisible(hWnd))
                    return true;

                var className = new StringBuilder(256);
                var windowTitle = new StringBuilder(256);
                WinInput.GetClassName(hWnd, className, className.Capacity);
                WinInput.GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
                if (windowTitle.Length == 0)
                    return true;

                cmbWindows.Items.Add(new Window()
                {
                    hWnd = hWnd,
                    className = className.ToString(),
                    windowTitle = windowTitle.ToString()
                });
                return true; // Continue enumeration
            }, IntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbButton.Items.AddRange(Enum.GetNames<Keys>());
            btnRefresh_Click(null, null);

            try
            {
                Settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"), new JsonSerializerOptions() { TypeInfoResolver = SourceGenerationContext.Default }) ?? new Settings();
                chkAutoStart.Checked = Settings.autoStart;
                cmbWindows.SelectedItem = cmbWindows.Items.Cast<Window>().FirstOrDefault(w => w.windowTitle == Settings.windowTitle && w.className == Settings.className);
                cmbWindows.Text = cmbWindows.SelectedItem.ToString();
                numericUpDown1.Value = Settings.interval;
                cmbButton.SelectedItem = Settings.key;
                if (Settings.autoStart)
                    btnStartStop_Click(null, null);
            }
            catch (Exception) { Settings = new(); }

        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if (timer1.Enabled)
            {
                timer1.Stop();
                btnStartStop.Text = "Start";
                lblStatus.Text = "Stopped";
                cmbWindows.Enabled = true;
                cmbButton.Enabled = true;
                chkAutoStart.Enabled = true;
                numericUpDown1.Enabled = true;
                timer3.Stop();

            }
            else
            {
                cmbWindows.Enabled = false;
                cmbButton.Enabled = false;
                chkAutoStart.Enabled = false;
                numericUpDown1.Enabled = false;

                timer1_Tick(null, null);
                timer1.Interval = (int)numericUpDown1.Value * 1000;
                timer1.Start();
                btnStartStop.Text = "Stop";
                lblStatus.Text = "Running";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            lblStatus.Text = "Sending key";
            var hwndMain = WinInput.FindWindow("UnityWndClass", "Ragnarok M");
            if (hwndMain == IntPtr.Zero)
            {
                lblStatus.Text = "Game window not found!";
                return;
            }
            int key = 0;
            try
            {
                key = (int)Enum.Parse<Keys>(cmbButton.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Invalid key selected!";
                return;
            }

            WinInput.PostMessage(hwndMain, WinInput.WM_ACTIVATE, 2, 0);
            WinInput.PostMessage(hwndMain, WinInput.WM_SETFOCUS, 0, 0);
            WinInput.PostMessage(hwndMain, WinInput.WM_KEYDOWN, new IntPtr(key), new IntPtr(0xF0001));
            Thread.Sleep(10);
            WinInput.PostMessage(hwndMain, WinInput.WM_CHAR, new IntPtr(key), new IntPtr(0xF0001));
            Thread.Sleep(50);
            WinInput.PostMessage(hwndMain, WinInput.WM_KEYUP, new IntPtr(key), new nint(0xC00F0001));
            lblStatus.Text = "Sent Key";
            lastClick.Restart();
            timer3.Start();

        }


        public void SaveSettings()
        {
            Settings.autoStart = chkAutoStart.Checked;
            Settings.className = ((Window)cmbWindows.SelectedItem).className;
            Settings.windowTitle = ((Window)cmbWindows.SelectedItem).windowTitle;
            Settings.interval = (int)numericUpDown1.Value;
            Settings.key = cmbButton.SelectedItem.ToString();
            File.WriteAllText("settings.json", JsonSerializer.Serialize(Settings, new JsonSerializerOptions() { WriteIndented = true, TypeInfoResolver = SourceGenerationContext.Default }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        int lastTime = -1;
        private void timer3_Tick(object sender, EventArgs e)
        {
            int time = (int)Math.Round(Settings.interval - lastClick.Elapsed.TotalSeconds);
            if (lastClick.Elapsed.TotalSeconds > 4 && time != lastTime)
                lblStatus.Text = time + "s until next key";

            lastTime = time;
            progressBar1.Value = (int)Math.Round(lastClick.Elapsed.TotalSeconds / Settings.interval * 10000);
        }
    }
}

[System.Text.Json.Serialization.JsonSerializable(typeof(Settings))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

public class Window
{
    public IntPtr hWnd { get; set; }
    public string className { get; set; }
    public string windowTitle { get; set; }

    public override string? ToString() => $"{hWnd}: {className} => {windowTitle}";
}