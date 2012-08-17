using System;
using System.Collections.Generic;
using System.Diagnostics;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.Dictionary
{
    public interface IVolume
    {
        sbyte this[int x, int y, int z] { get; set; }
        void Wipe(sbyte value);
        bool Compact();
    }

    /// <summary>
    /// Contains a volume size calculations.
    /// </summary>
    public sealed class VolumeSize
    {
        /// <summary>
        /// The axis unit size.
        /// </summary>
        public readonly int SideLength = 8;

        /// <summary>
        /// The axis unit size^2.
        /// </summary>
        public readonly int SideLengthSquared = 64;

        /// <summary>
        /// The axis unit size^3.
        /// </summary>
        public readonly int SideLengthCubed = 512;

        /// <summary>
        /// Creates a new instance of VolumeSize.
        /// </summary>
        /// <param name="sideLength">The axis unit size of the volume. Must be a power of 2 and between 8 and 256.</param>
        public VolumeSize(int sideLength)
        {
            if (sideLength % 2 != 0)
                throw new ArgumentOutOfRangeException("sideLength", "Not a power of 2.");
            if (sideLength < 8)
                throw new ArgumentOutOfRangeException("sideLength", "Must be between 8 and 256.");
            if (sideLength > 256)
                throw new ArgumentOutOfRangeException("sideLength", "Must be between 8 and 256.");

            SideLength = sideLength;
            SideLengthSquared = sideLength * sideLength;
            SideLengthCubed = SideLength * SideLength * SideLength;
        }
    }

    public class VolumePage : IVolumeData
    {
        private readonly VolumeSize _size;
        private readonly Dictionary<Vector3f, VolumeChunk> _data;

        public VolumePage(VolumeSize size)
        {
            _size = size;
            _data = new Dictionary<Vector3f, VolumeChunk>();
        }

        #region Implementation of IVolume

        public sbyte this[int x, int y, int z]
        {
            get
            {
                int xI = x / (_size.SideLength * 2);
                int yI = y / (_size.SideLength * 2);
                int zI = z / (_size.SideLength * 2);
                if (x < 0) xI--;
                if (y < 0) yI--;
                if (z < 0) zI--;
                var key = new Vector3f(xI, yI, zI);

                VolumeChunk current;
                if (!_data.ContainsKey(key))
                    return default(sbyte);

                current = _data[key];
                return current[x, y, z];
            }
            set
            {
                int xI = x / (_size.SideLength * 2);
                int yI = y / (_size.SideLength * 2);
                int zI = z / (_size.SideLength * 2);
                if (x < 0) xI--;
                if (y < 0) yI--;
                if (z < 0) zI--;
                var key = new Vector3f(xI, yI, zI);

                VolumeChunk current;
                if (!_data.ContainsKey(key))
                {
                    current = CreateChunk(key);
                    _data.Add(key, current);
                }
                else
                {
                    current = _data[key];
                }
                current[x, y, z] = value;
            }
        }

        sbyte IVolumeData.this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }

        public int ChunkSize
        {
            get { return _size.SideLength; }
            set{ }
        }
        public int ChunkBits { get { return 1; } set { } }

        private VolumeChunk CreateChunk(Vector3f key)
        {
            float xI = key.X * (_size.SideLength * 2);
            float yI = key.Y * (_size.SideLength * 2);
            float zI = key.Z * (_size.SideLength * 2);
            if (key.X < 0) xI--;
            if (key.Y < 0) yI--;
            if (key.Z < 0) zI--;

            return new VolumeChunk(new Vector3f(xI, yI, zI), _size);
        }

        public void Wipe(sbyte value)
        {
            throw new NotImplementedException();
        }

        public bool Compact()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class VolumeChunk : IVolume
    {
        private readonly VolumeSize _size;
        private VolumeLeaf[] _data;

        public readonly Vector3f Min;
        public readonly Vector3f Max;

        public VolumeChunk(Vector3f min, VolumeSize size)
        {
            _size = size;
            _data = new VolumeLeaf[8];
            Min = min;
            Max = new Vector3f(min.X + size.SideLength * 2, min.Y + size.SideLength * 2, min.Z + size.SideLength * 2);
        }

        #region Implementation of IVolume

        public sbyte this[int x, int y, int z]
        {
            get
            {
                var xPos = (x - (int)Min.X);
                var yPos = (y - (int)Min.Y);
                int zPos = (z - (int)Min.Z);

                int xI = xPos / _size.SideLength;
                int yI = yPos / _size.SideLength;
                int zI = zPos / _size.SideLength;

#if DEBUG
                Debug.Assert(xI >= 0 && xI < 2);
                Debug.Assert(yI >= 0 && yI < 2);
                Debug.Assert(zI >= 0 && zI < 2);
#endif

                int octreeIndex = xI + yI * 2 + zI * 4;

                var leaf = _data[octreeIndex];

                if (leaf != null)
                    return leaf[xPos % _size.SideLength, yPos % _size.SideLength, zPos % _size.SideLength];

                return default(sbyte);
            }
            set
            {
                var xPos = (x - (int)Min.X);
                var yPos = (y - (int)Min.Y);
                int zPos = (z - (int)Min.Z);

                int xI = xPos / _size.SideLength;
                int yI = yPos / _size.SideLength;
                int zI = zPos / _size.SideLength;

#if DEBUG
                Debug.Assert(xI >= 0 && xI < 2);
                Debug.Assert(yI >= 0 && yI < 2);
                Debug.Assert(zI >= 0 && zI < 2);
#endif

                int octreeIndex = xI + yI * 2 + zI * 4;

                var leaf = _data[octreeIndex];
                if (leaf == null)
                {
                    leaf = new VolumeLeaf(_size);
                    _data[octreeIndex] = leaf;
                }

                leaf[xPos % _size.SideLength, yPos % _size.SideLength, zPos % _size.SideLength] = value;
            }
        }

        public void Wipe(sbyte value)
        {
            for (int i = 0; i < 8; i++)
            {
                _data[i].Wipe(value);
            }
        }

        public bool Compact()
        {
            bool isEmpty = true;
            for (int i = 0; i < 8; i++)
            {
                if (_data[i].Compact())
                    _data[i] = null;
                else
                    isEmpty = false;
            }
            return isEmpty;
        }

        #endregion
    }

    public class VolumeLeaf : IVolume
    {
        private readonly VolumeSize _size;
        private sbyte[] _data;

        public VolumeLeaf(VolumeSize size)
        {
            _size = size;
            _data = new sbyte[_size.SideLengthCubed];
        }

        public void Wipe(sbyte value)
        {
            for (int i = 0; i < _size.SideLengthCubed; i++)
            {
                _data[i] = value;
            }
        }

        public bool Compact()
        {
            for (int i = 0; i < _size.SideLengthCubed; i++)
            {
                if (!default(sbyte).Equals(_data[i]))
                    return false;
            }

            return true;
        }

        public sbyte this[int x, int y, int z]
        {
            get { return _data[x + y * _size.SideLength + z * _size.SideLengthSquared]; }
            set { _data[x + y * _size.SideLength + z * _size.SideLengthSquared] = value; }
        }

        public sbyte this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }
    }
}