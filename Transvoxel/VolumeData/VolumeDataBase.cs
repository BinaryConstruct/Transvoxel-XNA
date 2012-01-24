using TransvoxelXna.helper;

namespace TransvoxelXna.VolumeData
{
    public interface IVolumeData
    {
        sbyte this[int x, int y, int z] { get; set; }
        sbyte this[Vector3i v] { get; set; }
    }

    public abstract class VolumeDataBase : IVolumeData
    {
        public abstract sbyte this[int x, int y, int z] { get; set; }

        public sbyte this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }
    }
}
