using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BasicDrawTiles
{
    /// <summary>
    /// --- LAYERS ---
    /// BASE	Grass, Dirt, Stone, Water, Shallow water, Mud, Leaves
    /// FRINGE	Trees, Rocks, Reed, Moss
    /// MOVE	People, Enemies, Player
    /// </summary>
    class Map
    {
        public Tile[,] tiles;   // Layers

        // Diamond-Square
        public int Size { get; private set; } // ChunkSize
        private double Seed { get; set; }
        Random r = new Random();// in range of h
        private double Rand
        {
            get
            {
                return r.NextDouble();
            }
        }
        private int Chunks { get; set; }
        private double[,] ds;
        private double off;
        // Evaluation
        private int water;
        private int hill;
        public Map(int powerSize, double startValue, int nrOfChunks)
        {
            do
            {
                // Initiate point-map
                this.Size = (int)(Math.Pow(2, powerSize)) + 1;// +1 gives the map a mid-point
                Seed = startValue;
                Chunks = nrOfChunks;
                ds = new double[Size, ((Size - 1) * Chunks) + 1];// 33x33, 17x17, etc
                Size--;
                // Initiate map
                tiles = new Tile[Size, Size * Chunks];// 32x32, 16x16, etc

                // Create noise - algorithm usage
                DiamondSquare(30.0, 0.76789);
                //DiamondSquareSingle(30.0, 0.76789);

                // Load map
                LoadQuads();
            } while (evaluateMap());
        }
        public Map(int powerSize, double startValue, int nrOfChunks, int seed)
        {
            // Initiate seed
            setRandom(seed);
            // Initiate point-map
            this.Size = (int)(Math.Pow(2, powerSize)) + 1;// +1 gives the map a mid-point
            Seed = startValue;
            Chunks = nrOfChunks;
            ds = new double[Size, ((Size - 1) * Chunks) + 1];// 33x33, 17x17, etc
            Size--;
            // Initiate map
            tiles = new Tile[Size, Size * Chunks];// 32x32, 16x16, etc

            // Create noise - algorithm usage
            DiamondSquare(30.0, 0.76789);
            //DiamondSquareSingle(30.0, 0.76789);

            // Load map
            LoadQuads();
        }
        public Map(int amount, int size, bool a)    // Motion: remove rendering entirely from map and tile
        {
            // Initiate map
            tiles = new Tile[amount, amount];

            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    tiles[i, j] = new Tile(size);
                }
            }

            // Create noise - algorithm usage

            //--- Rule based algorithm and configurations ---\\
            // Set base tile

            // Add different type of terrain

            // Add lakes
            // then rivers

            // Add obstacles - trees and stones atm

        }

        public Map(int exponent, int nrOfChunks)
        {
            // Initiate map
            this.Size = (pow(2, exponent) + 1);// +1 gives the map a mid-point
            Chunks = nrOfChunks;
            Size--;
            tiles = new Tile[Size, Size * Chunks];// 32x32, 16x16, etc

            for (int h = 0; h < Size; h++)
            {
                for (int w = 0; w < Size * Chunks; w++)
                {
                    tiles[h, w] = new Tile(10);
                }
            }
        }
        private void LoadQuads()
        {
            double avg;
            for (int h = 0; h < Size; h++)
            {
                for (int w = 0; w < (Size * Chunks); w++)
                {
                    avg = average(h, w, 1);

                    tiles[h, w] = new Tile(avg);

                    evaluateTile(tiles[h, w]);
                }
            }
        }
        private void setRandom(int value)
        {
            if(value < 1)
            {
                r = new Random();
            }
            else
            {
                r = new Random(value);
            }
        }
        private void DiamondSquare(double range, double decrease)
        {
            // Starting value for corners
            for (int s = 0; s < Chunks; s++)
            {
                double offset = 1.2;
                this.ds[0, (Size * s)] = Seed + (Seed * Rand * offset);// Top left
                this.ds[0, Size + (Size * s)] = Seed + (Seed * Rand * offset);// Top right
                this.ds[Size, (Size * s)] = Seed + (Seed * Rand * offset);// Bot left
                this.ds[Size, Size + (Size * s)] = Seed + (Seed * Rand * offset);// Bot right
            }

            off = range;// the range (-off -> +off) for the average offset
            double avg;
            for (int i = Size; i >= 2; i /= 2, off *= decrease)// decrease the variation of the offset
            {
                int halfI = i / 2;
                // generate new square values
                for (int h = 0; h < Size; h += i)
                {
                    for (int w = 0; w < (Size * Chunks); w += i)
                    {
                        avg = average(h, w, i);

                        // center is average plus random offset
                        ds[h + halfI, w + halfI] = avg + (Rand * 2 * off) - off;
                    }
                }//__SQUARE_END__//

                // generate the diamond values
                for (int h = 0; h < (Size + 1); h += halfI)
                {
                    for (int w = (h + halfI) % i; w < ((Size * Chunks) + 1); w += i)
                    {
                        avg = ds[(h - halfI + Size) % (Size), w] +// left of center
                              ds[(h + halfI) % (Size), w] +// right of center
                              ds[h, (w + halfI) % (Size)] +// below center
                              ds[h, (w - halfI + Size) % (Size)];// above center
                        avg /= 4.0;

                        // new value = average plus random offset
                        avg = avg + (Rand * 2 * off) - off;
                        // update value
                        ds[h, w] = avg;

                        // remove this below and adjust loop condition above
                        if (h == 0)
                        {
                            ds[Size, w] = avg;
                        }
                        if (w == 0) // Due to modulus-usage
                        {
                            ds[h, Size] = avg;
                        }
                    }
                }//__DIAMOND_END__//
            }//__HEIGHT_MAP_END__//
        }//__FUNCTION_END__//
        private void DiamondSquareSingle(double range, double decrease)
        {
            // Starting value for corners
            for (int s = 0; s < Chunks; s++)
            {
                double offset = 1.2;
                this.ds[0, (Size * s)] = Seed + (Seed * Rand * offset);// Top left
                this.ds[0, Size + (Size * s)] = Seed + (Seed * Rand * offset);// Top right
                this.ds[Size, (Size * s)] = Seed + (Seed * Rand * offset);// Bot left
                this.ds[Size, Size + (Size * s)] = Seed + (Seed * Rand * offset);// Bot right
            }

            for (int c = 0; c < Chunks; c++)
            {
                // Starting value for corners // Start location here gives a somewhat tacky pattern
                //double offset = 1.2;
                //this.ds[0, (Size * c)] = Seed + (Seed * Rand * offset);// Top left
                //this.ds[0, Size + (Size * c)] = Seed + (Seed * Rand * offset);// Top right
                //this.ds[Size, (Size * c)] = Seed + (Seed * Rand * offset);// Bot left
                //this.ds[Size, Size + (Size * c)] = Seed + (Seed * Rand * offset);// Bot right

                off = range;// the range (-off -> +off) for the average offset
                double avg;
                for (int i = Size; i >= 2; i /= 2, off *= decrease)// decrease the variation of the offset
                {
                    int halfI = i / 2;
                    // generate new square values
                    for (int h = 0; h < Size; h += i)
                    {
                        for (int w = 0; w < Size; w += i)
                        {
                            avg = average(h, w, i);

                            // center is average plus random offset
                            ds[h + halfI, w + halfI + (Size * c)] = avg + (Rand * 2 * off) - off;
                        }
                    }//__SQUARE_END__//

                    // generate the diamond values
                    for (int h = 0; h < (Size + 1); h += halfI)
                    {
                        for (int w = (h + halfI) % i; w < (Size + 1); w += i)
                        {
                            avg = ds[(h - halfI + Size) % (Size), w + (Size * c)] +// left of center
                                  ds[(h + halfI) % (Size), w + (Size * c)] +// right of center
                                  ds[h, (w + halfI + (Size * c)) % (Size)] +// below center
                                  ds[h, (w - halfI + Size + (Size * c)) % (Size)];// above center
                            avg /= 4.0;

                            // new value = average plus random offset
                            avg = avg + (Rand * 2 * off) - off;
                            // update value
                            ds[h, w + (Size * c)] = avg;

                            // remove this below and adjust loop condition above
                            if (h == 0)
                            {
                                ds[Size, w + (Size * c)] = avg;
                            }
                            if (w == 0) // Due to modulus-usage
                            {
                                ds[h, Size + (Size * c)] = avg;
                            }
                        }
                    }//__DIAMOND_END__//
                }//__HEIGHT_MAP_END__//
            }//__CHUNKS_PASSED__//
        }//__FUNCTION_END__//

        private void evaluateTile(Tile tile)
        {
            if(tile.Height < 60)
            {
                tile.Obstacle = true;
                water++;
            }
            else if(tile.Height > 180)
            {
                tile.Obstacle = true;
                hill++;
            }
        }
        private bool evaluateMap()
        {
            bool redo = false;
            int mapSize = Size * Size;
            float calc = mapSize;
            calc = water / calc;
            if(calc < 0.03 || calc > 0.18)
            {
                redo = true;
                water = 0;
                hill = 0;
            }

            calc = mapSize;
            calc = hill / calc;
            if (calc > 0.10)
            {
                redo = true;
                water = 0;
                hill = 0;
            }

            return redo;
        }
        private int pow(int power, int exponent)
        {
	        int b = 1;
	        if (exponent > 0)
	        {
		        b = 2;
		        for (int i = 1; i < exponent; i++)
		        {
                    b *= power;
		        }
	        }
	        else
	        {
		        b = 1;
	        }
	        return b;
        }
        private double average(int h, int w, int i)
        {
            // calculate average of existing corners
            double avg = ds[h, w] +// top left
                  ds[h + i, w] +// top right
                  ds[h, w + i] +// bot left
                  ds[h + i, w + i];// bot right
            avg /= 4.0;

            return avg;
        }
    }
}
