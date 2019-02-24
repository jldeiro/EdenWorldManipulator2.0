using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Eden_World_Maniputor_2._0
{
    class Tree
    {
        public static int newbaseHeight, newaddress, newChunkX, newChunkY, newi, newj;
        public static List<int> pine = new List<int>();
        public static List<int> normal = new List<int>();

        public static void PlaceLeaf(int i, int j, int k, int z, World world, int baseX, int baseY, int address, int baseHeight, int type, int color, List<int> ct)
        {
            if (i < 0 || i > 15 || j < 0 || j > 15)
            {
                if (i < 0)
                {
                    newi = 16 + i;
                    newChunkX = world.WorldArea.X + (baseX - 16) / 16;
                }
                else if (i > 15)
                {
                    newi = i - 16;
                    newChunkX = world.WorldArea.X + (baseX + 16) / 16;
                }
                else if (i <= 15 || i >= 0)
                {
                    newi = i;
                    newChunkX = world.Chunks[address].X;
                }
                if (j < 0)
                {
                    newj = 16 + j;
                    newChunkY = world.WorldArea.Y + (baseY - 16) / 16;
                }
                else if (j > 15)
                {
                    newj = j - 16;
                    newChunkY = world.WorldArea.Y + (baseY + 16) / 16;
                }
                else if (j <= 15 || j >= 0)
                {
                    newj = j;
                    newChunkY = world.Chunks[address].Y;
                }
                newaddress = world.Chunks.First(c => c.Value == (new Point(newChunkX, newChunkY))).Key;
            }
            else
            {
                newi = i; newj = j;
                newaddress = address;
            }
            newbaseHeight = (((baseHeight * 16) + z) + (k - z)) / 16;
            if (k > 15) k = k - 16;
            world.Bytes[newaddress + newbaseHeight * 8192 + newi * 256 + newj * 16 + k] = (byte)type;
            world.Bytes[newaddress + newbaseHeight * 8192 + newi * 256 + newj * 16 + k + 4096] = (byte)ct[color];
        }

        public static void PlaceStump (int baseHeight, int address, int x, int y, int z, int height, World world)
        {
            for (int t = z; t < z + height; t++)
            {
                newbaseHeight = (((baseHeight * 16) + z) + (t - z)) / 16;
                if (t > 15) t = t - 16;
                world.Bytes[address + newbaseHeight * 8192 + x * 256 + y * 16 + t] = 6;
                world.Bytes[address + newbaseHeight * 8192 + x * 256 + y * 16 + t + 4096] = 47;
                if (t < z) t = t + 16;
            }
        }

        public static void PineTree(int x, int y, int z,Random rnd, int address, int baseHeight, World world, int baseX, int baseY)
        {
            PlaceStump(baseHeight, address, x, y, z, 2, world);
            
            int color = rnd.Next(pine.Count);
            int type = 5;

            for (int k = z + 2; k < z + 10; k++)
            {
                for (int i = x - 2; i <= x + 2; i++)
                {
                    for (int j = y - 2; j <= y + 2; j++)
                    {
                        if (k == z + 2 || k == z + 4)
                        {
                            if (((i == x - 2 || i == x + 2) && j == y) || ((j == y - 2 || j == y + 2) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                        else if (k == z + 3 || k == z + 5 || k == z + 7)
                        {
                            if (((i == x - 1 || i == x + 1) && j == y) || ((j == y - 1 || j == y + 1) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2 && i != x - 1 && i != x + 1 && j != y - 1 && j != y + 1)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                        else if (k == z + 6 || k == z + 8 || k == z + 9)
                        {
                            if (j == y && i == x)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                    }
                }
            }
        }

        public static void TallPineTree(int x, int y, int z, Random rnd, int address, int baseHeight, World world, int baseX, int baseY)
        {
            PlaceStump(baseHeight, address, x, y, z, 2, world);

            int color = rnd.Next(pine.Count);
            int type = 5;

            for (int k = z + 2; k < z + 13; k++)
            {
                for (int i = x - 3; i <= x + 3; i++)
                {
                    for (int j = y - 3; j <= y + 3; j++)
                    {
                        if (k == z + 2 || k == z + 4)
                        {
                            if (((i == x - 3 || i == x + 3) && j == y) || ((j == y - 3 || j == y + 3) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            else if (i != x - 3 && i != x + 3 && j != y - 3 && j != y + 3)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            if ((i == x - 2 && j == y - 2) || (i == x - 2 && j == y + 2) || (i == x + 2 && j == y - 2) || (i == x + 2 && j == y + 2))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, 0, 0, pine);
                            }
                        }
                        else if (k == z + 3 || k == z + 5 || k == z + 7)
                        {
                            if (((i == x - 2 || i == x + 2) && j == y) || ((j == y - 2 || j == y + 2) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2 && i != x - 3 && i != x + 3 && j != y - 3 && j != y + 3)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                        else if (k == z + 6 || k == z + 8 || k == z + 10)
                        {
                            if (((i == x - 1 || i == x + 1) && j == y) || ((j == y - 1 || j == y + 1) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                            else if (i != x - 3 && i != x + 3 && j != y - 3 && j != y + 3 && i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2 && i != x - 1 && i != x + 1 && j != y - 1 && j != y + 1)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                        else if (k == z + 9 || k == z + 11 || k == z + 12)
                        {
                            if (j == y && i == x)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, pine);
                            }
                        }
                    }
                }
            }
        }

        public static void NormalTerrainTree(int x, int y, int z, Random rnd, int address, int baseHeight, World world, int baseX, int baseY)
        {
            int tree_height = rnd.Next(6) + 6;
            
            PlaceStump(baseHeight, address, x, y, z, 3 * tree_height / 4, world);

            int color = rnd.Next(normal.Count);
            int type = 5;

            for (int i = x - 2; i <= x + 2; i++)
            {
                for (int j = y - 2; j <= y + 2; j++)
                {
                    for (int k = z + 2 * tree_height / 3; k < tree_height + z; k++)
                    {
                        if (i == x - 2 || i == x + 2 || j == y - 2 || j == y + 2)
                        {
                            if ((i == x - 2 || i == x + 2) && (j == y - 2 || j == y + 2) && (k == z + 2 * tree_height / 3 || k == z + tree_height - 1))
                            {
                            }
                            else
                                if (rnd.Next(2) == 0)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                        }
                        else
                        {
                            PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                        }
                    }
                }
            }
        }

        public static void NormalTree(int x, int y, int z, Random rnd, int address, int baseHeight, World world, int baseX, int baseY)
        {
            int tree_height = rnd.Next(6) + 3;

            PlaceStump(baseHeight, address, x, y, z, tree_height, world);

            int color = rnd.Next(normal.Count);
            int type = 5;

            for (int k = z + tree_height; k < z + tree_height + 4; k++)
            {
                for (int i = x - 2; i <= x + 2; i++)
                {
                    for (int j = y - 2; j <= y + 2; j++)
                    {
                        if (k == z + tree_height || k == z + tree_height + 2)
                        {
                            if (((i == x - 1 || i == x + 1) && j == y) || ((j == y - 1 || j == y + 1) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2 && i != x - 1 && i != x + 1 && j != y - 1 && j != y + 1)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                        }
                        else if (k == z + tree_height + 1)
                        {
                            if (((i == x - 2 || i == x + 2) && j == y) || ((j == y - 2 || j == y + 2) && i == x))
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                        }
                        else if (k == z + tree_height + 3)
                        {
                            if (j == y && i == x)
                            {
                                PlaceLeaf(i, j, k, z, world, baseX, baseY, address, baseHeight, type, color, normal);
                            }
                        }
                    }
                }
            }
        }

        public static void RealTree(int x, int y, int z, byte?[,,,] newMap, Random rnd)
        {
            int tree_height = rnd.Next(6) + 3;

            for (int i = 0; i < tree_height; i++)
            {
                newMap[x, y, z + i, 0] = 6;
                newMap[x, y, z + i, 1] = 0;
            }

            int[] ct2 = { 0, 19, 20, 21, 31, 40, 40 };
            int[] ct = { 0, 19, 20, 21, 31, 40, 40 };
            int color = rnd.Next(7);
            int type = 5;

            for (int k = z + tree_height; k < z + tree_height + 4; k++)
            {
                for (int i = x - 2; i <= x + 2; i++)
                {
                    for (int j = y - 2; j <= y + 2; j++)
                    {
                        if (k == z + tree_height || k == z + tree_height + 2)
                        {
                            if (((i == x - 1 || i == x + 1) && j == y) || ((j == y - 1 || j == y + 1) && i == x))
                            {
                                newMap[i, j, k, 0] = (byte)type;
                                newMap[i, j, k, 1] = (byte)ct[color];
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2 && i != x - 1 && i != x + 1 && j != y - 1 && j != y + 1)
                            {
                                newMap[i, j, k, 0] = (byte)type;
                                newMap[i, j, k, 1] = (byte)ct[color];
                            }
                        }
                        else if (k == z + tree_height + 1)
                        {
                            if (((i == x - 2 || i == x + 2) && j == y) || ((j == y - 2 || j == y + 2) && i == x))
                            {
                                newMap[i, j, k, 0] = (byte)type;
                                newMap[i, j, k, 1] = (byte)ct[color];
                            }
                            else if (i != x - 2 && i != x + 2 && j != y - 2 && j != y + 2)
                            {
                                newMap[i, j, k, 0] = (byte)type;
                                newMap[i, j, k, 1] = (byte)ct[color];
                            }
                        }
                        else if (k == z + tree_height + 3)
                        {
                            if (j == y && i == x)
                            {
                                newMap[i, j, k, 0] = (byte)type;
                                newMap[i, j, k, 1] = (byte)ct[color];
                            }
                        }
                    }
                }
            }
        }
    }
}
