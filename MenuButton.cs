using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace PathFindingAlgo
{
    /// <summary>
    /// A Button displayed on a form.
    /// Should only be used as a base class for other buttons.
    /// Has property's to animate and draw different buttons.
    /// </summary>
    public abstract class MenuButton
    {
        bool ButtonVisability;

        public bool ActiveAnimation;
        public bool MoveUp;
        public bool MoveDown;
        const int MaxUpPixels = 10;
        int ImageDeltaY = 0;

        readonly Form Display;
        readonly Brush ButtonBrush;
        Point ButtonPoint;
        Size ButtonSize;
        readonly string ButtonType;

        readonly string ButtonText;
        readonly Font TextFont = new Font("Times New Roman", 14, FontStyle.Bold);
        readonly Brush TextBrush = new SolidBrush(Color.Black);
        Point TextPoint;
        Size TextSize;

        protected Image MenuBarIcon;

        readonly int CornerOffset;
        const int ClickOffset = 5;
        Rectangle HitBox;
        Rectangle ButtonRect;

        /// <summary>
        /// The constructor of the Menubutton to declare all variables, except the animations.
        /// </summary>
        /// <param name="br">Back color of the button</param>
        /// <param name="po">Top left point from where the button is drawn</param>
        /// <param name="si">The width and height of the button</param>
        /// <param name="ty">Button Types: "Rounded Box"</param>
        /// <param name="fo">The form to be drawn on</param>
        /// <param name="bt">A string to be displayed under the button, only visible on hover.</param>
        public MenuButton(Brush br, Point po, Size si, string ty, Form fo, string bt)
        {
            ButtonVisability = true;
            ActiveAnimation = false;

            Display = fo;
            ButtonBrush = br;
            ButtonType = ty;
            ButtonPoint = po;
            ButtonSize = si;
            ButtonText = bt;

            TextSize = TextRenderer.MeasureText(ButtonText, TextFont); 
            TextPoint = new Point(ButtonPoint.X + (ButtonSize.Width - TextSize.Width) / 2 , ButtonPoint.Y + Convert.ToInt32(ButtonSize.Height * 0.85) - MaxUpPixels);

            CornerOffset = Math.Min(si.Width, si.Height) / 8;
            ButtonRect = new Rectangle(ButtonPoint, ButtonSize);
            HitBox = Rectangle.Inflate(ButtonRect, ClickOffset, ClickOffset);
        }

        /// <summary>
        /// 1. Draws the buttontext.
        /// 2. Draws the button itself dependent on its button type.
        /// 3. Draws the Icon.
        /// </summary>
        /// <param name="g"></param>
        public void DrawButton(Graphics g)
        {
            g.DrawString(ButtonText, TextFont, TextBrush, TextPoint);
            if (ButtonVisability && ButtonType == "Rounded Box")
            {
                g.FillRectangle(ButtonBrush, new Rectangle(new Point(ButtonRect.X + CornerOffset, ButtonRect.Y), new Size(ButtonRect.Width - CornerOffset * 2, ButtonRect.Height)));
                g.FillRectangle(ButtonBrush, new Rectangle(new Point(ButtonRect.X, ButtonRect.Y + CornerOffset), new Size(ButtonRect.Width, ButtonRect.Height - CornerOffset * 2)));

                Size CornerSize = new Size(CornerOffset * 2, CornerOffset * 2);
                g.FillEllipse(ButtonBrush, new Rectangle(new Point(ButtonRect.X, ButtonRect.Y), CornerSize));
                g.FillEllipse(ButtonBrush, new Rectangle(new Point(ButtonRect.X + ButtonRect.Width - CornerOffset * 2, ButtonRect.Y), CornerSize));
                g.FillEllipse(ButtonBrush, new Rectangle(new Point(ButtonRect.X + ButtonRect.Width - CornerOffset * 2 , ButtonRect.Y + ButtonRect.Height - CornerOffset * 2), CornerSize));
                g.FillEllipse(ButtonBrush, new Rectangle(new Point(ButtonRect.X, ButtonRect.Y + ButtonRect.Height - CornerOffset * 2), CornerSize));
            }
            else if (ButtonVisability)
                g.FillRectangle(ButtonBrush, ButtonRect);

            if (MenuBarIcon != null)
            {
                g.DrawImage(MenuBarIcon, ButtonPoint.X + Convert.ToInt32(ButtonSize.Width * 0.1),
                                         ButtonPoint.Y + ImageDeltaY + Convert.ToInt32(ButtonSize.Height * 0.1),
                                         Convert.ToInt32(ButtonSize.Width * 0.8),
                                         Convert.ToInt32(ButtonSize.Height * 0.8));
            }
        }

        /// <summary>
        /// Starts the Up Down Animation when the button is hovered.
        /// </summary>
        public void BHover(Point MousePos, string obj)
        {
            if (HitBox.Contains(MousePos) && !ActiveAnimation && obj == "Main Display")
            {
                Thread animation = new Thread(UpDownAnimation);
                animation.Start();
            }
            else if (ActiveAnimation && (!HitBox.Contains(MousePos) || obj != "Main Display"))
            {
                MoveDown = true;
                MoveUp = false;
            }
        }

        /// <summary>
        /// Performs the individual click event when the button is pressed.
        /// </summary>
        public void BClick(Point MousePos)
        {
            if (HitBox.Contains(MousePos)) ClickEvent();
        }

        /// <summary>
        /// Animation to push the button MaxUpPixels up, to visualize the text of the button.
        /// When the Mouse isn't hovering the button anymore, sink to button to its original point. 
        /// </summary>
        public void UpDownAnimation()
        {
            ImageDeltaY = 0;
            ActiveAnimation = true;
            MoveUp = true;
            MoveDown = false;
            while (MoveUp && ButtonRect.Y > -MaxUpPixels)
            {
                ButtonRect.Y -= 3;
                ImageDeltaY -= 3;
                Display.Invalidate();
                Thread.Sleep(20);
                if(ButtonRect.Y <= -MaxUpPixels) MoveUp = false;
            }
            while (!MoveDown) Thread.Sleep(20);
            while (MoveDown)
            {
                ButtonRect.Y += 3;
                ImageDeltaY += 3;
                Display.Invalidate();
                Thread.Sleep(20);
                if(ButtonRect.Y == ButtonPoint.Y)
                {
                    MoveDown = false;
                    ActiveAnimation = false;
                }
            }
        }

        /// <summary>
        /// The idividual click event of the button.
        /// </summary>
        public abstract void ClickEvent();
    }

    /// <summary>
    /// Creates a dialog and saves the board.
    /// </summary>
    public class SaveButton: MenuButton
    {
        Board ABoard;
        public SaveButton(Brush br, Point po, Size si, string ty, Form fo, Board ab) : base(br, po, si, ty, fo, "Save")
        {
            ABoard = ab;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Save.png");
        }

        public override void ClickEvent()
        {
            ABoard.ClearBoard();
            string csv = ABoard.ToString();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV Files|*.csv|Alle Files|*.*";
            dialog.Title = "Bestand opslaan als...";
            dialog.FileName = "AstarBoard";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dialog.FileName);
                sw.Write(csv);
                sw.Close();
            }
        }
    }
    /// <summary>
    /// Creates a dialog and loads a board from a csv file.
    /// </summary>
    public class LoadButton: MenuButton
    {
        Board ABoard;
        Form Display;
        public LoadButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Load-In")
        {
            ABoard = aBoard;
            Display = fo;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Load.png");
        }

        public override void ClickEvent()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV Files|*.csv|Alle Files|*.*";
            dialog.Title = "Tekst openen...";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(dialog.FileName);
                Display.Text = dialog.FileName;
                ABoard.Load_board(sr);
                sr.Close();
            }
        }
    }
    /// <summary>
    /// Toggles the grid
    /// </summary>
    public class GridButton: MenuButton
    {
        Board ABoard;
        public GridButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Grid")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Grid.png");
        }

        public override void ClickEvent()
        {
            ABoard.ToggleGrid();
        }
    }
    /// <summary>
    /// Runs the AStar algorithm.
    /// </summary>
    public class AstarButton: MenuButton
    {
        Board ABoard;
        public AstarButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Run")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Path.png");
        }

        public override void ClickEvent()
        {
            ABoard.RunAstarAlgo();
        }
    }
    /// <summary>
    /// Resets the board by removing the path cells, and restoring the begin and endcell.
    /// </summary>
    public class ResetButton: MenuButton
    {
        Board ABoard;
        public ResetButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Reset")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Reset.png");
        }

        public override void ClickEvent()
        {
            ABoard.ClearBoard();
        }
    }
    /// <summary>
    /// Randomizes the board.
    /// </summary>
    public class RandomizeButton: MenuButton
    {
        Board ABoard;
        public RandomizeButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Random")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Randomize.png");
        }

        public override void ClickEvent()
        {
            ABoard.RandomizeBoard();
        }
    }
    /// <summary>
    /// Enlarges the board by the size of 2 in both directions.
    /// </summary>
    public class EnlargeBoardButton: MenuButton
    {
        Board ABoard;
        public EnlargeBoardButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Enlarge")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Enlarge.png");
        }

        public override void ClickEvent()
        {
            ABoard.Enlarge();
        }
    }
    /// <summary>
    /// Shrinks the board by the size of 2 in both directions.
    /// </summary>
    public class ShrinkBoardButton: MenuButton
    {
        Board ABoard;
        public ShrinkBoardButton(Brush br, Point po, Size si, string ty, Form fo, Board aBoard) : base(br, po, si, ty, fo, "Shrink")
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Shrink.png");
        }

        public override void ClickEvent()
        {
            ABoard.Shrink();
        }
    }
}


