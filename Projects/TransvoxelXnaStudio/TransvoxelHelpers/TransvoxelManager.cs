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
using System.Diagnostics;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public class TransvoxelManager
    {
        private readonly GraphicsDevice _gd;
        private ConcurrentDictionary<Vector3, Chunk> _chunks;
        private TransvoxelExtractor _surfaceExtractor;
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

        public TransvoxelExtractor SurfaceExtractor
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


    //    int cnt = 0;

        public void ExtractMesh(OctreeNode<VolumeChunk> n)
        {

            if (n == null)
                return;

            Vector3i center = n.GetCenter();

            float dst = (float)Math.Sqrt(center.X * center.X + center.Y * center.Y + center.Z * center.Z);

            int lod = n.GetLevelOfDetail();

            if (lod == 1)//(dst >= 512 && lod == 3) || (dst < 512 && dst >= 128 && lod == 2) || (dst < 128 && lod == 1))
            {
                //            cnt++;
                //            if (cnt < 18 || cnt > 18)
                //               return;

                Vector3i position = n.GetPos();
                Vector3 posXna = position.ToVector3();

                Logger.GetLogger().Log(null, "" + dst);

                var m = _surfaceExtractor.GenLodCell(n);
                var v = Converters.ConvertMeshToXna(m, LodColors[lod - 1]);
                var i = m.GetIndices();

                var chunk = new Chunk
                {
                    BoundingBox = new BoundingBox(posXna, posXna + new Vector3(n.Size(), n.Size(), n.Size())),
                    Position = posXna,
                    Lod = lod
                };

                if (i.Length > 0)
                {
                    chunk.VertexBuffer = new VertexBuffer(_gd, typeof(VertexPositionTextureNormalColor), v.Length, BufferUsage.WriteOnly);
                    chunk.VertexBuffer.SetData(v);
                    chunk.IndexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, i.Length, BufferUsage.WriteOnly);
                    chunk.IndexBuffer.SetData(i);

                    Console.WriteLine("Chunk has : " + v.Length + " Vertices " + i.Length + " Indices");

                    if (_chunks.ContainsKey(posXna))
                    {
                        Chunk removed;
                        _chunks.TryRemove(posXna, out removed);
                    }
                    _chunks.TryAdd(posXna, chunk);
                }
            }
            else
            {

                for (int i = 0; i < 8; i++)
                {
                    ExtractMesh(n.GetChilds()[i]);
                }
            }
        }
    }
}