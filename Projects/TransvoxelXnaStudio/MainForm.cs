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

namespace TransvoxelXnaStudio
{
    public partial class MainForm : Form
    {
        private Logger _logger;
        private Timer _updateTimer;

        public MainForm()
        {
            InitializeComponent();
            _logger = Logger.GetLogger();
            _logger.Logged += OnLogged;
            _updateTimer = new Timer();
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
                outputTextbox.BeginInvoke(new Action(() => OnLogged(sender, e)));
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
                mainStatusBar.BeginInvoke(new Action(() => OnProgressChanged(sender, e)));
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
            Task.Factory.StartNew(
                () =>
                {
                    for (int i = 0; i < 4; i++)
                    {
                        OnProgressChanged(null, new ProgressChangedEventArgs((int)((i / 4f) * 100.0f), "Generating Volume Data..."));
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                Vector3 position = new Vector3(i * 16, j * 16, k * 16);
                                previewWindow1.TransvoxelManager.GenerateVolumeData(position);
                            }
                        }
                        OnProgressChanged(null, new ProgressChangedEventArgs(100, "Generating Volume Data..."));
                    }
                    OnProgressChanged(null, new ProgressChangedEventArgs(0, string.Empty));
                }
                );
        }

        private void extractMeshBtn_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    for (int i = 0; i < 4; i++)
                    {
                        OnProgressChanged(null, new ProgressChangedEventArgs((int)((i / 4f) * 100.0f), "Extracting Meshes..."));
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                Vector3 position = new Vector3(i * 16, j * 16, k * 16);
                                previewWindow1.TransvoxelManager.ExtractMesh(position, 1);
                            }
                        }
                        OnProgressChanged(null, new ProgressChangedEventArgs(100, "Extracting Meshes..."));
                    }
                    OnProgressChanged(null, new ProgressChangedEventArgs(0, string.Empty));
                }
                );
        }

        private void UpdateStatusText()
        {
            toolStripStatusLabel1.Text = string.Format("Yaw: {0} Pitch: {1} Roll:{2} Position: {3}", previewWindow1.Camera.Yaw, previewWindow1.Camera.Position, previewWindow1.Camera.Roll, previewWindow1.Camera.Position);
        }
    }
}
