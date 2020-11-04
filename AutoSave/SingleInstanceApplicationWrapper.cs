using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSave
{
    public class SingleInstanceApplicationWrapper : WindowsFormsApplicationBase
    {
        private readonly Form _mainForm;

        public SingleInstanceApplicationWrapper(Form form)
        {
            this._mainForm = form;
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            if (eventArgs.CommandLine != null && eventArgs.CommandLine.Count > 0)
            {
                if (eventArgs.CommandLine.Any(p => p == "-auto"))
                {
                    this._mainForm.ShowInTaskbar = false;
                    this._mainForm.Visible = false;
                    this._mainForm.WindowState = FormWindowState.Minimized;
                }
            }
            Application.Run(this._mainForm);
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            if (eventArgs.CommandLine != null && eventArgs.CommandLine.Count > 0)
            {
                if (eventArgs.CommandLine.Any(p => p == "-auto"))
                {
                    return;
                }
            }

            this._mainForm.Visible = true;
            this._mainForm.ShowInTaskbar = true;
            this._mainForm.WindowState = FormWindowState.Normal;
        }
    }
}
