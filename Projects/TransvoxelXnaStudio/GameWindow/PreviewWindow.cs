using System;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Transvoxel.Math;
using Transvoxel.SurfaceExtractor;
using Transvoxel.VolumeData;
using Transvoxel.VolumeData.CompactOctree;
using TransvoxelXnaStudio.Host;
using TransvoxelXnaStudio.TransvoxelHelpers;

namespace TransvoxelXnaStudio.GameWindow
{
    public class PreviewWindow : GraphicsDeviceControl
    {
        private System.Timers.Timer updateTimer;
        BasicEffect _effect;
        BasicEffect mBoundingBoxEffect;
        Stopwatch timer;
        private Camera _cam;
        private Logger _logger;
        private string _logSender;

        private TransvoxelManager _tvm;

        VertexPositionColor[] verts;

        #region Overrides of GraphicsDeviceControl

        protected override void Initialize()
        {
            _logger = Logger.GetLogger();
            _logSender = "PreviewWindow";
            _logger.Log(_logSender, "Initializing...");

            

            _cam = new Camera(GraphicsDevice);
            _cam.currentCameraMode = Camera.CameraMode.free;
            _cam.ResetCamera();
            _cam.SetFreeCamPosition(new Vector3(64, 50, 150));

            _tvm = new TransvoxelManager(GraphicsDevice);
            
            // Create our effect.
            _effect = new BasicEffect(GraphicsDevice);

            mBoundingBoxEffect = new BasicEffect(GraphicsDevice);
            mBoundingBoxEffect.LightingEnabled = false;
            mBoundingBoxEffect.VertexColorEnabled = true;

            _effect.VertexColorEnabled = true;

            // Start the animation timer.
            timer = Stopwatch.StartNew();
            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
            _logger.Log(_logSender, "Initialization Complete.");

            updateTimer = new System.Timers.Timer(1 / 30f);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += Update;
            updateTimer.Start();
            verts = new VertexPositionColor[24];
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            _cam.Update(Matrix.Identity);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            // Spin the triangle according to how much time has passed.
            float time = (float)timer.Elapsed.TotalSeconds;
            float aspect = GraphicsDevice.Viewport.AspectRatio;

            DrawSolids(time);
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            if (graphicsDeviceService != null && GraphicsDevice != null)
                _cam.ResetCamera();
        }

        public Camera Camera
        {
            get { return _cam; }
        }
        public TransvoxelManager TransvoxelManager
        {
            get { return _tvm; }
        }

        #endregion


        private void DrawSolids(float time)
        {
            //float rotation = time * 0.2f;
            //Matrix world = Matrix.CreateRotationY(rotation);
            //Matrix view = Matrix.CreateLookAt(new Vector3(100, 25, 100), Vector3.Zero, Vector3.Up);
            //Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), GraphicsDevice.Viewport.AspectRatio, 0.01f, 1000f);

            _effect.World = Matrix.Identity;
            _effect.View = _cam.ViewMatrix;
            _effect.Projection = _cam.ProjectionMatrix;
            _effect.EnableDefaultLighting();

            Matrix viewProj = _cam.ViewMatrix * _cam.ProjectionMatrix;

            var viewFastFrustrum = new FastFrustum(ref viewProj);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            var raster = GraphicsDevice.RasterizerState;


            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (Vector3 key in _tvm.Chunks.Keys)
                {
                    Chunk chunk;
                    _tvm.Chunks.TryGetValue(key, out chunk);
                    if (chunk == null) continue;
                    //if (viewFastFrustrum.Intersects(chunk.BoundingBox) && chunk.IndexBuffer != null)
                    //if (chunk.IndexBuffer != null)
                    {
                        //todo: can this be here?
                        //if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);

                        if (chunk.IndexBuffer.IndexCount > 0)
                        {
                            GraphicsDevice.SetVertexBuffer(chunk.VertexBuffer);
                            GraphicsDevice.Indices = chunk.IndexBuffer;
                            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.VertexBuffer.VertexCount, 0, chunk.IndexBuffer.IndexCount / 3);
                        }
                    }
                }
            }

            GraphicsDevice.RasterizerState = new RasterizerState { FillMode = FillMode.WireFrame };


            mBoundingBoxEffect.World = Matrix.Identity;
            mBoundingBoxEffect.View = _cam.ViewMatrix;
            mBoundingBoxEffect.Projection = _cam.ProjectionMatrix;
            foreach (EffectPass pass in mBoundingBoxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (Vector3 key in _tvm.Chunks.Keys)
                {
                    Chunk chunk;
                    _tvm.Chunks.TryGetValue(key, out chunk);
                    if (chunk == null) continue;

                    SetupBoundingBox(chunk.BoundingBox, TransvoxelManager.LodColors[chunk.Lod - 1]);
                    //if (viewFastFrustrum.Intersects(chunk.BoundingBox) && chunk.IndexBuffer != null)
                    //if (chunk.IndexBuffer != null)
                    {
                        //todo: can this be here?
                        //if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);

                        if (chunk.IndexBuffer.IndexCount > 0)
                        {

                            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, verts, 0, 12);
                        }
                    }
                }
            }

            GraphicsDevice.RasterizerState = raster;
        }

        public void SetupBoundingBox(BoundingBox box, Color color)
        {
            Vector3 min = box.Min;
            Vector3 max = box.Max;

            verts[0].Position = new Vector3(min.X, min.Y, min.Z);
            verts[1].Position = new Vector3(max.X, min.Y, min.Z);
            verts[2].Position = new Vector3(min.X, min.Y, max.Z);
            verts[3].Position = new Vector3(max.X, min.Y, max.Z);
            verts[4].Position = new Vector3(min.X, min.Y, min.Z);
            verts[5].Position = new Vector3(min.X, min.Y, max.Z);
            verts[6].Position = new Vector3(max.X, min.Y, min.Z);
            verts[7].Position = new Vector3(max.X, min.Y, max.Z);
            verts[8].Position = new Vector3(min.X, max.Y, min.Z);
            verts[9].Position = new Vector3(max.X, max.Y, min.Z);
            verts[10].Position = new Vector3(min.X, max.Y, max.Z);
            verts[11].Position = new Vector3(max.X, max.Y, max.Z);
            verts[12].Position = new Vector3(min.X, max.Y, min.Z);
            verts[13].Position = new Vector3(min.X, max.Y, max.Z);
            verts[14].Position = new Vector3(max.X, max.Y, min.Z);
            verts[15].Position = new Vector3(max.X, max.Y, max.Z);
            verts[16].Position = new Vector3(min.X, min.Y, min.Z);
            verts[17].Position = new Vector3(min.X, max.Y, min.Z);
            verts[18].Position = new Vector3(max.X, min.Y, min.Z);
            verts[19].Position = new Vector3(max.X, max.Y, min.Z);
            verts[20].Position = new Vector3(min.X, min.Y, max.Z);
            verts[21].Position = new Vector3(min.X, max.Y, max.Z);
            verts[22].Position = new Vector3(max.X, min.Y, max.Z);
            verts[23].Position = new Vector3(max.X, max.Y, max.Z);

            for (int i = 0; i < 24; i++)
            {
                verts[i].Color = color;
            }

        }


    }
}