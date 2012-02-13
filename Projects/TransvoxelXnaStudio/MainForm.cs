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

namespace TransvoxelXnaStudio
{
    public partial class MainForm : Form
    {
        private Logger _logger;
        private System.Windows.Forms.Timer _updateTimer;
        private TaskFactory _uiFactory;
        private readonly TaskScheduler uiScheduler;

        public MainForm()
        {
            InitializeComponent();
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _uiFactory = new TaskFactory(uiScheduler);
            _logger = Logger.GetLogger();
            _logger.Logged += OnLogged;
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 500;
            _updateTimer.Tick += _updateTimer_Tick;
            _updateTimer.Start();
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

        int sizex = 50;
        int sizey = 50;
        int sizez = 50;


        private void genVolBtn_Click(object sender, EventArgs e)
        {
            _logger.Log("MAIN", "Generating Volume Data...");
            Task.Factory.StartNew(
                () =>
                {
                    for (int i = 0; i <= sizex; i++)
                    {
                        _logger.Log("MAIN", i+"/"+sizex);
                        OnProgressChanged(null, new ProgressChangedEventArgs((int)(((float)i/(float)sizex) * 100.0f), "Generating Volume Data..."));
                        for (int j = 0; j <= sizey; j++)
                        {
                            for (int k = 0; k <= sizez; k++)
                            {
                                double div = 31.0;
                                double val = (SimplexNoise.noise(i / div, j / div, k / div)) * 128.0;
                                previewWindow1.TransvoxelManager.VolumeData[i,j,k] = (sbyte)val;
                            }
                        }
                        //OnProgressChanged(null, new ProgressChangedEventArgs(100, "Generating Volume Data..."));
                    }
                    OnProgressChanged(null, new ProgressChangedEventArgs(0, string.Empty));
                    _logger.Log("MAIN", "Volume Data Generation Complete.");
                }
                );
        }

        private void threaded(object dummy)
        {
             
        }

        private void extractMeshBtn_Click(object sender, EventArgs e)
        {
            _logger.Log("MAIN", "Extracting Mesh...");
            Task.Factory.StartNew(
                () =>
                {
                    
                    IVolumeData v = previewWindow1.TransvoxelManager.VolumeData;
                    CompactOctree o = (CompactOctree)v;
                    previewWindow1.TransvoxelManager.ExtractMesh(o.Head());
                    _logger.Log("MAIN", "Mesh Extraction Complete.");
                }

                );
        }

        private void UpdateStatusText()
        {
            toolStripStatusLabel1.Text = string.Format("Yaw: {0} Pitch: {1} Roll:{2} Position: {3}", previewWindow1.Camera.Yaw, previewWindow1.Camera.Position, previewWindow1.Camera.Roll, previewWindow1.Camera.Position);
        }
    }
}
