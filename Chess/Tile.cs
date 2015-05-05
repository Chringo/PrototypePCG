using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BasicDrawTiles
{
    class Tile
    {
        public Rectangle Body { get; private set; }
        public Rectangle BodyObs { get; private set; }
        int gSize { get; set; }
        private int color;
        public bool Obstacle { get; set; }
        public bool Grey { get; set; }

        private double height;
        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                if (this.height < 0) this.height = 0;
                if (this.height > 255) this.height = 255;
            }
        }

        public Tile(double startValue)
        {
            this.Height = startValue;
            this.gSize = 15;
            this.color = (int)this.Height;
            Obstacle = false;
            Grey = false;
        }

        public void colorBody(Graphics g, int x, int y)
        {
            Body = new Rectangle(x, y * gSize, gSize, gSize);
            BodyObs = new Rectangle(x + 2, (y * gSize) + 2, (gSize - 4), (gSize - 4));

            Pen pen = new Pen(Color.FromArgb(0, 0, 0));
            Pen penObs = new Pen(Color.FromArgb(200, 25, 25));
            Brush brush;

            if (!Grey)
            {
                brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                if (color < 60) // Water - Stone spawn
                {
                    //Obstacle = true;
                    int a = 30;
                    int b = 10;
                    if (color < 30)
                    {
                        a = 0;
                    }
                    if (color < 10)
                    {
                        b = 0;
                    }
                    brush = new SolidBrush(Color.FromArgb(color - (a), color - (b), 153));
                }
                else if (color > 59 && color < 70) // Mud - Stone spawn
                {
                    brush = new SolidBrush(Color.FromArgb(color - 30, color - 10, 0));
                }
                else if (color > 69 && color < 89) // Grass - Tree Spawn
                {
                    brush = new SolidBrush(Color.FromArgb(color - 50, color, 0));
                }

                else // Default grass
                {
                    brush = new SolidBrush(Color.FromArgb(color, 153, color));
                }
            }
            else
                brush = new SolidBrush(Color.FromArgb(color, color, color));

            g.FillRectangle(brush, Body);
            g.DrawRectangle(pen, Body);
            if (Obstacle)
            {
                g.DrawRectangle(penObs, BodyObs);
            }
        }

        public void setTileValue(int value)
        {
            this.Height = value;
            this.color = (int)this.Height;
        }

        public Tile(int size)
        {
            this.gSize = size;
            this.Height = 200;
            this.color = (int)this.Height;
            Obstacle = false;
        }
    }
}
