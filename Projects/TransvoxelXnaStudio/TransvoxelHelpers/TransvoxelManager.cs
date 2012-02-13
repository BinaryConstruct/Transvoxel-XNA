using System;
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

        public static Color[] LodColors = new Color[]
                                             {
                                                 Color.Green,
                                                 Color.Yellow,
                                                 Color.Red
                                             };

        public void ExtractMesh(OctreeNode n)
        {
            if (n == null)
                return;

            Vector3i center = n.GetCenter();

            float dst = (float)Math.Sqrt(center.X * center.X + center.Y * center.Y + center.Z * center.Z);

            int lod = n.GetLevelOfDetail();


            if ((dst >= 128 && lod == 3) || (dst < 128 && dst >= 64 && lod == 2) || (dst < 64 && lod == 1))
            {
                Logger.GetLogger().Log(null, "Distance: " + dst);
                Vector3i position = n.GetPos();
                Vector3 posXna = Converters.Vector3iToVector3(position);
                //int lod = n.GetLevelOfDetail();

                var m = _surfaceExtractor.GenLodCell(n);
                var v = Converters.ConvertMeshToXna(m, LodColors[lod-1]);
                var i = m.GetIndices();
                var chunk = new Chunk
                {
                    BoundingBox = new BoundingBox(posXna, posXna + new Vector3(VolumeChunk.CHUNKSIZE, VolumeChunk.CHUNKSIZE, VolumeChunk.CHUNKSIZE)),
                    Position = posXna,
                    Lod = lod
                };

                if (i.Length > 0)
                {
                    chunk.VertexBuffer = new VertexBuffer(_gd, typeof(VertexPositionTextureNormalColor), v.Length, BufferUsage.WriteOnly);
                    chunk.VertexBuffer.SetData(v);
                    chunk.IndexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, i.Length, BufferUsage.WriteOnly);
                    chunk.IndexBuffer.SetData(i);
                }
                
                if (_chunks.ContainsKey(posXna))
                {
                    Chunk removed;
                    _chunks.TryRemove(posXna, out removed);
                }
                _chunks.TryAdd(posXna, chunk);
            }
            else 
            {
                if(n is OctreeChildNode)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        OctreeChildNode node = (OctreeChildNode)n;
                        ExtractMesh(node.GetChilds()[i]);
                    }
                }
            }
        }

        
        /*public void GenerateVolumeData(Vector3 position)
        {
           
            for (int x = 0; x < VolumeChunk.CHUNKSIZE; x++)
            {
                int localX = (int)position.X + x;
                for (int y = 0; y < VolumeChunk.CHUNKSIZE; y++)
                {
                    int localY = (int)position.Y + y;
                    for (int z = 0; z < VolumeChunk.CHUNKSIZE; z++)
                    {
                        int localZ = (int)position.Z + z;
                        double div = 31.0;
                        double val = (SimplexNoise.noise(localX / div, localY / div, localZ /div)) * 128.0;
                        _volumeData[localX, localY, localZ] = (sbyte)val;
                    }
                }
            }
        }*/
    }
}