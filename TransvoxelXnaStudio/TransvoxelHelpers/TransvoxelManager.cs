using System;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Transvoxel.Math;
using Transvoxel.SurfaceExtractor;
using Transvoxel.VolumeData;
using Transvoxel.VolumeData.VolumeHash;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public class TransvoxelManager
    {
        private readonly GraphicsDevice _gd;
        private ConcurrentDictionary<Vector3, Chunk> _chunks;
        private TransvoxelExtractor _surfaceExtractor;
        private IVolumeData<sbyte> _volumeData;
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
            _volumeData = new VolumeDictionary<sbyte>(new VolumeSize(8));
            _logger.Log(_logSend, "Creating TransvoxelExtractor");
            _surfaceExtractor = new TransvoxelExtractor(_volumeData);
        }

        public TransvoxelExtractor SurfaceExtractor
        {
            get { return _surfaceExtractor; }
        }

        public IVolumeData<sbyte> VolumeData
        {
            get { return _volumeData; }
        }

        public ConcurrentDictionary<Vector3, Chunk> Chunks
        {
            get { return _chunks; }
            set { _chunks = value; }
        }

        public static Color[] LodColors = new Color[]
        {
            Color.Green,
            Color.Yellow,
            Color.Red,
            Color.AliceBlue,
        };

        public void ExtractMesh(int size, int lod)
        {

            int numchunks = size / VolumeData.Size.SideLength;
            for (int j = 0; j < numchunks; j++)
            {
                for (int h = 0; h < numchunks; h++)
                {


                    for (int k = 0; k < numchunks; k++)
                    {

                        Vector3i position = new Vector3i(h, j, k) * 8 * lod;
                        Vector3 posXna = position.ToVector3();

                        //Logger.GetLogger().Log(null, "" + dst);

                        var m = _surfaceExtractor.GenLodRegion(position, 8, lod);
                        //var m = _surfaceExtractor.GenLodCell(n);
                        var v = Converters.ConvertMeshToXna(m, LodColors[lod % LodColors.Length]);
                        var i = m.GetIndices();

                        var chunk = new Chunk
                        {
                            BoundingBox = new BoundingBox(posXna, posXna + new Vector3(8, 8, 8) * lod),
                            Position = posXna,
                            Lod = lod
                        };

                        if (i.Length > 0 && v.Length > 0)
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
                }
            }
        }
    }
}