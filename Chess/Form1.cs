using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BasicDrawTiles
{
    public partial class Form1 : Form
    {
        //int amount = 32;// Number of tiles
        int size = 15;// Graphical size of tiles
        bool greyscale = false;

        Bitmap frame;
        Graphics g;
        GameBoard gameboard;// Logic handler
        public Form1()
        {
            //gameboard = new GameBoard(amount, size);
            gameboard = new GameBoard(size);
            //int sumBuffer = (gameboard.Quads * size) + 1;  // For allocating memory, +1 is an offset
            int height = (gameboard.Quads * size) + 1;
            int width = (gameboard.Quads * gameboard.Chunks * size) + 1;

            frame = new Bitmap(width, height);
            g = Graphics.FromImage(frame);
            gameboard.startGraphics(g, frame);
            
            InitializeComponent();

            this.Height = height + 38;
            this.Width = width + 16;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            gameboard.updateGraphics(e, g, frame);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //switch (e.Button)
            //{
            //    case MouseButtons.Left:
            //        gameboard.movePiece(e);
            //        break;
            //}
            gameboard.mouseAction(e);
            Invalidate();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Space)
            {
                gameboard = new GameBoard(size);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyChar == 'g')
            {
                greyscale = !greyscale;
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '1')
            {
                gameboard = new GameBoard(size, 1);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '2')
            {
                gameboard = new GameBoard(size, 2);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '3')
            {
                gameboard = new GameBoard(size, 3);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '4')
            {
                gameboard = new GameBoard(size, 4);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '5')
            {
                gameboard = new GameBoard(size, 5);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '6')
            {
                gameboard = new GameBoard(size, 9);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
            else if (e.KeyChar == '7')
            {
                gameboard = new GameBoard(size, 7);
                gameboard.colorMap(greyscale);
                this.Invalidate();
            }
        }
    }
}
