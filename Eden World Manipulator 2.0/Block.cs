namespace Eden_World_Maniputor_2._0
{
    public class Block
    {
        public readonly BlockType BlockType;
        public readonly Painting Painting;
        public readonly int X, Y, Z;

        public Block(BlockType type, Painting painting, int x, int y, int z)
        {
            BlockType = type;
            Painting = painting;
            X = x;
            Y = y;
            Z = z;
        }

        public Block(byte? type, byte? painting, int x, int y, int z)
            : this((BlockType)type, (Painting)painting, x, y, z) { }

        public Block(BlockType type, Painting painting)
            : this(type, painting, -1, -1, -1) { }
    }
}
