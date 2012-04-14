using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using TransvoxelXnaStudio.Framework;
using Transvoxel.VolumeData;
using Transvoxel.VolumeData.CompactOctree;
using System.Threading;
using Transvoxel.Math;
using TransvoxelXnaStudio.TransvoxelHelpers;

namespace TransvoxelXnaStudio
{
    public partial class MainForm : Form
    {
        private Logger _logger;
        private System.Windows.Forms.Timer _updateTimer;
        private TaskFactory _uiFactory;
        private readonly TaskScheduler _uiScheduler;

        public MainForm()
        {
            InitializeComponent();

            // Create UI taskscheduler and factory
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _uiFactory = new TaskFactory(_uiScheduler);

            // Get the logger
            _logger = Logger.GetLogger();
            _logger.Logged += OnLogged;

            // create an update timer
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 500;
            _updateTimer.Tick += _updateTimer_Tick;
            _updateTimer.Start();

            propertyGrid1.SelectedObject = previewWindow1.Settings;
        }

        void _updateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatusText();
        }

        private void OnLogged(object sender, EventArgs<string> e)
        {
            if (outputTextbox.InvokeRequired)
            {
                _uiFactory.StartNew(() => OnLogged(sender, e));
                //outputTextbox.BeginInvoke(new Action(() => OnLogged(sender, e)));
                return;
            }

            outputTextbox.Text += string.Format("{0:HH:mm:ss.ffff}: {1} - {2}\r\n", DateTime.Now, sender, e.Value1);
            outputTextbox.SelectionStart = outputTextbox.Text.Length;
            outputTextbox.ScrollToCaret();
            outputTextbox.Refresh();
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (mainStatusBar.InvokeRequired)
            {
                _uiFactory.StartNew(() => OnProgressChanged(sender, e));
                
                //mainStatusBar.BeginInvoke(new Action(() => OnProgressChanged(sender, e)));
                return;
            }

            toolStripProgressText.Text = e.UserState.ToString();
            int pct = e.ProgressPercentage;
            if (pct < 0) pct = 0;
            if (pct > 100) pct = 100;
            toolStripProgressBar.Value = pct;
        }



        private void genVolBtn_Click(object sender, EventArgs e)
        {
            int size = previewWindow1.Settings.PreGeneratedRandomSize;
            
            _logger.Log("MAIN", "Generating Volume Data...");
            Task.Factory.StartNew(
                () =>
                {
                    for (int i = 0; i <= size; i++)
                    {
                        _logger.Log("MAIN", i+"/"+size);
                        OnProgressChanged(null, new ProgressChangedEventArgs((int)(((float)i/(float)size) * 100.0f), "Generating Volume Data..."));
                        for (int j = 0; j <= size; j++)
                        {
                            for (int k = 0; k <= size; k++)
                            {
                                double div = 64.0;
                                double val = (SimplexNoise.noise((i) / div, (j) / div, (k) / div)) * 128.0;

                                //val = -100;

                                previewWindow1.TransvoxelManager.VolumeData[i,j,k] = (sbyte)val;
                            }
                        }
                        //OnProgressChanged(null, new ProgressChangedEventArgs(100, "Generating Volume Data..."));
                    }
                    OnProgressChanged(null, new ProgressChangedEventArgs(0, string.Empty));
                    _logger.Log("MAIN", "Volume Data Generation Complete.");

                   // Console.WriteLine(previewWindow1.TransvoxelManager.VolumeData.ToString());
                }
                );
        }

        private void threaded(object dummy)
        {
             
        }

        private void extractMeshBtn_Click(object sender, EventArgs e)
        {
            previewWindow1.TransvoxelManager.Chunks = new System.Collections.Concurrent.ConcurrentDictionary<Vector3, TransvoxelHelpers.Chunk>();
            previewWindow1.TransvoxelManager.SurfaceExtractor.UseCache = previewWindow1.Settings.ReuseVert;
            previewWindow1.TransvoxelManager.VolumeData.ChunkSize = previewWindow1.Settings.ChunkSize;
            _logger.Log("MAIN", "Extracting Mesh...");

            

            new Thread(
                () =>
                {
                    //Thread.CurrentThread.IsBackground = true;

                    TransvoxelManager tvm = previewWindow1.TransvoxelManager;
                    HashedVolume<sbyte> v = (HashedVolume<sbyte>)tvm.VolumeData;
                    foreach(WorldChunk<sbyte> c in v.data.Values)
                    {
                        tvm.ExtractMesh(c);
                    }
                    
                    _logger.Log("MAIN", "Mesh Extraction Complete.");
                }

                ).Start();
        }

        private void UpdateStatusText()
        {
            toolStripStatusLabel1.Text = string.Format("Yaw: {0} Pitch: {1} Roll:{2} Position: {3}", previewWindow1.Camera.Yaw, previewWindow1.Camera.Position, previewWindow1.Camera.Roll, previewWindow1.Camera.Position);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //propertyGrid1.SelectedObject = e.Node.n
        }
    }
}
