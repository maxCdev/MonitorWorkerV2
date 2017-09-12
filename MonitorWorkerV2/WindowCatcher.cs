using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonitorWorkerV2
{
    class WindowCatcher
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private static int WM_CLOSE = 0x10;

        /// <summary>
        /// Disable maxmize, minimize, close buttons
        /// </summary>
        /// <param name="hwnd"></param>
        private void DisableResize(IntPtr hwnd)
        {
            var value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~(WS_MAXIMIZEBOX|WS_MINIMIZEBOX| WM_CLOSE)));
        }
        /// <summary>
        /// Set target process window as app window children
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handle"></param>
        public void CatchWindow(string path, IntPtr handle)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int timeout = 10 * 1000;     // Timeout value (10s) in case we want to cancel the task if it's taking too long.

            Process p = Process.Start(path);
            while (p.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(10);
                p.Refresh();

                if (sw.ElapsedMilliseconds > timeout)
                {
                    sw.Stop();
                    return;
                }
            }

            SetParent(p.MainWindowHandle, handle);      // Set the process parent window to the window we want
            SetWindowPos(p.MainWindowHandle, 0, 0, 0, 0, 0, 0x0001 | 0x0040);       // Place the window in the top left of the parent window without resizing it
            MoveWindow(p.MainWindowHandle, 10, 10, 100, 100, true);
            DisableResize(p.MainWindowHandle);
        }
    }
}
