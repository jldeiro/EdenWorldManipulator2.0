using System.Drawing;

namespace Eden_World_Maniputor_2._0
{
    class Mapping
    {
        public static int block = 0;
        public static bool painted = false;
        public static int WairX = 0, WairY = 0, WairZ = 0, tempaddress = 0, tempBaseHeight = 0, transpPixel = 0;

        public static Color Mix(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        public static Bitmap NormalMap(World world, Bitmap m_Canvas)
        {
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++, painted = false)
                    {
                        for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                        {
                            for (int z = 15; z >= 0; z--)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0) //Block
                                {
                                    block++;
                                    if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0 && painted == false) //Color
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                        m_Canvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[pen]);
                                        painted = true;
                                    }
                                    else if (painted == false)
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                        m_Canvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[pen]);
                                        painted = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return m_Canvas;
        }

        public static Bitmap ZSlice(World world, Bitmap newCanvas, int cut)
        {
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++, painted = false)
                    {
                        for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                        {
                            for (int z = 15; z >= 0; z--)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0 && ((baseHeight * 16) + z) == cut) //Block
                                {
                                    if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0 && painted == false) //Color
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                        newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[pen]);
                                        painted = true;
                                        break;
                                    }
                                    else if (painted == false)
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                        newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[pen]);
                                        painted = true;
                                        break;
                                    }
                                }
                            }
                            if (painted)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return newCanvas;
        }

        public static Bitmap BlendNormalMap(World world, Bitmap newCanvas, Bitmap  tempCanvas)
        {
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++, painted = false)
                    {
                        for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                        {
                            for (int z = 15; z >= 0; z--)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0) //Block
                                {
                                    if (transpPixel != 0 && world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 20 && world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 58)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0)
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[pen]);
                                            if ((world.Bytes[address + tempBaseHeight * 8192 + x * 256 + y * 16 + WairZ] == 58)) newCanvas.SetPixel(baseX + x, baseY + y, Mix(tempCanvas.GetPixel(baseX + x, baseY + y), newCanvas.GetPixel(baseX + x, baseY + y), .25));
                                            else newCanvas.SetPixel(baseX + x, baseY + y, Mix(tempCanvas.GetPixel(baseX + x, baseY + y), newCanvas.GetPixel(baseX + x, baseY + y), .65));
                                            painted = true;
                                            transpPixel = 0;
                                            break;
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[pen]);
                                            if ((world.Bytes[address + tempBaseHeight * 8192 + x * 256 + y * 16 + WairZ] == 58)) newCanvas.SetPixel(baseX + x, baseY + y, Mix(tempCanvas.GetPixel(baseX + x, baseY + y), newCanvas.GetPixel(baseX + x, baseY + y), .25));
                                            else newCanvas.SetPixel(baseX + x, baseY + y, Mix(tempCanvas.GetPixel(baseX + x, baseY + y), newCanvas.GetPixel(baseX + x, baseY + y), .65));
                                            transpPixel = 0;
                                            painted = true;
                                            break;
                                        }
                                    }
                                    if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0 && transpPixel == 0) //Color
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 58 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 20 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 59 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 60 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 61)
                                        {
                                            transpPixel += 1;
                                            int transp = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            WairZ = z; tempBaseHeight = baseHeight;
                                            tempCanvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[transp]);
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[pen]);
                                            painted = true;
                                            break;
                                        }
                                    }
                                    else if (transpPixel == 0)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 58 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 20 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 59 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 60 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 61)
                                        {
                                            transpPixel += 1;
                                            int transp = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            WairZ = z; tempBaseHeight = baseHeight;
                                            tempCanvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[transp]);
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[pen]);
                                            painted = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (painted)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return newCanvas;
        }

        public static Bitmap Treasure(World world, Bitmap newCanvas)
        {
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++, painted = false)
                    {
                        for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                        {
                            for (int z = 15; z >= 0; z--)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0) //Block
                                {
                                    if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0 && painted == false)
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                        newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Painted[pen]);
                                        painted = true;
                                    }
                                    else if (painted == false)
                                    {
                                        int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                        newCanvas.SetPixel(baseX + x, baseY + y, MapColors.Unpainted[pen]);
                                        painted = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var brush = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++, painted = false)
                    {
                        for (int baseHeight = 3; baseHeight >= 0; baseHeight--)
                        {
                            for (int z = 15; z >= 0; z--)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 71)
                                {
                                    using (var g = Graphics.FromImage(newCanvas))
                                    {
                                        g.FillEllipse(brush, baseX + x, baseY + y, 10, 10);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return newCanvas;
        }

        // Doesn't work anymore
        public static Bitmap YSlice(World world, Bitmap newCanvas, Bitmap tempCanvas, int cut)
        {
            foreach (int address in world.Chunks.Keys)
            {
                int baseX = (world.Chunks[address].X - world.WorldArea.X) * 16, baseY = (world.Chunks[address].Y - world.WorldArea.Y) * 16;
                for (int x = 0; x < 16; x++)
                {
                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 0) //Block
                                {
                                    if (transpPixel != 0 && world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 20 && world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] != 58)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0) //Color
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Painted[pen]);
                                            if (world.Bytes[tempaddress + tempBaseHeight * 8192 + WairX * 256 + WairY * 16 + WairZ] == 20) newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, Mix(tempCanvas.GetPixel(baseX + x, baseHeight * 16 + z), newCanvas.GetPixel(baseX + x, baseHeight * 16 + z), .5));
                                            else newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, Mix(tempCanvas.GetPixel(baseX + x, baseHeight * 16 + z), newCanvas.GetPixel(baseX + x, baseHeight * 16 + z), .25));
                                            transpPixel = 0;
                                            break;
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Unpainted[pen]);
                                            if (world.Bytes[tempaddress + tempBaseHeight * 8192 + WairX * 256 + WairY * 16 + WairZ] == 20) newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, Mix(tempCanvas.GetPixel(baseX + x, baseHeight * 16 + z), newCanvas.GetPixel(baseX + x, baseHeight * 16 + z), .5));
                                            else newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, Mix(tempCanvas.GetPixel(baseX + x, baseHeight * 16 + z), newCanvas.GetPixel(baseX + x, baseHeight * 16 + z), .25));
                                            transpPixel = 0;
                                            break;
                                        }
                                    }
                                    if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] != 0 && transpPixel == 0 && baseY + y == cut)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 58 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 20)
                                        {
                                            transpPixel += 1;
                                            int transp = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            WairX = x; WairY = y; WairZ = z; tempaddress = address; tempBaseHeight = baseHeight;
                                            tempCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Painted[transp]);
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z + 4096] - 1;
                                            newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Painted[pen]);
                                            break;
                                        }
                                    }
                                    else if (transpPixel == 0 && baseY + y == cut)
                                    {
                                        if (world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 58 || world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] == 20)
                                        {
                                            transpPixel += 1;
                                            int transp = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            WairX = x; WairY = y; WairZ = z; tempaddress = address; tempBaseHeight = baseHeight;
                                            tempCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Unpainted[transp]);
                                        }
                                        else
                                        {
                                            int pen = world.Bytes[address + baseHeight * 8192 + x * 256 + y * 16 + z] - 1;
                                            newCanvas.SetPixel(baseX + x, baseHeight * 16 + z, MapColors.Unpainted[pen]);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return newCanvas;

            /*for (int x = 0; x < world.Map.GetLength(0); x++)
            {
                for (int z = world.Map.GetLength(2) - 1; z >= 0; z--)
                {
                    for (int y = cut; y < world.Map.GetLength(1); y++)
                    {
                        if (world.Map[x, y, z, 1] != null)
                        {
                            if (world.Map[x, y, z, 0] != 0)
                            {
                                if (transpPixel != 0 && world.Map[x, y, z, 0] != 20 && world.Map[x, y, z, 0] != 58)
                                {
                                    if (world.Map[x, y, z, 1] != 0)
                                    {
                                        int pen = ((int)world.Map[x, y, z, 1]) - 1;
                                        newCanvas.SetPixel(x, z, MapColors.Painted[pen]);
                                        if (world.Map[x, WairZ, z, 0] == 20) newCanvas.SetPixel(x, z, Mix(tempCanvas.GetPixel(x, z), newCanvas.GetPixel(x, z), .5));
                                        else newCanvas.SetPixel(x, z, Mix(tempCanvas.GetPixel(x, z), newCanvas.GetPixel(x, z), .25));
                                        transpPixel = 0;
                                        break;
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 0]) - 1;
                                        newCanvas.SetPixel(x, z, MapColors.Unpainted[pen]);
                                        if (world.Map[x, WairZ, z, 0] == 20) newCanvas.SetPixel(x, z, Mix(tempCanvas.GetPixel(x, z), newCanvas.GetPixel(x, z), .5));
                                        else newCanvas.SetPixel(x, z, Mix(tempCanvas.GetPixel(x, z), newCanvas.GetPixel(x, z), .25));
                                        transpPixel = 0;
                                        break;
                                    }
                                }
                                if (world.Map[x, y, z, 1] != 0 && transpPixel == 0)
                                {
                                    if ((int)world.Map[x, y, z, 0] == 58 || (int)world.Map[x, y, z, 0] == 20)
                                    {
                                        transpPixel += 1;
                                        int transp = ((int)world.Map[x, y, z, 1]) - 1;
                                        WairZ = y;
                                        tempCanvas.SetPixel(x, z, MapColors.Painted[transp]);
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 1]) - 1;
                                        newCanvas.SetPixel(x, z, MapColors.Painted[pen]);
                                        break;
                                    }
                                }
                                else if (transpPixel == 0)
                                {
                                    if ((int)world.Map[x, y, z, 0] == 58 || (int)world.Map[x, y, z, 0] == 20)
                                    {
                                        transpPixel += 1;
                                        int transp = ((int)world.Map[x, y, z, 0]) - 1;
                                        WairZ = y;
                                        tempCanvas.SetPixel(x, z, MapColors.Unpainted[transp]);
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 0]) - 1;
                                        newCanvas.SetPixel(x, z, MapColors.Unpainted[pen]);
                                        break;
                                    }
                                }
                            }
                            else if (y == world.Map.GetLength(1) - 1 && world.Map[x, y, z, 0] == 0)
                            {
                                int pen = (World.skyColor - 1);
                                if (World.skyColor == 14)
                                {
                                    newCanvas.SetPixel(x, z, Color.LightSkyBlue);
                                }
                                else newCanvas.SetPixel(x, z, MapColors.Painted[pen]);
                                break;
                            }
                        }
                        else if (y == world.Map.GetLength(1) - 1 && world.Map[x, y, z, 1] == null)
                        {
                            int pen = (World.skyColor - 1);
                            if (World.skyColor == 14)
                            {
                                newCanvas.SetPixel(x, z, Color.LightSkyBlue);
                            }
                            else newCanvas.SetPixel(x, z, MapColors.Painted[pen]);
                            break;
                        }
                    }
                }
            }*/
        }

        // Doesn't Work, Not even updated
        public static Bitmap XSlice(World world, Bitmap newCanvas, Bitmap tempCanvas, int cut)
        {
            return newCanvas;
            /*for (int y = 0; y < world.Map.GetLength(1); y++)
            {
                for (int z = world.Map.GetLength(2) - 1; z >= 0; z--)
                {
                    for (int x = cut; x < world.Map.GetLength(0); x++)
                    {
                        if (world.Map[x, y, z, 1] != null)
                        {
                            if (world.Map[x, y, z, 0] != 0)
                            {
                                if (transpPixel != 0 && world.Map[x, y, z, 0] != 20 && world.Map[x, y, z, 0] != 58)
                                {
                                    if (world.Map[x, y, z, 1] != 0)
                                    {
                                        int pen = ((int)world.Map[x, y, z, 1]) - 1;
                                        newCanvas.SetPixel(y, z, MapColors.Painted[pen]);
                                        if (world.Map[WairZ, y, z, 0] == 20) newCanvas.SetPixel(y, z, Mix(tempCanvas.GetPixel(y, z), newCanvas.GetPixel(y, z), .5));
                                        else newCanvas.SetPixel(y, z, Mix(tempCanvas.GetPixel(y, z), newCanvas.GetPixel(y, z), .25));
                                        transpPixel = 0;
                                        break;
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 0]) - 1;
                                        newCanvas.SetPixel(y, z, MapColors.Unpainted[pen]);
                                        if (world.Map[WairZ, y, z, 0] == 20) newCanvas.SetPixel(y, z, Mix(tempCanvas.GetPixel(y, z), newCanvas.GetPixel(y, z), .5));
                                        else newCanvas.SetPixel(y, z, Mix(tempCanvas.GetPixel(y, z), newCanvas.GetPixel(y, z), .25));
                                        transpPixel = 0;
                                        break;
                                    }
                                }
                                if (world.Map[x, y, z, 1] != 0 && transpPixel == 0)
                                {
                                    if ((int)world.Map[x, y, z, 0] == 58 || (int)world.Map[x, y, z, 0] == 20)
                                    {
                                        transpPixel += 1;
                                        int transp = ((int)world.Map[x, y, z, 1]) - 1;
                                        WairZ = x;
                                        tempCanvas.SetPixel(y, z, MapColors.Painted[transp]);
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 1]) - 1;
                                        newCanvas.SetPixel(y, z, MapColors.Painted[pen]);
                                        break;
                                    }
                                }
                                else if (transpPixel == 0)
                                {
                                    if ((int)world.Map[x, y, z, 0] == 58 || (int)world.Map[x, y, z, 0] == 20)
                                    {
                                        transpPixel += 1;
                                        int transp = ((int)world.Map[x, y, z, 0]) - 1;
                                        WairZ = x;
                                        tempCanvas.SetPixel(y, z, MapColors.Unpainted[transp]);
                                    }
                                    else
                                    {
                                        int pen = ((int)world.Map[x, y, z, 0]) - 1;
                                        newCanvas.SetPixel(y, z, MapColors.Unpainted[pen]);
                                        break;
                                    }
                                }
                            }
                            else if (x == world.Map.GetLength(0) - 1 && world.Map[x, y, z, 0] == 0)
                            {
                                int pen = (World.skyColor - 1);
                                if (World.skyColor == 14)
                                {
                                    newCanvas.SetPixel(y, z, Color.LightSkyBlue);
                                }
                                else newCanvas.SetPixel(y, z, MapColors.Painted[pen]);
                                break;
                            }
                        }
                        else if (x == world.Map.GetLength(0) - 1 && world.Map[x, y, z, 1] == null)
                        {
                            int pen = (World.skyColor - 1);
                            if (World.skyColor == 14)
                            {
                                newCanvas.SetPixel(y, z, Color.LightSkyBlue);
                            }
                            else newCanvas.SetPixel(y, z, MapColors.Painted[pen]);
                            break;
                        }
                    }
                }
            }
            newCanvas.RotateFlip(RotateFlipType.RotateNoneFlipX);*/
        }
    }
}
