using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden_World_Maniputor_2._0
{
    public static partial class Manipulator
    {
        //Add your own manipulations into this Dictionary if you want them to appear in the program
        public static readonly Dictionary<string, Func<Block, int, int, int, int, Block>> Manipulations = new Dictionary<string, Func<Block, int, int, int, int, Block>>()
        {
            {"Generate Random Trees", RandomTrees},
            {"Create Ocean", OceanOutCreation},
            {"Lower or Raise", LowerWorld},

            {"Bedrock World", Bedrock},
            {"Move Area Horizontally", MoveWorld},
            {"Rotate and Move", Rotate},

            { "Winterize", Winterize},
            {"Block and Color Change", BlockColorChange},
            {"Block Change", BlockChange},

            {"Color Change", ColorChange},
            {"Remove Dirt,Stone,Grass", RemoveStuff},
            { "Add Land", EraseOcean},

            {"Erase", Erase},
        };


        static Random random = new Random();
        
        public static Painting PaintColor, PaintColor2;
        public static BlockType BlockChoice, BlockChoice2;
        public static List<int> xYxY = new List<int>();
        public static int X1, X2, Y1, Y2, Z1, Z2, Xmin, Xmax, Ymin, Ymax, XMove, YMove, Xr1, Xr2, Yr1, Yr2;
        static bool done = false;

        public static Block Erase(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // Erases anything but grass, dirt, stone, bedrock
            for (int i = 0; i < length; i +=4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && (block.BlockType == BlockType.Stone || block.BlockType == BlockType.Grass || block.BlockType == BlockType.Dirt || block.BlockType == BlockType.Bedrock) && block.Painting == Painting.Unpainted)
                {
                    return block;
                }
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3])
                {
                    return new Block(BlockType.Air, Painting.Unpainted);
                }

            }
            return block;
        }

        public static Block RandomTrees(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            return block;
        }

        public static Block OceanOutCreation(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // creates ocean of whole map or in rectangle
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z == Z1)
                {
                    xYxY.Count();
                    return new Block(BlockChoice, PaintColor);
                }
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z <= Z2 && block.Z > Z1)
                {
                    return new Block(BlockType.Water, PaintColor2);
                }
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z > Z2)
                {
                    return new Block(BlockType.Air, Painting.Unpainted);
                }
            }
            return block;

        }

        public static Block LowerWorld(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            return block;
        }

        public static Block Bedrock(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            return block;

        }

        public static Block MoveWorld(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            return block;
        }

        public static Block Rotate(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            return block;
        }

        public static Block Winterize(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            // will make a winterized world
            return block;
        }

        public static Block BlockColorChange(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // change the X and Y to set the position, the block to what you want and the painting
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && ((block.Z >= Z1 && block.Z <= Z2) || (block.Z == Z1 && Z1 == Z2)) && (BlockType)block.BlockType == BlockChoice && block.Painting == PaintColor) return new Block(BlockChoice2, PaintColor2);
            }
            return block;
        }

        public static Block BlockChange(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // change the X and Y to set the position, the block to what you want and the painting
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && ((block.Z >= Z1 && block.Z <= Z2) || (block.Z == Z1 && Z1 == Z2)) && (BlockType)block.BlockType == BlockChoice) return new Block(BlockChoice2, block.Painting);
            }
            return block;
        }

        public static Block ColorChange(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // change the X and Y to set the position, the block to what you want and the painting
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && ((block.Z >= Z1 && block.Z <= Z2) || (block.Z == Z1 && Z1 == Z2)) && block.Painting == PaintColor) return new Block(block.BlockType, PaintColor2);
            }
            return block;
        }

        public static Block RemoveStuff(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // removes grass, stone, and weed blocks
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z > Z1 && block.Z < Z2 && block.Painting == Painting.Unpainted && ((BlockType)block.BlockType == BlockType.Grass || (BlockType)block.BlockType == BlockType.Dirt || (BlockType)block.BlockType == BlockType.Stone || (BlockType)block.BlockType == BlockType.Weeds || (BlockType)block.BlockType == BlockType.Flowers)) return new Block(BlockType.Air, Painting.Unpainted);
            }
            return block;

        }

        public static Block EraseOcean(Block block, int address, int baseX, int baseY, int baseHeight)
        {
            int length = xYxY.Count;
            // Erases anything and returns flat land
            for (int i = 0; i < length; i += 4)
            {
                if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z == 0 && block.Z >= Z1 && block.Z <= Z2) return new Block(BlockType.Bedrock, Painting.Unpainted);
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z <= 15 && block.Z >= Z1 && block.Z <= Z2) return new Block(BlockType.Stone, Painting.Unpainted);
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z <= 31 && block.Z >= Z1 && block.Z <= Z2) return new Block(BlockType.Dirt, Painting.Unpainted);
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z == 32 && block.Z >= Z1 && block.Z <= Z2) return new Block(BlockType.Grass, Painting.Unpainted);
                else if (block.X >= xYxY[i] && block.X <= xYxY[i + 1] && block.Y >= xYxY[i + 2] && block.Y <= xYxY[i + 3] && block.Z > 32 && block.Z >= Z1 && block.Z <= Z2) return new Block(BlockType.Air, Painting.Unpainted);

            }
            return block;
        }
    }
}
