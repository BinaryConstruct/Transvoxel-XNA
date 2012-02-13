using Transvoxel.Math;

namespace Transvoxel.VolumeData
{
    public interface IVolumeData
    {
        sbyte this[int x, int y, int z] { get; set; }
        sbyte this[Vector3i v] { get; set; }
        int ChunkSize { get; }
    }
}