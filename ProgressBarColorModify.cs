using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
/*
 * This class can change the color of a ProgressBar to red, yellow and green.
 *
 * If you want to use this class in your project, just use the SetStatus function
 * in front of a ProgressBar object that you have defined in your Windows Form project
 * and use the numbers 1, 2 and 3 in the parentheses.
 * Example 1 : [ProgressBar_Name].SetStatus(1); => CHANGE COLOR PROGRESSBAR TO GREEN
 * Example 2 : [ProgressBar_Name].SetStatus(2); => CHANGE COLOR PROGRESSBAR TO RED
 * Example 3 : [ProgressBar_Name].SetStatus(3); => CHANGE COLOR PROGRESSBAR TO YELLOW
 * 1 : GREEN
 * 2 : RED
 * 3 : YELLOW
 */
namespace TankShooter
{
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetStatus(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
