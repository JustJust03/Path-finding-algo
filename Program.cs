using System;
using System.Windows.Forms;

namespace PathFindingAlgo
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainDisplay());
        }

    }
}
