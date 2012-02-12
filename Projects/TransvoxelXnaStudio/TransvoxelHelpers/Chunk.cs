using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public class Chunk
    {
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        public Vector3 Position;
        public BoundingBox BoundingBox;
    }
}