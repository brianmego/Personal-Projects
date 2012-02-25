using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Acme.Common.Infrastructure;

namespace WinUICandy
{
    class MainWindowViewModel : ObservableObject
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        [DllImport("user32.dll", SetLastError = true)]

        public static extern bool GetWindowInfo(IntPtr hWnd, out WINDOWINFO pwi);
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);


        #region Commands

        public ICommand SwitchApplicationMonitorsCommand
        {
            get { return new RelayCommand(SwitchApplicationMonitors); }
        }

        #endregion


        #region Constructor
        public MainWindowViewModel()
        {
            
        }
        #endregion


        #region Methods

        public void SwitchApplicationMonitors()
        {
            
            var procs = Process.GetProcesses();
            foreach (Process p in procs)
            {
                MoveWindow(p.Handle, 0, 0, 100, 100,true);
            }
        }

        #endregion
    }
}
