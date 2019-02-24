using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Eden_World_Maniputor_2._0
{
    public static partial class Manipulator
    {
        public static bool normal = false, normalterrain = false, smallpine = false, largepine = false;
        public static byte[] undo;
        public static int Density;

        public static void Manipulate(World world, Func<Block, int, int, int, int, Block> manipulatorDelegate, String path)
        {
            Random rnd = new Random();

            Block block;
            int length = xYxY.Count;
            int x1, y1, newbaseX, newbaseY, newaddress, chunknewX, chunknewY, newZ, newHeight;
            undo = world.Bytes.ToArray();

            if (manipulatorDelegate == Winterize)
            {
                foreach (int address in world.Chunks.Keys)
                {
                    int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                    for (int x = 0; x < 16; x++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                            {
                                for (int z = 15; z >= 0; z--)
                                {
                                    if ((baseHeight * 16 + z) == 20 && (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 8 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 5) && done == false)
                                    {
                                        done = true;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 4;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 9;
                                    }
                                    else if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 5 && done == false)
                                    {
                                        done = true;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 5;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 9;
                                    }
                                    else if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 8 && done == false)
                                    {
                                        done = true;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 4;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 9;
                                    }
                                    else if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 20 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 59 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 60 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 61 && done == false)
                                    {
                                        done = true;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z];
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096];
                                    }
                                    else if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0 && world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 20 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 59 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 60 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 61 && done == false)
                                    {
                                        done = true;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z];
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 9;
                                    }
                                    else
                                    {
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z];
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096];
                                    }
                                    if (z == 0 && baseHeight == 0)
                                    {
                                        done = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (manipulatorDelegate == MoveWorld)
            {
                foreach (int address in world.Chunks.Keys)
                {
                    if (world.WorldArea.X + ((X1 + XMove) / 16) <= world.Chunks[address].X && world.WorldArea.X + ((X2 + XMove) / 16) >= world.Chunks[address].X)
                    {
                        int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                        for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                        {
                            for (int x = 0; x < 16; x++)
                            {
                                for (int y = 0; y < 16; y++)
                                {
                                    if ((baseY + y) >= Y1 + YMove && (baseY + y) <= Y2 + YMove && (baseX + x) >= X1 + XMove && (baseX + x) <= X2 + XMove)
                                    {
                                        chunknewX = world.WorldArea.X + ((baseX + x) - XMove) / 16; chunknewY = world.WorldArea.Y + ((baseY + y) - YMove) / 16;
                                        newaddress = world.Chunks.First(c => c.Value == (new Point(chunknewX, chunknewY))).Key;
                                        newbaseX = (world.Chunks[newaddress].X - world.WorldArea.X) * 16; newbaseY = (world.Chunks[newaddress].Y - world.WorldArea.Y) * 16;
                                        x1 = ((baseX + x) - XMove) - newbaseX; y1 = ((baseY + y) - YMove) - newbaseY;
                                        for (int z = 15; z >= 0; z--)
                                        {
                                            world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z];
                                            world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z + 4096];
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else if (manipulatorDelegate == Bedrock)
            {
                foreach (int address in world.Chunks.Keys)
                {
                    int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                for (int z = 15; z >= 0; z--)
                                {
                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 1;
                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = undo[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096];
                                }
                            }

                        }
                    }
                }
            }
            else if (manipulatorDelegate == Rotate)
            {
                int blocknum;
                if (Form1.rotation == "Right 90" || Form1.rotation == "Left 90")
                {
                    foreach (int address in world.Chunks.Keys)
                    {
                        if (world.WorldArea.X + (Xr1 / 16) <= world.Chunks[address].X && world.WorldArea.X + (Xr2 / 16) >= world.Chunks[address].X)
                        {
                            int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                            for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                            {
                                for (int x = 0; x < 16; x++)
                                {
                                    for (int y = 0; y < 16; y++)
                                    {
                                        if ((baseY + y) >= Yr1 && (baseY + y) <= Yr2 && (baseX + x) >= Xr1 && (baseX + x) <= Xr2 && Form1.rotation == "Right 90")
                                        {
                                            chunknewX = world.WorldArea.X + (X1 + ((y + baseY) - Yr1)) / 16; chunknewY = world.WorldArea.Y + (Y1 + (Xr2 - (x + baseX))) / 16;
                                            newaddress = world.Chunks.First(c => c.Value == (new Point(chunknewX, chunknewY))).Key;
                                            newbaseX = (world.Chunks[newaddress].X - world.WorldArea.X) * 16; newbaseY = (world.Chunks[newaddress].Y - world.WorldArea.Y) * 16;
                                            x1 = (X1 + ((y + baseY) - Yr1)) - newbaseX; y1 = (Y1 + (Xr2 - (x + baseX))) - newbaseY;
                                            for (int z = 15; z >= 0; z--)
                                            {
                                                if ((undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 24 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 55) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 66 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 69) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 75 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 78))
                                                {
                                                    if (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 27 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 31 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 35 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 39 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 43 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 47 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 51 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 55 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 69 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 78)
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] - 3;
                                                    }
                                                    else
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] + 1;
                                                    }
                                                }
                                                else
                                                {
                                                    blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z];
                                                }
                                                if ((baseHeight * 16) + z >= Z1 && (baseHeight * 16) + z <= Z2)
                                                {
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = (byte)blocknum;
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z + 4096];
                                                }
                                            }
                                        }
                                        else if ((baseY + y) >= Yr1 && (baseY + y) <= Yr2 && (baseX + x) >= Xr1 && (baseX + x) <= Xr2 && Form1.rotation == "Left 90")
                                        {
                                            chunknewX = world.WorldArea.X + (X1 + (Yr2 - (y + baseY))) / 16; chunknewY = world.WorldArea.Y + (Y1 + ((x + baseX) - Xr1)) / 16;
                                            newaddress = world.Chunks.First(c => c.Value == (new Point(chunknewX, chunknewY))).Key;
                                            newbaseX = (world.Chunks[newaddress].X - world.WorldArea.X) * 16; newbaseY = (world.Chunks[newaddress].Y - world.WorldArea.Y) * 16;
                                            x1 = (X1 + (Yr2 - (y + baseY))) - newbaseX; y1 = (Y1 + ((x + baseX) - Xr1)) - newbaseY;
                                            for (int z = 15; z >= 0; z--)
                                            {
                                                if ((undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 24 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 55) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 66 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 69) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 75 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 78))
                                                {
                                                    if (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 24 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 28 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 32 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 36 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 40 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 44 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 48 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 52 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 66 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 75)
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] + 3;
                                                    }
                                                    else
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] - 1;
                                                    }
                                                }
                                                else
                                                {
                                                    blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z];
                                                }
                                                if ((baseHeight * 16) + z >= Z1 && (baseHeight * 16) + z <= Z2)
                                                {
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = (byte)blocknum;
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z + 4096];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (int address in world.Chunks.Keys)
                    {
                        if (world.WorldArea.X + ((X1 + XMove) / 16) <= world.Chunks[address].X && world.WorldArea.X + ((X2 + XMove) / 16) >= world.Chunks[address].X)
                        {
                            int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                            for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                            {
                                for (int x = 0; x < 16; x++)
                                {
                                    for (int y = 0; y < 16; y++)
                                    {
                                        if ((baseY + y) >= Y1 + YMove && (baseY + y) <= Y2 + YMove && (baseX + x) >= X1 + XMove && (baseX + x) <= X2 + XMove)
                                        {
                                            chunknewX = world.WorldArea.X + (X1 + (X2 - ((baseX + x) - XMove))) / 16; chunknewY = world.WorldArea.Y + (Y1 + (Y2 - ((baseY + y) - YMove))) / 16;
                                            newaddress = world.Chunks.First(c => c.Value == (new Point(chunknewX, chunknewY))).Key;
                                            newbaseX = (world.Chunks[newaddress].X - world.WorldArea.X) * 16; newbaseY = (world.Chunks[newaddress].Y - world.WorldArea.Y) * 16;
                                            x1 = (X1 + (X2 - ((baseX + x) - XMove))) - newbaseX; y1 = (Y1 + (Y2 - ((baseY + y) - YMove))) - newbaseY;
                                            for (int z = 15; z >= 0; z--)
                                            {
                                                if ((undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 24 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 55) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 66 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 69) || (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] >= 75 && undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] <= 78))
                                                {
                                                    if (undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 24 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 28 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 32 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 36 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 40 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 44 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 48 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 52 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 25 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 29 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 33 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 37 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 41 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 45 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 49 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 53 ||
                                                        undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 66 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 67 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 75 || undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] == 76)
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] + 2;
                                                    }
                                                    else
                                                    {
                                                        blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z] - 2;
                                                    }
                                                }
                                                else
                                                {
                                                    blocknum = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z];
                                                }
                                                if ((baseHeight * 16) + z >= Z1 && (baseHeight * 16) + z <= Z2)
                                                {
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = (byte)blocknum;
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = undo[newaddress + baseHeight * 8192 + x1 * 256 + y1 * 16 + z + 4096];
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            else if (manipulatorDelegate == LowerWorld)
            {
                foreach (int address in world.Chunks.Keys)
                {
                    int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                    for (int x = 0; x < 16; x++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            if ((baseX + x) >= Xmin && (baseX + x) <= Xmax && (baseY + y) >= Ymin && (baseY + y) <= Ymax)
                            {
                                if (Z1 != 0)
                                {
                                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                                    {
                                        for (int z = 0; z < 16; z++)
                                        {
                                            if ((baseHeight * 16) + z < 64 - Z1)
                                            {
                                                newHeight = (((baseHeight * 16) + z) + Z1) / 16;
                                                newZ = (z + Z1) % 16;
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + newHeight * 8192 + x * 256 + y * 16 + newZ];
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = world.Bytes[address + newHeight * 8192 + x * 256 + y * 16 + newZ + 4096];
                                            }
                                            else
                                            {
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 0;
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                            }
                                        }
                                    }
                                }
                                else if (Z2 != 0)
                                {
                                    for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                                    {
                                        for (int z = 15; z >= 0; z--)
                                        {
                                            if ((baseHeight * 16) + z > Z2)
                                            {
                                                newHeight = (((baseHeight * 16) + z) - Z2) / 16;
                                                newZ = ((64 + z) - Z2) % 16;
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + newHeight * 8192 + x * 256 + y * 16 + newZ];
                                                world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = world.Bytes[address + newHeight * 8192 + x * 256 + y * 16 + newZ + 4096];
                                            }
                                            else
                                            {
                                                if ((baseHeight * 16) + z == 0)
                                                {
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 1;
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                                }
                                                else
                                                {
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 2;
                                                    world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (manipulatorDelegate == RandomTrees)
            {
                List<Point> placed = new List<Point>();
                bool placing = true;
                foreach (int address in world.Chunks.Keys)
                {
                    int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                for (int z = 15; z >= 0; z--)
                                {
                                    for (int i = 0; i < length; i += 4)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 8 && (baseX + x) > xYxY[i] && (baseX + x) < xYxY[i + 1] && (baseY + y) > xYxY[i + 2] && (baseY + y) < xYxY[i + 3])
                                        {
                                            newHeight = (((baseHeight * 16) + z) + 1) / 16;
                                            if (world.Bytes[address + newHeight * 8192 + x * 256 + y * 16 + z + 1] == 0/* && x > 2 && x < 13 && y > 2 && y < 13*/)
                                            {
                                                for (int j = 0; j < placed.Count; j ++)
                                                {
                                                    int d = (int)Math.Sqrt(Math.Pow((baseX + x) - placed[j].X, 2) + Math.Pow((baseY + y) - placed[j].Y, 2));
                                                    if (d <= 5)
                                                    {
                                                        placing = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        placing = true;
                                                    }
                                                }
                                                if (smallpine && rnd.Next(25 * Density) == 0 && placing == true)
                                                {
                                                    Tree.PineTree(x, y, z, rnd, address, baseHeight, world, baseX, baseY);
                                                    placed.Add(new Point(baseX + x, baseY + y));
                                                }
                                                else if (largepine && rnd.Next(25 * Density) == 0 && placing == true)
                                                {
                                                    Tree.TallPineTree(x, y, z, rnd, address, baseHeight, world, baseX, baseY);
                                                    placed.Add(new Point(baseX + x, baseY + y));
                                                }
                                                else if(normalterrain && rnd.Next(25 * Density) == 0 && placing == true)
                                                {
                                                    Tree.NormalTerrainTree(x, y, z, rnd, address, baseHeight, world, baseX, baseY);
                                                    placed.Add(new Point(baseX + x, baseY + y));
                                                }
                                                else if(normal && rnd.Next(25 * Density) == 0 && placing == true)
                                                {
                                                    Tree.NormalTree(x, y, z, rnd, address, baseHeight, world, baseX, baseY);
                                                    placed.Add(new Point(baseX + x, baseY + y));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (int address in world.Chunks.Keys)
                {
                    int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                if ((baseX + x) >= Xmin && (baseX + x) <= Xmax && (baseY + y) >= Ymin && (baseY + y) <= Ymax)
                                {
                                    for (int z = 15; z >= 0; z--)
                                    {
                                        block = GetBlockAtPosition(x, y, z, address, baseX, baseY, baseHeight, world);
                                        block = manipulatorDelegate(block, address, baseX, baseY, baseHeight);
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] = (byte)block.BlockType;
                                        world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = (byte)block.Painting;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static Block GetBlockAtPosition(int x, int y, int z, int address, int baseX, int baseY, int baseHeight, World world)
        {
            return new Block((BlockType)world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z], (Painting)world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096], baseX + x, baseY + y, baseHeight * 16 + z);
        }

        static Painting GetPaintingAtPosition(int x, int y, int z, int address, int baseHeight, World world)
        {
            return (Painting)world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096];
        }

        static BlockType GetBlockTypeAtPosition(int x, int y, int z, int address, int baseHeight, World world)
        {
            return (BlockType)world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z];
        }

        public static void AddChunks(World world)
        {
            int xMin = world.Chunks.Values.Min(p => p.X), xMax = world.Chunks.Values.Max(p => p.X);
            int yMin = world.Chunks.Values.Min(p => p.Y), yMax = world.Chunks.Values.Max(p => p.Y);
            int maxaddress = world.Chunks.Keys.Max();
            int maxchunk = world.Chunks.Keys.Max();

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    if (!world.Chunks.ContainsValue(new Point(x, y)))
                    {
                        maxchunk += 32768;
                        world.Chunks.Add(maxchunk, new Point(x, y));
                    }
                }
            }
            int chunkPointerStartIndex = (world.Chunks.Count) * 32768 + 192;
            byte[] temp = new byte[chunkPointerStartIndex + world.Chunks.Count * 16];

            for (int i = 0; i < 32; i++)
            {
                temp[i] = world.Bytes[i];
            }

            temp[32] = ((byte)chunkPointerStartIndex);
            temp[33] = ((byte)(chunkPointerStartIndex >> 8));
            temp[34] = ((byte)(chunkPointerStartIndex >> 16));
            temp[35] = ((byte)(chunkPointerStartIndex >> 24));

            for (int i = 36; i < 192; i++)
            {
                temp[i] = world.Bytes[i];
            }

            foreach (int address in world.Chunks.Keys.OrderBy(k => k))
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            for (int z = 0; z < 16; z++)
                            {
                                if (address <= maxaddress)
                                {
                                    temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z];
                                    temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096];
                                }
                                else
                                {
                                    if (baseHeight * 16 + z == 0)
                                    {
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 1;
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                    }
                                    else if (baseHeight * 16 + z > 0 && baseHeight * 16 + z < 16)
                                    {
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 2;
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                    }
                                    else if (baseHeight * 16 + z >= 16 && baseHeight * 16 + z < 32)
                                    {
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 3;
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                    }
                                    else if (baseHeight * 16 + z == 32)
                                    {
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 8;
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                    }
                                    else
                                    {
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z] = 0;
                                        temp[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int ChunkPointer = 192;

            foreach (int adress in world.Chunks.Keys.OrderBy(k => k))
            {
                temp[chunkPointerStartIndex] = ((byte)world.Chunks[adress].X);
                temp[chunkPointerStartIndex + 1] = ((byte)(world.Chunks[adress].X >> 8));
                temp[chunkPointerStartIndex + 2] = ((byte)(world.Chunks[adress].X >> 16));
                temp[chunkPointerStartIndex + 3] = ((byte)(world.Chunks[adress].X >> 24));
                temp[chunkPointerStartIndex + 4] = ((byte)world.Chunks[adress].Y);
                temp[chunkPointerStartIndex + 5] = ((byte)(world.Chunks[adress].Y >> 8));
                temp[chunkPointerStartIndex + 6] = ((byte)(world.Chunks[adress].Y >> 16));
                temp[chunkPointerStartIndex + 7] = ((byte)(world.Chunks[adress].Y >> 24));
                temp[chunkPointerStartIndex + 8] = ((byte)ChunkPointer);
                temp[chunkPointerStartIndex + 9] = ((byte)(ChunkPointer >> 8));
                temp[chunkPointerStartIndex + 10] = ((byte)(ChunkPointer >> 16));
                temp[chunkPointerStartIndex + 11] = ((byte)(ChunkPointer >> 24));
                temp[chunkPointerStartIndex + 12] = (0 >> 24);
                temp[chunkPointerStartIndex + 13] = (0 >> 16);
                temp[chunkPointerStartIndex + 14] = (0 >> 8);
                temp[chunkPointerStartIndex + 15] = (0);
                ChunkPointer += 32768;
                chunkPointerStartIndex += 16;
            }
            Array.Resize(ref world.Bytes, temp.Length);
            world.Bytes = temp;
        }
    }
}
