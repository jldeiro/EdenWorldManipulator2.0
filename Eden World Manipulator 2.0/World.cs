using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Eden_World_Maniputor_2._0
{
    public class World
    {
        public string Name;
        public Dictionary<int, Point> Chunks;
        public Rectangle WorldArea = default(Rectangle);
        public static int skyColor;
        public static string worldName;
        public byte[] Bytes;

        public static World LoadWorld(string path)
        {
            List<int> skyColors = new List<int>();
            byte[] bytes;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
            }

            //Get Sky Color
            for (int i = 132; i <= 148; i++)
            {
                if (bytes[i] != 14) skyColors.Add(bytes[i]);
            }

            if (skyColors.Count == 0) skyColor = 14;
            skyColor = skyColors.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;

            int chunkPointerStartIndex = bytes[35] * 256 * 256 * 256 + bytes[34] * 256 * 256 + bytes[33] * 256 + bytes[32];

            byte[] nameArray = bytes.TakeWhile((b, i) => ((i < 40 || b != 0) && i <= 75)).ToArray();
            worldName = Encoding.ASCII.GetString(nameArray, 40, nameArray.Length - 40);
            Rectangle worldArea = Rectangle.Empty;
            Dictionary<int, Point> chunks = new Dictionary<int, Point>();

            //Create array of chunk points and addresses
            int currentChunkPointerIndex = chunkPointerStartIndex;
            do
            {
                chunks.Add(
                    bytes[currentChunkPointerIndex + 11] * 256 * 256 * 256 + bytes[currentChunkPointerIndex + 10] * 256 * 256 + bytes[currentChunkPointerIndex + 9] * 256 + bytes[currentChunkPointerIndex + 8],//address
                    new Point(bytes[currentChunkPointerIndex + 1] * 256 + bytes[currentChunkPointerIndex], bytes[currentChunkPointerIndex + 5] * 256 + bytes[currentChunkPointerIndex + 4])); //Position
            } while ((currentChunkPointerIndex += 16) < bytes.Length);

            //Get max dimensions of world
            worldArea.X = chunks.Values.Min(p => p.X);
            worldArea.Y = chunks.Values.Min(p => p.Y);
            worldArea.Width = chunks.Values.Max(p => p.X) - worldArea.X + 1;
            worldArea.Height = chunks.Values.Max(p => p.Y) - worldArea.Y + 1;

            return new World(bytes, chunks, worldName, worldArea);
        }

        public static World LoadDownload(byte[] down)
        {
            byte[] bytes = down;

            List<int> skyColors = new List<int>();

            //Get Sky Color
            for (int i = 132; i <= 148; i++)
            {
                if (bytes[i] != 14) skyColors.Add(bytes[i]);
            }

            if (skyColors.Count == 0) skyColor = 14;
            skyColor = skyColors.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;

            int chunkPointerStartIndex = bytes[35] * 256 * 256 * 256 + bytes[34] * 256 * 256 + bytes[33] * 256 + bytes[32];

            byte[] nameArray = bytes.TakeWhile((b, i) => ((i < 40 || b != 0) && i <= 75)).ToArray();
            worldName = Encoding.ASCII.GetString(nameArray, 40, nameArray.Length - 40);
            Rectangle worldArea = Rectangle.Empty;
            Dictionary<int, Point> chunks = new Dictionary<int, Point>();

            //Create array of chunk points and addresses
            int currentChunkPointerIndex = chunkPointerStartIndex;
            do
            {
                chunks.Add(
                    bytes[currentChunkPointerIndex + 11] * 256 * 256 * 256 + bytes[currentChunkPointerIndex + 10] * 256 * 256 + bytes[currentChunkPointerIndex + 9] * 256 + bytes[currentChunkPointerIndex + 8],//address
                    new Point(bytes[currentChunkPointerIndex + 1] * 256 + bytes[currentChunkPointerIndex], bytes[currentChunkPointerIndex + 5] * 256 + bytes[currentChunkPointerIndex + 4])); //Position
            } while ((currentChunkPointerIndex += 16) < bytes.Length);

            //Get max dimensions of world
            worldArea.X = chunks.Values.Min(p => p.X);
            worldArea.Y = chunks.Values.Min(p => p.Y);
            worldArea.Width = chunks.Values.Max(p => p.X) - worldArea.X + 1;
            worldArea.Height = chunks.Values.Max(p => p.Y) - worldArea.Y + 1;

            return new World(bytes, chunks, worldName, worldArea);
        }

        public void SaveWorld(string path, World world)
        {
            using (FileStream stream = new FileStream(path, FileMode.CreateNew))
            {
                //Save File with changed world name or original world name
                byte[] name = Encoding.ASCII.GetBytes(worldName);
                for (int i = 0; i < world.Bytes.Length; i++)
                {
                    if (i >= 40 && i <= (75))
                    {
                        if (i - 40 < name.Length)
                        {
                            stream.WriteByte(name[i - 40]);
                        }
                        else
                        {
                            stream.WriteByte(0);
                        }

                    }
                    else
                    {
                        stream.WriteByte(world.Bytes[i]);
                    }

                }
            }
        }

        private World(byte[] bytes, Dictionary<int, Point> chunks, string name, Rectangle worldArea)
        {
            Bytes = bytes;
            Chunks = chunks;
            Name = name;
            WorldArea = worldArea;
        }
    }
}
