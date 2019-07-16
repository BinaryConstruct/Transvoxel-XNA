using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.VolumeHash
{
    public class VolumeDictionary<T> : IVolumeData<T>
    {
        private Dictionary<Vector3i, VolumeChunk<T>> _data;

        public VolumeDictionary(VolumeSize size)
        {
            _data = new Dictionary<Vector3i, VolumeChunk<T>>();
            _size = size;
        }

        private readonly VolumeSize _size;

        public VolumeSize Size
        {
            get { return _size; }
        }

        private Vector3i GetChunkIndex(int x, int y, int z)
        {
            int xI = x / Size.SideLength;
            if (x < 0) xI--;

            int yI = y / Size.SideLength;
            if (y < 0) yI--;

            int zI = z / Size.SideLength;
            if (z < 0) zI--;

            return new Vector3i(xI, yI, zI);
        }

        #region Implementation of IVolumeData<T>

        public T this[int x, int y, int z]
        {
            get
            {
                var chunkIndex = GetChunkIndex(x, y, z);
                if (!_data.ContainsKey(chunkIndex))
                    return default(T);

                int offsetIndex = (x - chunkIndex.X * Size.SideLength) +
                                  (y - chunkIndex.Y * Size.SideLength) * Size.SideLength +
                                  (z - chunkIndex.Z * Size.SideLength) * Size.SideLengthSquared;

                return _data[chunkIndex][offsetIndex];
            }
            set
            {
                var chunkIndex = GetChunkIndex(x, y, z);

                int offsetIndex = (x - chunkIndex.X * Size.SideLength) +
                                  (y - chunkIndex.Y * Size.SideLength) * Size.SideLength +
                                  (z - chunkIndex.Z * Size.SideLength) * Size.SideLengthSquared;

                VolumeChunk<T> chunk;
                if (!_data.ContainsKey(chunkIndex))
                    chunk = CreateChunk(chunkIndex);
                else
                    chunk = _data[chunkIndex];

                chunk[offsetIndex] = value;
            }
        }

        private VolumeChunk<T> CreateChunk(Vector3i chunkIndex)
        {
            var chunk = new VolumeChunk<T>(Size, chunkIndex * Size.SideLength);
            _data.Add(chunkIndex, chunk);
            return chunk;
        }

        public T this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }

        public T this[Vector3 v]
        {
            get { return this[(int)v.X, (int)v.Y, (int)v.Z]; }
            set { this[(int)v.X, (int)v.Y, (int)v.Z] = value; }
        }

        #endregion
    }

    public class VolumeChunk<T> : IVolumeData<T>
    {
        private T[] _data;
        private readonly VolumeSize _size;
        public readonly Vector3i Position;

        public VolumeChunk(VolumeSize size, Vector3i position)
        {
            _size = size;
            Position = position;
            _data = new T[_size.SideLengthCubed];
        }

        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        #region Implementation of IVolumeData<T>

        public T this[int x, int y, int z]
        {
            get { return _data[x + y * _size.SideLength + z * _size.SideLengthSquared]; }
            set { _data[x + y * _size.SideLength + z * _size.SideLengthSquared] = value; }
        }

        public T this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }

        public T this[Vector3 v]
        {
            get { return this[(int)v.X, (int)v.Y, (int)v.Z]; }
            set { this[(int)v.X, (int)v.Y, (int)v.Z] = value; }
        }

        public VolumeSize Size { get { return _size; } }

        #endregion
    }
}