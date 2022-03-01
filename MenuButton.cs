using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace PathFindingAlgo
{
    public class MenuButton
    {
        bool ButtonVisability;
        public bool ActiveAnimation;
        public bool MoveUp;
        public bool MoveDown;

        Form Display;
        Brush ButtonBrush;
        Point ButtonPoint;
        Size ButtonSize;
        string ButtonType;
        Panel BoardDisplay;

        protected Image MenuBarIcon;
        int ImageDeltaY = 0;

        int CornerOffset;
        int ClickOffset;
        Rectangle HitBox;
        Rectangle ButtonRect;

        public MenuButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd)
        {
            ButtonVisability = true;
            ActiveAnimation = false;
            ClickOffset = 5;

            Display = fo;
            ButtonBrush = br;
            ButtonType = ty;
            ButtonPoint = po;
            ButtonSize = si;
            BoardDisplay = bd;
            

            CornerOffset = Math.Min(si.Width, si.Height) / 8;
            ButtonRect = new Rectangle(ButtonPoint, ButtonSize);
            HitBox = CreateHitBox(ButtonRect);
        }

        public void DrawButton(Graphics g)
        {
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

        public void BHover(Point MousePos, string obj)
        {
            if (HitBox.Contains(MousePos) && !ActiveAnimation && obj == "Main Display")
            {
                Thread animation = new Thread(Animate);
                animation.Start();
            }
            else if (ActiveAnimation && (!HitBox.Contains(MousePos) || obj != "Main Display"))
            {
                MoveDown = true;
                MoveUp = false;
            }
        }

        public void BClick(Point MousePos)
        {
            if (HitBox.Contains(MousePos)) ClickEvent();
        }

        private Rectangle CreateHitBox(Rectangle Box)
        {
            return Rectangle.Inflate(Box, ClickOffset, ClickOffset);
        }

        public void Animate()
        {
            ImageDeltaY = 0;
            ActiveAnimation = true;
            MoveUp = true;
            MoveDown = false;
            while (MoveUp && ButtonRect.Y > -10)
            {
                ButtonRect.Y -= 3;
                ImageDeltaY -= 3;
                Display.Invalidate();
                Thread.Sleep(20);
                if(ButtonRect.Y <= -10) MoveUp = false;
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

        public virtual void ClickEvent()
        {
            BoardDisplay.Invalidate();
        }
    }

    public class SaveButton: MenuButton
    {
        Board ABoard;
        public SaveButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board ab) : base(br, po, si, ty, fo, bd)
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
            base.ClickEvent();
        }
    }
    public class LoadButton: MenuButton
    {
        Board ABoard;
        Form Display;
        public LoadButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
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
            base.ClickEvent();
            }
        }
    }
    public class GridButton: MenuButton
    {
        Board ABoard;
        public GridButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Grid.png");
        }

        public override void ClickEvent()
        {
            ABoard.Grid = !ABoard.Grid;
            base.ClickEvent();
        }
    }
    public class DijkstraButton: MenuButton
    {
        Board ABoard;
        public DijkstraButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Path.png");
        }

        public override void ClickEvent()
        {
            ABoard.RunAstarAlgo();
            base.ClickEvent();
        }
    }
    public class ResetButton: MenuButton
    {
        Board ABoard;
        public ResetButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Reset.png");
        }

        public override void ClickEvent()
        {
            ABoard.ClearBoard();
            base.ClickEvent();
        }
    }
    public class RandomizeButton: MenuButton
    {
        Board ABoard;
        public RandomizeButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Randomize.png");
        }

        public override void ClickEvent()
        {
            ABoard.RandomizeBoard();
            base.ClickEvent();
        }
    }
    public class EnlargeBoardButton: MenuButton
    {
        Board ABoard;
        public EnlargeBoardButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Enlarge.png");
        }

        public override void ClickEvent()
        {
            ABoard.Enlarge();
            base.ClickEvent();
        }
    }
    public class ShrinkBoardButton: MenuButton
    {
        Board ABoard;
        public ShrinkBoardButton(Brush br, Point po, Size si, string ty, Form fo, Panel bd, Board aBoard) : base(br, po, si, ty, fo, bd)
        {
            ABoard = aBoard;
            MenuBarIcon = Image.FromFile(MainDisplay.rootfolder + @"Icons\Shrink.png");
        }

        public override void ClickEvent()
        {
            ABoard.Shrink();
            base.ClickEvent();
        }
    }
}


