/*
namespace TransvoxelXna.VolumeData
{
    public interface IVolumeData
    {
        sbyte this[int x, int y, int z] { get; set; }
        sbyte this[Vector3i v] { get; set; }
        sbyte[] Buffer { get; }
        int GetBufferSize();
    }


    public class VolumeData : IVolumeData
    {
        private readonly sbyte[] _samples;
        private Vector3i _offset;
        private Vector3i _size;
        public VolumeData(Vector3i size, Vector3i offset)
        {
            _size = size;
            _offset = offset;
            _samples = new sbyte[size.X * size.Y * size.Z];
        }
        public VolumeData(Vector3i size)
            : this(size, Vector3i.Zero)
        {
        }

        public sbyte this[int x, int y, int z]
        {
            get
            {
                if (x < 0 || x >= _size.X || y < 0 || y >= _size.Y || z < 0 || z >= _size.Z)
                    return -1;
                return _samples[(x - _offset.X) + (y - _offset.Y) * _size.X + (z - _offset.Z) * (_size.X * _size.Y)];
            }
            set
            {
                if (x < 0 || x >= _size.X || y < 0 || y >= _size.Y || z < 0 || z >= _size.Z)
                    return;
                _samples[(x - _offset.X) + (y - _offset.Y) * _size.X + (z - _offset.Z) * (_size.X * _size.Y)] = value;
            }
        }

        public sbyte this[Vector3i v]
        {
            get
            {
                if (v.X < 0 || v.X >= _size.X || v.Y < 0 || v.Y >= _size.Y || v.Z < 0 || v.Z >= _size.Z)
                    return -1;
                v = v - _offset;
                return _samples[v.X + v.Y * _size.X + v.Z * (_size.X * _size.Y)];
            }
            set
            {
                if (v.X < 0 || v.X >= _size.X || v.Y < 0 || v.Y >= _size.Y || v.Z < 0 || v.Z >= _size.Z)
                    return;
                v = v - _offset;
                _samples[v.X + v.Y * _size.X + v.Z * (_size.X * _size.Y)] = value;
            }
        }

        public sbyte[] Buffer { get { return _samples; } }

        public int GetBufferSize()
        {
            return _samples.Length;
        }
    }
}
*/