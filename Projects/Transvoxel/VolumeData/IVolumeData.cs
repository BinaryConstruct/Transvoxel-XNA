using Microsoft.Xna.Framework;
using Transvoxel.Math;
using Transvoxel.VolumeData.VolumeHash;

namespace Transvoxel.VolumeData
{
    public interface IVolumeData<T>
    {
        T this[int x, int y, int z] { get; set; }
        T this[Vector3i v] { get; set; }
        T this[Vector3 v] { get; set; }
        VolumeSize Size { get; }
    }
}