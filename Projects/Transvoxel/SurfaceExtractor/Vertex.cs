using Transvoxel.Math;

namespace Transvoxel.SurfaceExtractor
{
    public struct Vertex
    {
        public Vector3f Primary;
        public Vector3f Secondary;
        public Vector3f Normal;
        public byte Near;
    }
}