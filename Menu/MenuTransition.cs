using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System;

namespace PathFindingAlgo
{
    /// <summary>
    /// Transitions between the game states
    /// </summary>
    public class MenuTransition
    {
        readonly MainDisplay MDisplay;
        // [0, 1] To keep track of the time and state the transition should be in
        double TimeDelta1;
        // To keep track of a transition is interupted mid transition
        bool InTransition;

        public MenuTransition(MainDisplay mdisplay)
        {
            MDisplay = mdisplay;
        }

        /// <summary>
        /// Runs the transition between the pre game and the in game state.
        /// The menu will slide up to reveal the board, and down to show the menu again.
        /// </summary>
        /// <param name="up">True if PreGame --> InGame, false if InGame --> PreGame</param>
        public void PreGameIngame(bool up)
        {
            //If the function is called during the transition, it reverses the time.
            if (InTransition)
            {
                TimeDelta1 = 1 - TimeDelta1;
            }
            else
            {
                InTransition = true;
                TimeDelta1 = 0;
            }
            while (TimeDelta1 <= 1)
            {
                TransposePanel(MDisplay.PGM, up);
                MDisplay.Invalidate();
                Application.DoEvents();
                TimeDelta1 += 0.05;
                Thread.Sleep(10);
            }
            InTransition = false;
        }

        /// <summary>
        /// Moves the panel up or down dependent on the EaseInOutCircle function and the timedelta.
        /// </summary>
        /// <param name="Window">Panel to be moved</param>
        /// <param name="up"></param>
        private void TransposePanel(Panel Window, bool up)
        {
            if (up)
                Window.Location = 
                    new Point(Window.Location.X, 1 - Convert.ToInt32(EaseInOutCirc(TimeDelta1) * (Window.Height + 1)));
            else if (!up)
                Window.Location = 
                    new Point(Window.Location.X, Convert.ToInt32(EaseInOutCirc(TimeDelta1) * Window.Height) - Window.Height);
        }

        /// <summary>
        /// Returns a y value of and x (TimeDelta)
        /// https://easings.net/#easeInOutCirc
        /// </summary>
        /// <param name="TD">TimeDelta</param>
        /// <returns></returns>
        private double EaseInOutCirc(double TD)
        {
            return TD < 0.5 ? 
                    (1 - Math.Sqrt(1 - Math.Pow(2 * TD, 2))) / 2 : 
                    (Math.Sqrt(1 - Math.Pow(-2 * TD + 2, 2)) + 1) / 2;
        }
    }
}
