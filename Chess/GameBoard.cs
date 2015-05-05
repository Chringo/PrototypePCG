using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BasicDrawTiles
{
    class GameBoard
    {
        int gSize;// Graphical size on tiles

        private int nrOfChunks;
        public int Chunks
        {
            get
            {
                return nrOfChunks;
            }
            set
            {
                this.nrOfChunks = value;
                if (nrOfChunks < 1) nrOfChunks = 1;
            }
        }
        private Map map;

        public int Quads { get; private set; }// Number of tiles per chunk - quadratic set, e.g. 32x32

        public GameBoard(int size)
        {
            this.gSize = size;
            this.Chunks = 1;
            map = new Map(6, 60.0, Chunks);
            //map = new Map(5, Chunks);
            Quads = map.Size;
        }
        public GameBoard(int size, int seed)
        {
            this.gSize = size;
            this.Chunks = 1;
            map = new Map(6, 60.0, Chunks, seed);
            //map = new Map(5, Chunks);
            Quads = map.Size;
        }
        public GameBoard(/*int size, int nrOfChunks*/)
        {
            //this.gSize = size;
            //this.Chunks = nrOfChunks;
            //map = new Map(5, 95.0, Chunks);
            ////map = new Map(5, Chunks);
            //Quads = map.Size;
        }

        // Updates visuals
        private void colorMap(Graphics g)
        {
            for (int h = 0; h < Quads; h++)
            {
                for (int w = 0; w < Quads * Chunks; w++)
                {
                    // Tiles
                    map.tiles[h, w].colorBody(g, w * gSize, h);
                }
            }
        }
        public void colorMap(bool grey) // Removes "textures"
        {
            for (int h = 0; h < Quads; h++)
            {
                for (int w = 0; w < Quads * Chunks; w++)
                {
                    // Tiles
                    map.tiles[h, w].Grey = grey;
                }
            }
        }

        public void startGraphics(Graphics g, Bitmap frame)
        {
            g = Graphics.FromImage(frame);
            g.Clear(Color.Black);
            colorMap(g);
            g.Dispose();
        }

        public void updateGraphics(PaintEventArgs e, Graphics g, Bitmap frame)
        {
            g = Graphics.FromImage(frame);
            colorMap(g);
            g.Dispose();
            e.Graphics.DrawImageUnscaled(frame, 0, 0);
        }

        public void mouseAction(MouseEventArgs e)
        {
            for (int h = 0; h < Quads; h++)
            {
                for (int w = 0; w < Quads * Chunks; w++)
                {
                    if (map.tiles[h, w].Body.Contains(e.Location))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            map.tiles[h, w].Obstacle = !(map.tiles[h, w].Obstacle);
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            map.tiles[h, w].setTileValue(255);
                        }
                    }
                }
            }
        }
    }
}
