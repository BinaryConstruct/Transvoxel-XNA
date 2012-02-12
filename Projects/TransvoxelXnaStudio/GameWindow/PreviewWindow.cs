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
        Stopwatch timer;
        private Camera _cam;
        private Logger _logger;
        private string _logSender;

        private TransvoxelManager _tvm;

        #region Overrides of GraphicsDeviceControl

        protected override void Initialize()
        {



            _logger = Logger.GetLogger();
            _logSender = "PreviewWindow";
            _logger.Log(_logSender, "Initializing...");

            _cam = new Camera();
            _cam.currentCameraMode = Camera.CameraMode.free;
            _cam.ResetCamera();

            _tvm = new TransvoxelManager(GraphicsDevice);
            // Create our effect.
            _effect = new BasicEffect(GraphicsDevice);
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
        }

        protected void Update(object sender, ElapsedEventArgs e)
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
            float rotation = time * 0.2f;
            Matrix world = Matrix.CreateRotationY(rotation);
            Matrix view = Matrix.CreateLookAt(new Vector3(100,25,100), Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, GraphicsDevice.Viewport.AspectRatio,
                                                                0.01f, 1000f);

            //_effect.World = Matrix.Identity;
            //_effect.View = _cam.ViewMatrix;
            //_effect.Projection = _cam.ProjectionMatrix;

            //Matrix viewProj = _cam.ViewMatrix * _cam.ProjectionMatrix;

            Update(null, null);
            _effect.World = world;
            _effect.View = _cam.ViewMatrix;
            _effect.Projection = _cam.ProjectionMatrix;
            _effect.EnableDefaultLighting();

            Matrix viewProj = view * projection;
            var viewFastFrustrum = new FastFrustum(ref viewProj);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (Vector3 key in _tvm.Chunks.Keys)
                {
                    Chunk chunk;
                    _tvm.Chunks.TryGetValue(key, out chunk);
                    if (chunk == null) continue;
                    if (viewFastFrustrum.Intersects(chunk.BoundingBox) && chunk.IndexBuffer != null)
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
        }


    }
}