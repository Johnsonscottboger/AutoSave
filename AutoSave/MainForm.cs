using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSave
{
    public partial class MainForm : Form
    {
        private List<string> processes = new List<string>()
        {
            "notepad",
            "winword",
            "excel",
            "wps",
            "et",
            "wpp"
        };
        private readonly Timer timer = new Timer();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            启动ToolStripMenuItem_Click(sender, e);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            this.notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void 启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Tick -= AutoSave;
            timer.Tick += AutoSave;
            timer.Interval = 10000;
            timer.Start();
        }

        private void 停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void AutoSave(object sender, EventArgs e)
        {
            var list = LoadProcesses();
            var process = GetActiveProcess();
            if (list.Any(p => p.Equals(process.ProcessName, StringComparison.InvariantCultureIgnoreCase)))
            {
                SendInputUtil.CtrlS();
            }
        }

        private List<string> LoadProcesses()
        {
            var list = new List<string>();
            list.AddRange(this.processes);
            var fileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\process.txt";
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                list.AddRange(lines);
            }
            return list;
        }

        private Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

    }
}
