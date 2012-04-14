using Transvoxel.Math;

namespace Transvoxel.VolumeData
{
    public abstract class IVolumeData<TYPE>
    {
        public abstract TYPE this[int x, int y, int z] { get; set; }
        public TYPE this[Vector3i v] 
        { 
            get
            {
                return this[v.X,v.Y,v.Z];
            } 
            
            set
            {
                this[v.X, v.Y, v.Z] = value;
            }
        }

        public abstract int ChunkSize { get; set; }
    }
}