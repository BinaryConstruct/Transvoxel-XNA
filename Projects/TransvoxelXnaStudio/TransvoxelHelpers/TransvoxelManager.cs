using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Transvoxel.Math;
using Transvoxel.SurfaceExtractor;
using Transvoxel.VolumeData;
using System.Linq;
using Transvoxel.VolumeData.CompactOctree;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public class TransvoxelManager
    {
        private readonly GraphicsDevice _gd;
        private ConcurrentDictionary<Vector3, Chunk> _chunks;
        private ISurfaceExtractor _surfaceExtractor;
        private IVolumeData _volumeData;
        private Logger _logger;
        private string _logSend;

        public TransvoxelManager(GraphicsDevice gd)
        {
            _gd = gd;
            _logger = Logger.GetLogger();
            _chunks = new ConcurrentDictionary<Vector3, Chunk>();

            // Initialize Transvoxel
            _logSend = "TransvoxelManager";
            _logger.Log(_logSend, "Creating Octree");
            _volumeData = new CompactOctree();
            _logger.Log(_logSend, "Creating TransvoxelExtractor");
            _surfaceExtractor = new TransvoxelExtractor(_volumeData);
        }

        public ISurfaceExtractor SurfaceExtractor
        {
            get { return _surfaceExtractor; }
        }

        public IVolumeData VolumeData
        {
            get { return _volumeData; }
        }

        public ConcurrentDictionary<Vector3, Chunk> Chunks
        {
            get { return _chunks; }
        }

        public void ExtractMesh(Vector3 position, int lod)
        {
            var m = _surfaceExtractor.GenLodCell(Converters.Vector3ToVector3i(position), 1);
            var v = Converters.ConvertMeshToXna(m);
            var i = m.GetIndices();
            var chunk = new Chunk
                            {
                                BoundingBox = new BoundingBox(position, position + new Vector3(TransvoxelExtractor.BlockWidth, TransvoxelExtractor.BlockWidth, TransvoxelExtractor.BlockWidth)),
                                Position = position
                            };

            chunk.VertexBuffer = new VertexBuffer(_gd, typeof(VertexPositionTextureNormalColor), v.Length, BufferUsage.WriteOnly);
            chunk.VertexBuffer.SetData(v);
            chunk.IndexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, i.Length, BufferUsage.WriteOnly);
            chunk.IndexBuffer.SetData(i);

            if (_chunks.ContainsKey(position))
            {
                Chunk removed;
                _chunks.TryRemove(position, out removed);
            }
            _chunks.TryAdd(position, chunk);
        }

        public void GenerateVolumeData(Vector3 position)
        {
            for (int x = 0; x < TransvoxelExtractor.BlockWidth; x++)
            {
                int localX = (int)position.X + x;
                for (int y = 0; y < TransvoxelExtractor.BlockWidth; y++)
                {
                    int localY = (int)position.Y + y;
                    for (int z = 0; z < TransvoxelExtractor.BlockWidth; z++)
                    {
                        int localZ = (int)position.Z + z;
                        var val = (sbyte) (SimplexNoise.noise(localX, localY, localZ)*2);
                        _volumeData[localX, localY, localZ] = val;
                    }
                }
            }
        }
    }
}