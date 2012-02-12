namespace TransvoxelXnaStudio
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolArea = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.genVolBtn = new System.Windows.Forms.Button();
            this.extractMeshBtn = new System.Windows.Forms.Button();
            this.mainStatusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusExpander = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logArea = new System.Windows.Forms.Panel();
            this.outputTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editorArea = new System.Windows.Forms.Panel();
            this.previewWindow1 = new TransvoxelXnaStudio.GameWindow.PreviewWindow();
            this.toolArea.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mainStatusBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.logArea.SuspendLayout();
            this.editorArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolArea
            // 
            this.toolArea.Controls.Add(this.groupBox1);
            this.toolArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolArea.Location = new System.Drawing.Point(0, 0);
            this.toolArea.Name = "toolArea";
            this.toolArea.Size = new System.Drawing.Size(250, 492);
            this.toolArea.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.genVolBtn);
            this.groupBox1.Controls.Add(this.extractMeshBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initialization";
            // 
            // genVolBtn
            // 
            this.genVolBtn.Location = new System.Drawing.Point(6, 19);
            this.genVolBtn.Name = "genVolBtn";
            this.genVolBtn.Size = new System.Drawing.Size(220, 23);
            this.genVolBtn.TabIndex = 0;
            this.genVolBtn.Text = "Gen Volume Data";
            this.genVolBtn.UseVisualStyleBackColor = true;
            this.genVolBtn.Click += new System.EventHandler(this.genVolBtn_Click);
            // 
            // extractMeshBtn
            // 
            this.extractMeshBtn.Location = new System.Drawing.Point(6, 48);
            this.extractMeshBtn.Name = "extractMeshBtn";
            this.extractMeshBtn.Size = new System.Drawing.Size(220, 23);
            this.extractMeshBtn.TabIndex = 1;
            this.extractMeshBtn.Text = "Extract Mesh";
            this.extractMeshBtn.UseVisualStyleBackColor = true;
            this.extractMeshBtn.Click += new System.EventHandler(this.extractMeshBtn_Click);
            // 
            // mainStatusBar
            // 
            this.mainStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusExpander,
            this.toolStripProgressText,
            this.toolStripProgressBar});
            this.mainStatusBar.Location = new System.Drawing.Point(0, 616);
            this.mainStatusBar.Name = "mainStatusBar";
            this.mainStatusBar.Size = new System.Drawing.Size(864, 22);
            this.mainStatusBar.TabIndex = 0;
            this.mainStatusBar.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusExpander
            // 
            this.toolStripStatusExpander.Name = "toolStripStatusExpander";
            this.toolStripStatusExpander.Size = new System.Drawing.Size(591, 17);
            this.toolStripStatusExpander.Spring = true;
            // 
            // toolStripProgressText
            // 
            this.toolStripProgressText.Name = "toolStripProgressText";
            this.toolStripProgressText.Size = new System.Drawing.Size(38, 17);
            this.toolStripProgressText.Text = "status";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(864, 24);
            this.mainMenu.TabIndex = 2;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // logArea
            // 
            this.logArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logArea.Controls.Add(this.outputTextbox);
            this.logArea.Controls.Add(this.label1);
            this.logArea.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logArea.Location = new System.Drawing.Point(0, 492);
            this.logArea.Name = "logArea";
            this.logArea.Size = new System.Drawing.Size(864, 100);
            this.logArea.TabIndex = 3;
            // 
            // outputTextbox
            // 
            this.outputTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.outputTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outputTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.outputTextbox.Location = new System.Drawing.Point(0, 17);
            this.outputTextbox.Multiline = true;
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.ReadOnly = true;
            this.outputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextbox.Size = new System.Drawing.Size(862, 81);
            this.outputTextbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output";
            // 
            // editorArea
            // 
            this.editorArea.Controls.Add(this.previewWindow1);
            this.editorArea.Controls.Add(this.toolArea);
            this.editorArea.Controls.Add(this.logArea);
            this.editorArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorArea.Location = new System.Drawing.Point(0, 24);
            this.editorArea.Name = "editorArea";
            this.editorArea.Size = new System.Drawing.Size(864, 592);
            this.editorArea.TabIndex = 4;
            // 
            // previewWindow1
            // 
            this.previewWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewWindow1.Location = new System.Drawing.Point(250, 0);
            this.previewWindow1.Name = "previewWindow1";
            this.previewWindow1.Size = new System.Drawing.Size(614, 492);
            this.previewWindow1.TabIndex = 1;
            this.previewWindow1.Text = "previewWindow1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 638);
            this.Controls.Add(this.editorArea);
            this.Controls.Add(this.mainStatusBar);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Transvoxel Xna Studio";
            this.toolArea.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.mainStatusBar.ResumeLayout(false);
            this.mainStatusBar.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.logArea.ResumeLayout(false);
            this.logArea.PerformLayout();
            this.editorArea.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel toolArea;
        private GameWindow.PreviewWindow previewWindow1;
        private System.Windows.Forms.StatusStrip mainStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusExpander;
        private System.Windows.Forms.ToolStripStatusLabel toolStripProgressText;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel logArea;
        private System.Windows.Forms.Panel editorArea;
        private System.Windows.Forms.TextBox outputTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button extractMeshBtn;
        private System.Windows.Forms.Button genVolBtn;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}

